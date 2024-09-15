using Catalog.API.Warehouses.Exepctions;
using GameVault.Common.Interfaces.CQRS.Queries;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Warehouses.GetWarehouseById
{
    public record GetWarehouseByIdQuery(Guid Id) : IQuery<GetWarehouseByIdResult>;
    public record GetWarehouseByIdResult(Warehouse Result);
    public class GetWarehouseByIdHandler : IQueryHandler<GetWarehouseByIdQuery, GetWarehouseByIdResult>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GetWarehouseByIdHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<GetWarehouseByIdResult> Handle(GetWarehouseByIdQuery request, CancellationToken cancellationToken)
        {
            // get warehouse data from database
            var warehouse = await _applicationDbContext.Set<Warehouse>().AsNoTracking().SingleOrDefaultAsync(w => w.Id == request.Id);

            // check if this warehouse not exists
            if (warehouse is null)
                throw new WarehouseExceptions.WarehouseNotFoundException();

            // return warehouse data
            return new GetWarehouseByIdResult(warehouse);
        }
    }
}
