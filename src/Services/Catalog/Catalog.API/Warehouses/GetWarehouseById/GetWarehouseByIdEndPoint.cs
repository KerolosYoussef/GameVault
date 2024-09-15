
using Catalog.API.Categories.GetCategoryById;

namespace Catalog.API.Warehouses.GetWarehouseById
{
    public record GetWarehouseByIdResponse(Warehouse Result);
    public class GetWarehouseByIdEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/warehouses/{id}", GetWarehouseById)
                .WithName("GetWarehouseById")
                .Produces<GetWarehouseByIdResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Warehouse By Id")
                .WithDescription("Get Warehouse By Id");
        }

        private static async Task<IResult> GetWarehouseById(Guid id, ISender sender)
        {
            // create query to get the warehouse
            var query = new GetWarehouseByIdQuery(id);

            // send query using mediator
            var result = await sender.Send(query);

            // map result into response
            var response = result.Adapt<GetWarehouseByIdResponse>();

            return Results.Ok(response);
        }
    }
}
