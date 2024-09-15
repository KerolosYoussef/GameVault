
using Catalog.API.Categories.GetCategoryById;
using Catalog.API.Categories.Params;
using Catalog.API.Helpers;
using GameVault.Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Categories.GetAllCategories
{
    public record GetAllCategoriesRequest(CategoryParams Params);
    public record GetAllCategoriesResponse(Pagination<Category> Result);
    public class GetAllCategoriesEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/categories", GetAllCategories)
                .WithName("GetAllCategories")
                .Produces<GetAllCategoriesResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get All Categories")
                .WithDescription("Get All Categories");
        }

        private static async Task<IResult> GetAllCategories(HttpContext httpContext, ISender sender)
        {
            // Extract query parameters
            var queryParameters = httpContext.Request.Query;

            // Create the request object dynamically
            var request = new GetAllCategoriesRequest(RequestQueryHandler.CreateRequestFromQuery<CategoryParams>(queryParameters));

            // Map request to query
            var query = request.Adapt<GetAllCategoriesQuery>();

            // send query using mediator
            var result = await sender.Send(query);

            // Map result to response
            var response = result.Adapt<GetAllCategoriesResponse>();

            // return response
            return Results.Ok(response);
        }
    }
}
