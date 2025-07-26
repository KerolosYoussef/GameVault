using Catalog.API.Models;
using GameVault.Common.Interfaces.CQRS.Commands;
using GameVault.Common.Interfaces.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Catalog.API.Categories.CreateCategory
{
    public record CreateCategoryCommand(string Name, string? Description, string? ImageUrl, bool Status, Guid? ParentCategoryId) : ICommand<CreateCategoryResult>;
    public record CreateCategoryResult(Guid Id);
    internal class CreateCategoryHandler : ICommandHandler<CreateCategoryCommand, CreateCategoryResult>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IImageHandler _imageHandler;

        public CreateCategoryHandler(ApplicationDbContext applicationDbContext, IImageHandler imageHandler)
        {
            _applicationDbContext = applicationDbContext;
            _imageHandler = imageHandler;
        }

        public async Task<CreateCategoryResult> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            // Generate image url if image uploaded or return empty string
            string imageUrl = HandleCategoryImage(request);

            // Create category entity
            Category category = Category.Create(request.Name, request.Description, imageUrl, request.Status, request.ParentCategoryId);

            // Initialize Category DbSet
            DbSet<Category> categoryDbSet = _applicationDbContext.Set<Category>();

            // Check if the category name already exists
            if (await IsCategoryAlreadyExists(categoryDbSet, category.Name))
            {
                throw new CategoryExceptions.CategoryAlreadyExistsException("This category already exists");
            }

            // Save to database
            await categoryDbSet.AddAsync(category);
            await _applicationDbContext.SaveChangesAsync();

            // return result
            return new CreateCategoryResult(category.Id);
        }

        private string HandleCategoryImage(CreateCategoryCommand request)
        {
            string imageUrl = string.Empty;
            if (!string.IsNullOrEmpty(request.ImageUrl))
            {
                imageUrl = _imageHandler.SaveImageFromBase64(request.ImageUrl, $"Categories/Images");
            }

            return imageUrl;
        }
        private async Task<bool> IsCategoryAlreadyExists(DbSet<Category> categoryDbSet, string name) 
            => await categoryDbSet.AnyAsync(c => c.Name == name);

    }
}
