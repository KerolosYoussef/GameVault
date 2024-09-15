
using GameVault.Common.Interfaces.CQRS.Commands;
using Microsoft.EntityFrameworkCore;
using Catalog.API.Warehouses.Exepctions;

namespace Catalog.API.Warehouses.UpdateWarehouse
{
    public record UpdateWarehouseCommand(Guid Id, string Name, string? Description, string Address, decimal Capacity) : ICommand<UpdateWarehouseResult>;
    public record UpdateWarehouseResult(Guid Id);
    public class UpdateWarehouseHandler : ICommandHandler<UpdateWarehouseCommand, UpdateWarehouseResult>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UpdateWarehouseHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<UpdateWarehouseResult> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
        {
            // set warehouse query
            var query = _applicationDbContext.Set<Warehouse>();

            // get warehouse from database
            var warehouse = await query.SingleOrDefaultAsync(x => x.Id == request.Id);

            // check if warehouse is not exists
            if (warehouse is null)
                throw new WarehouseExceptions.WarehouseNotFoundException();

            // update warehouse
            warehouse.Name = request.Name;
            warehouse.Description = request.Description;
            warehouse.Address = request.Address;
            warehouse.Capacity = request.Capacity;

            // check if this warehouse name already exists
            if(await query.AnyAsync(x => x.Name == request.Name))
                throw new WarehouseExceptions.WarehouseAlreadyExistsException();

            // presist data in database
            query.Update(warehouse);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new UpdateWarehouseResult(warehouse.Id);
        }
    }
}
