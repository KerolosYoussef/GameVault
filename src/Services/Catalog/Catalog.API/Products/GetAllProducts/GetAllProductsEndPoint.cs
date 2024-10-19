using Catalog.API.Categories.GetAllCategories;
using Catalog.API.Categories.Params;
using Catalog.API.Helpers;
using Catalog.API.Products.Dtos;
using Catalog.API.Products.Params;
using GameVault.Common.Results;

namespace Catalog.API.Products.GetAllProducts
{
    public record GetAllProductsReqeuest(ProductParams Params);
    public record GetAllProductsResponse(Pagination<ProductDto> Result);
    public class GetAllProductsEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", GetAllProducts)
                .WithName("GetAllProducts")
                .Produces<GetAllProductsResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get All Products")
                .WithDescription("Get All Products");
        }

        private static async Task<IResult> GetAllProducts(HttpContext httpContext, ISender sender)
        {
            // Extract query parameters
            var queryParameters = httpContext.Request.Query;

            // Create the request object dynamically
            var request = new GetAllProductsReqeuest(RequestQueryHandler.CreateRequestFromQuery<ProductParams>(queryParameters));

            // Map from request to query
            var query = request.Adapt<GetAllProductsQuery>();

            // Send query using mediator
            var result = await sender.Send(query);

            // Map result to response
            var response = result.Adapt<GetAllProductsResponse>();

            return Results.Ok(response);
        }
    }
}
