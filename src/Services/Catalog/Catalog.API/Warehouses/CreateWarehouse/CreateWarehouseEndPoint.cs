
using Catalog.API.Categories.CreateCategory;

namespace Catalog.API.Warehouses.CreateWarehouse
{
    public record CreateWarehouseRequest(string Name, string Description, string Address, decimal Capacity);
    public record CreateWarehouseResponse(Guid Id);
    public class CreateWarehouseEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/warehouses", CreateWarehouse)
                .WithName("CreateWarehouse")
                .Produces<CreateWarehouseResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Warehouse")
                .WithDescription("Create Warehouse");
        }

        private static async Task<IResult> CreateWarehouse(CreateWarehouseRequest request, ISender sender)
        {
            // map request to command
            var command = request.Adapt<CreateWarehouseCommand>();

            // send command using mediator
            var result = await sender.Send(command);

            // map result to response
            var response = result.Adapt<CreateWarehouseResponse>();

            return Results.Created($"/warehouses/{response.Id}", response);
        }
    }
}
