using GameVault.Common.Interfaces.CQRS.Commands;
using Microsoft.EntityFrameworkCore;
using Catalog.API.Warehouses.Exepctions;

namespace Catalog.API.Warehouses.DeleteWarehouse
{
    public record DeleteWarehouseCommand(Guid Id) : ICommand;
    public class DeleteWarehouseHandler : ICommandHandler<DeleteWarehouseCommand>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DeleteWarehouseHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Unit> Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
        {
            // create query
            var query = _applicationDbContext.Set<Warehouse>();

            // get warehouse from database
            var warehouse = await query.SingleOrDefaultAsync(w => w.Id == request.Id);

            // check if the warehouse is not exists
            if (warehouse is null)
                throw new WarehouseExceptions.WarehouseNotFoundException();

            // delete warehouse
            warehouse.DeletedAt = DateTime.UtcNow;

            // presist data
            query.Update(warehouse);
            await _applicationDbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
