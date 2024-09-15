using Catalog.API.Categories.GetAllCategories;
using GameVault.Common.Interfaces.CQRS.Commands;

namespace Catalog.API.Categories.UpdateCategory
{
    public record UpdateCategoryRequest(Guid Id, string Name, string? Description, string? ImageUrl, bool Status, Guid? ParentCategoryId);
    public record UpdateCategoryResponse(Guid Id);
    public class UpdateCategoryEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/categories/{id}", UpdateCategory)
                .WithName("UpdateCategory")
                .Produces<UpdateCategoryResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Update Category")
                .WithDescription("UpdateCategory");
        }

        private async Task<IResult> UpdateCategory(UpdateCategoryRequest request, ISender sender)
        {
            // Map reqeust to comman
            var command = request.Adapt<UpdateCategoryCommand>();

            // Send command using mediator
            var result = await sender.Send(command);

            // Map result to response
            var response = result.Adapt<UpdateCategoryResponse>();

            // Return response
            return Results.Ok(response);
        }
    }
}
