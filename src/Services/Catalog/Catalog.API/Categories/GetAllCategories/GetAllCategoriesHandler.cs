using Catalog.API.Categories.Helpers;
using Catalog.API.Categories.Params;
using GameVault.Common.Interfaces.CQRS.Queries;
using GameVault.Common.Results;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Categories.GetAllCategories
{
    public record GetAllCategoriesQuery(CategoryParams Params) : IQuery<GetAllCategoriesResult>;
    public record GetAllCategoriesResult(Pagination<Category> Result);
    internal class GetAllCategoriesHandler : IQueryHandler<GetAllCategoriesQuery, GetAllCategoriesResult>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GetAllCategoriesHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<GetAllCategoriesResult> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            // Create query to get data
            var query = _applicationDbContext.Set<Category>().AsNoTracking();

            // Apply filter
            query = ApplyFilter(request, query);

            // Get Total Count
            var totalCounts = await query.CountAsync();

            // Apply paging & sorting
            query = ApplyPagingAndSorting(request, query, totalCounts);

            // Get categories from database
            var categories = await query.ToListAsync();

            // Create paginated categories
            var pagination = new Pagination<Category>(request.Params.PageIndex, request.Params.PageSize, totalCounts, categories);

            // Return response
            return new GetAllCategoriesResult(pagination);
        }
        private static IQueryable<Category> ApplyFilter(GetAllCategoriesQuery request, IQueryable<Category> query)
        {
            query = query.Where(c
                            => (string.IsNullOrEmpty(request.Params.CategoryName) || c.Name == request.Params.CategoryName)
                            && (request.Params.ParentCategoryId == null || request.Params.ParentCategoryId == c.ParentCategoryId)
                            && (request.Params.Status == null || request.Params.Status == c.Status));
            return query;
        }

        private static IQueryable<Category> ApplyPagingAndSorting(GetAllCategoriesQuery request, IQueryable<Category> query, int totalCounts)
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
