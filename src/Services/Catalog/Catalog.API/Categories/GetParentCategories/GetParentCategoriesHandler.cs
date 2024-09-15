using GameVault.Common.Interfaces.CQRS.Queries;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Categories.GetParentCategories
{
    public record GetParentCategoriesQuery() : IQuery<GetParentCategoriesResult>;
    public record GetParentCategoriesResult(List<Category> Categories);
    public class GetParentCategoriesHandler : IQueryHandler<GetParentCategoriesQuery, GetParentCategoriesResult>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public GetParentCategoriesHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<GetParentCategoriesResult> Handle(GetParentCategoriesQuery request, CancellationToken cancellationToken)
        {
            // Get Parent categories from database
            var parentCategories = await _applicationDbContext.Set<Category>().AsNoTracking().Where(x => x.ParentCategoryId == null).ToListAsync();

            // return parent catogries
            return new GetParentCategoriesResult(parentCategories);
        }
    }
}
