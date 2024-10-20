﻿using Catalog.API.Categories.Dtos;
using GameVault.Common.Interfaces.CQRS.Queries;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Categories.GetCategoryById
{
    public record GetCategoryByIdQuery(Guid Id): IQuery<GetCategoryByIdResult>;
    public record GetCategoryByIdResult(CategoryDto? Category);
    public class GetCategoryByIdHandler : IQueryHandler<GetCategoryByIdQuery, GetCategoryByIdResult>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GetCategoryByIdHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<GetCategoryByIdResult> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            // Get data from database
            var category = await _applicationDbContext.Set<Category>().Include(c => c.Products).SingleOrDefaultAsync(c => c.Id == request.Id);

            // return result
            return new GetCategoryByIdResult(category.Adapt<CategoryDto>());
        }
    }
}
