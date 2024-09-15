
using Catalog.API.Categories.Exceptions;
using GameVault.Common.Interfaces.CQRS.Commands;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Categories.DeleteCategory
{
    public record DeleteCategoryCommand(Guid Id) : ICommand;
    public class DeleteCategoryHandler : ICommandHandler<DeleteCategoryCommand>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DeleteCategoryHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            // Create category query
            var query = _applicationDbContext.Set<Category>();
            // Get Category from database
            var category = await query.SingleOrDefaultAsync(x =>  x.Id == request.Id);
            
            // Throw exception if category is null
            if(category is null)
            {
                throw new CategoryExceptions.CategoryNotFoundException();
            }

            // Soft delete
            category.DeletedAt = DateTime.UtcNow;

            // Save changes
            query.Update(category);
            await _applicationDbContext.SaveChangesAsync();

            // Return result
            return Unit.Value;
        }
    }
}
