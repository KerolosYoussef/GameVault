
namespace Catalog.API.Warehouses.DeleteWarehouse
{
    public class DeleteWarehouseEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/warehouses/{id}", DeleteWarehouse)
                .WithName("DeleteWarehouse")
                .WithSummary("Delete Warehouse")
                .WithDescription("Delete Warehouse");
        }

        private static async Task DeleteWarehouse(Guid id, ISender sender)
        {
            // create command to delete warehouse
            var command = new DeleteWarehouseCommand(id);

            // send command to mediator
            await sender.Send(command);
        }
    }
}
