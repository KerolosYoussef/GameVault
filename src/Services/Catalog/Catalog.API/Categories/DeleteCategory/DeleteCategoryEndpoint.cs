

using Catalog.API.Categories.GetParentCategories;

namespace Catalog.API.Categories.DeleteCategory
{
    public class DeleteCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/categories/{id}", DeleteCategory)
                .WithName("DeleteCategory")
                .WithSummary("Delete Category")
                .WithDescription("Delete Category");
        }

        private async Task DeleteCategory(Guid Id, ISender sender)
        {
            // Map request to command
            var command = new DeleteCategoryCommand(Id);

            // Send command using mediator
            await sender.Send(command);
        }
    }
}
