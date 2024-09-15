using Catalog.API.Categories.GetAllCategories;
using Catalog.API.Warehouses.Helpers;
using Catalog.API.Warehouses.Pramas;
using GameVault.Common.Interfaces.CQRS.Queries;
using GameVault.Common.Results;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace Catalog.API.Warehouses.GetAllWarehouse
{
    public record GetAllWarehousesQuery(WarehouseParams Params) : IQuery<GetAllWarehousesResult>;
    public record GetAllWarehousesResult(Pagination<Warehouse> Result);
    public class GetAllWarehousesHandler(ApplicationDbContext applicationDbContext) : IQueryHandler<GetAllWarehousesQuery, GetAllWarehousesResult>
    {
        public async Task<GetAllWarehousesResult> Handle(GetAllWarehousesQuery request, CancellationToken cancellationToken)
        {
            var query = applicationDbContext.Set<Warehouse>().AsNoTracking();

            // Apply filter
            query = ApplyFilter(request, query);

            // Get Total Count
            var totalCounts = await query.CountAsync();

            // Apply paging & sorting
            query = ApplyPagingAndSorting(request, query, totalCounts);

            // Get warehouses from database
            var warehouses = await query.ToListAsync();

            // Create paginated warehouses
            var pagination = new Pagination<Warehouse>(request.Params.PageIndex, request.Params.PageSize, totalCounts, warehouses);

            // Return response
            return new GetAllWarehousesResult(pagination);
        }

        private static IQueryable<Warehouse> ApplyFilter(GetAllWarehousesQuery request, IQueryable<Warehouse> query)
        {
            query = query.Where(
                c => string.IsNullOrEmpty(request.Params.Name) || c.Name == request.Params.Name
            );
            return query;
        }

        private static IQueryable<Warehouse> ApplyPagingAndSorting(GetAllWarehousesQuery request, IQueryable<Warehouse> query, int totalCounts)
        {
            // Apply paging
            if (request.Params.PageIndex != -1)
            {
                var skip = request.Params.PageSize * (request.Params.PageIndex);
                var take = request.Params.PageSize;
                query = query.Skip(skip).Take(take);
            }
            else
            {
                query = query.Skip(0).Take(totalCounts);
            }

            // Apply sorting
            if (request.Params.OrderBy is not null && request.Params.OrderByType == "ASC")
            {
                query = query.OrderBy(OrderByHelper.GetByOrderType(request.Params.OrderBy)!);
            }
            else if (request.Params.OrderBy is not null && request.Params.OrderByType == "DSC")
            {
                query = query.OrderByDescending(OrderByHelper.GetByOrderType(request.Params.OrderBy)!);
            }

            return query;
        }
    }
}
