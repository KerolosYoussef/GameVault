
using Catalog.API.Categories.GetAllCategories;
using Catalog.API.Categories.Params;
using Catalog.API.Helpers;
using Catalog.API.Warehouses.GetAllWarehouse;
using Catalog.API.Warehouses.Pramas;
using GameVault.Common.Results;

namespace Catalog.API.Warehouses.GetAllWarehouses
{
    public record GetAllWarehousesRequest(WarehouseParams Params);
    public record GetAllWarehouseResponse(Pagination<Warehouse> Result);
    public class GetAllWarehousesEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/warehouses", GetAllWarehouses)
                .WithName("GetAllWarehouses")
                .Produces<GetAllWarehouseResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get All Warehouses")
                .WithDescription("Get All Warehouses");
        }

        private static async Task<IResult> GetAllWarehouses(HttpContext httpContext, ISender sender)
        {
            // Extract query parameters
            var queryParameters = httpContext.Request.Query;

            // Create the request object dynamically
            var request = new GetAllWarehousesRequest(RequestQueryHandler.CreateRequestFromQuery<WarehouseParams>(queryParameters));

            // Map request to query
            var query = request.Adapt<GetAllWarehousesQuery>();

            // send query using mediator
            var result = await sender.Send(query);

            // Map result to response
            var response = result.Adapt<GetAllWarehouseResponse>();

            // return response
            return Results.Ok(response);
        }
    }
}
