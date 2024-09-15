using Catalog.API.Categories.Exceptions;
using GameVault.Common.Interfaces.CQRS.Commands;
using GameVault.Common.Interfaces.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Categories.UpdateCategory
{
    public record UpdateCategoryCommand(Guid Id, string Name, string? Description, string? ImageUrl, bool Status, Guid? ParentCategoryId) : ICommand<UpdateCategoryResult>;
    public record UpdateCategoryResult(Guid Id);
    internal class UpdateCategoryHandler : ICommandHandler<UpdateCategoryCommand, UpdateCategoryResult>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IImageHandler _imageHandler;

        public UpdateCategoryHandler(ApplicationDbContext applicationDbContext, IImageHandler imageHandler)
        {
            _applicationDbContext = applicationDbContext;
            _imageHandler = imageHandler;
        }

        public async Task<UpdateCategoryResult> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            // Check if there was an image uploaded
            string imageUrl = string.Empty;
            if (!string.IsNullOrEmpty(request.ImageUrl))
            {
                imageUrl = _imageHandler.SaveImageFromBase64(request.ImageUrl, $"Categories/Images");
            }

            // Get category from database
            Category? category = await _applicationDbContext.Set<Category>().SingleOrDefaultAsync(c => c.Id == request.Id);

            if (category is null)
                throw new CategoryExceptions.CategoryNotFoundException("Cannot find the category");

            // Update category
            category.Name = request.Name;
            category.Description = request.Description;
            category.ImageUrl = imageUrl;
            category.Status = request.Status;
            category.ParentCategoryId = request.ParentCategoryId;

            // Save to database
            _applicationDbContext.Set<Category>().Update(category);
            await _applicationDbContext.SaveChangesAsync();

            // return result
            return new UpdateCategoryResult(category.Id);
        }
    }
}
