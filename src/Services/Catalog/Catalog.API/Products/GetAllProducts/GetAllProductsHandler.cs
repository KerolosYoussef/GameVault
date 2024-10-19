using Catalog.API.Products.Dtos;
using Catalog.API.Products.Helpers;
using Catalog.API.Products.Params;
using GameVault.Common.Interfaces.CQRS.Queries;
using GameVault.Common.Results;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Catalog.API.Products.GetAllProducts
{
    public record GetAllProductsQuery(ProductParams Params) : IQuery<GetAllProductsResult>;
    public record GetAllProductsResult(Pagination<ProductDto> Result);
    public class GetAllProductsHandler : IQueryHandler<GetAllProductsQuery, GetAllProductsResult>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GetAllProductsHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<GetAllProductsResult> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            // Create query to get data
            var query = _applicationDbContext.Set<Product>().AsNoTracking();

            // Apply includes
            query = ApplyIncludes(query);

            // Apply filter
            query = ApplyFilter(request, query);

            // Get Total Count
            var totalCounts = await query.CountAsync();

            // Apply paging & sorting
            query = ApplyPagingAndSorting(request, query, totalCounts);

            // Get products from database
            var products = await query.ToListAsync();

            // Map products to its dto
            var productsDto = products.Adapt<List<ProductDto>>();

            // Create paginated categories
            var pagination = new Pagination<ProductDto>(request.Params.PageIndex, request.Params.PageSize, totalCounts, productsDto);

            // Return response
            return new GetAllProductsResult(pagination);
        }

        private IQueryable<Product> ApplyIncludes(IQueryable<Product> query)
        {
            // Add attributes with price
            query = query.Include(p => p.Attributes).ThenInclude(a => a.Price);

            return query;
        }

        private IQueryable<Product> ApplyFilter(GetAllProductsQuery request, IQueryable<Product> query)
        {
            var parameters = request.Params;
            return query
                .Where(p
                    => (string.IsNullOrEmpty(parameters.Name) || p.Name == parameters.Name)
                    && ((parameters.Status == null) || p.Status == parameters.Status)
                    && (string.IsNullOrEmpty(parameters.Sku) || p.Sku == parameters.Sku)
                    && (parameters.CategoryId == null || p.Categories.Any(c => c.Id == parameters.CategoryId))
                    && (parameters.WarehouseId == null || p.Warehouses.Any(c => c.Id == parameters.WarehouseId))
                    && (parameters.BrandId == null || p.BrandId == parameters.BrandId));
        }


        private IQueryable<Product> ApplyPagingAndSorting(GetAllProductsQuery request, IQueryable<Product> query, int totalCounts)
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
