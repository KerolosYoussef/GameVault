
using Catalog.API.Categories.UpdateCategory;

namespace Catalog.API.Warehouses.UpdateWarehouse
{
    public record UpdateWarehouseRequest(Guid Id, string Name, string? Description, string Address, decimal Capacity);
    public record UpdateWarehouseResponse(Guid Id);
    public class UpdateWarehouseEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/warehouses", UpdateWarehouse)
                .WithName("UpdateWarehouse")
                .Produces<UpdateWarehouseResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Update Warehouse")
                .WithDescription("Update Warehouse");
        }
        private static async Task<IResult> UpdateWarehouse(UpdateWarehouseRequest request, ISender sender)
        {
            // map request to command
            var command = request.Adapt<UpdateWarehouseCommand>();

            // send command using mediator
            var result = await sender.Send(command);

            // map result to response
            var response = result.Adapt<UpdateWarehouseResponse>();

            return Results.Ok(response);
        }
    }
}
