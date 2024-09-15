using Catalog.API.Models;
using Catalog.API.Warehouses.Exepctions;
using GameVault.Common.Interfaces.CQRS.Commands;

namespace Catalog.API.Warehouses.CreateWarehouse
{
    public record CreateWarehouseCommand(string Name, string Description, string Address, decimal Capacity) : ICommand<CreateWarehouseResult>;
    public record CreateWarehouseResult(Guid Id);
    internal class CreateWarehouseHandler : ICommandHandler<CreateWarehouseCommand, CreateWarehouseResult>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CreateWarehouseHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<CreateWarehouseResult> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
        {
            // Create warehouse entity
            Warehouse warehouse = new Warehouse(request.Name, request.Description, request.Address, request.Capacity);

            // Check if the warehouse name already exists
            if (_applicationDbContext.Set<Warehouse>().Any(c => c.Name == warehouse.Name))
            {
                throw new WarehouseExceptions.WarehouseAlreadyExistsException();
            }

            // Add warehouse to database and presist
            await _applicationDbContext.AddAsync(warehouse);
            await _applicationDbContext.SaveChangesAsync();

            // Return result
            return new CreateWarehouseResult(warehouse.Id);
        }
    }
}
