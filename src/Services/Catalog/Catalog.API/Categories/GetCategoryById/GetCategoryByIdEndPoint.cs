
using Catalog.API.Categories.CreateCategory;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Categories.GetCategoryById
{
    public record GetCategoryByIdRequest(Guid Id);
    public record GetCategoryByIdResponse(Category? Category);
    public class GetCategoryByIdEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/categories/{id}", GetCategoryById)
               .WithName("GetCategoryById")
               .Produces<GetCategoryByIdResponse>(StatusCodes.Status200OK)
               .ProducesProblem(StatusCodes.Status400BadRequest)
               .WithSummary("Get Category By Id")
               .WithDescription("Get Category By Id");
        }

        private async Task<IResult> GetCategoryById(Guid id, ISender sender)
        {
            // Create request
            GetCategoryByIdRequest request = new(id);

            // Map request to query
            GetCategoryByIdQuery query = request.Adapt<GetCategoryByIdQuery>();

            // Send query using mediator
            var result = await sender.Send(query);

            // Map result to response
            var response = result.Adapt<GetCategoryByIdResponse>();

            // return response
            return Results.Ok(response);
        }
    }
}
