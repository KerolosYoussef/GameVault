

using Catalog.API.Categories.UpdateCategory;

namespace Catalog.API.Categories.GetParentCategories
{
    public record GetParentCategoriesRequest();
    public record GetParentCategoriesResponse(List<Category> Categories);
    public class GetParentCatogriesEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/categories/parent", GetParentCatgories)
                .WithName("GetParentCatgories")
                .Produces<GetParentCategoriesResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Parent Categories")
                .WithDescription("Get Parent Categories");
        }

        private async Task<IResult> GetParentCatgories(ISender sender)
        {
            // Map request to query
            var query = new GetParentCategoriesQuery();

            // Send request using mediator
            var result = await sender.Send(query);

            // Map result to response
            var response = result.Adapt<GetParentCategoriesResponse>();

            // Return response
            return Results.Ok(response);
        }
    }
}
