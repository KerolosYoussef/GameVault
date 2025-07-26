using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace Catalog.API
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
            
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            ApplyDeletedAtGlobalFilter(modelBuilder);
        }

        private static void ApplyDeletedAtGlobalFilter(ModelBuilder modelBuilder)
        {
            Expression<Func<BaseModel, bool>> filterExpr = bm => bm.DeletedAt == null;
            var entityTypes = modelBuilder.Model.GetEntityTypes().Where(x => x.ClrType.IsAssignableTo(typeof(BaseModel)));
            foreach (var mutableEntityType in entityTypes)
            {
                // modify expression to handle correct child type
                var parameter = Expression.Parameter(mutableEntityType.ClrType);
                var body = ReplacingExpressionVisitor.Replace(filterExpr.Parameters[0], parameter, filterExpr.Body);
                var lambdaExpression = Expression.Lambda(body, parameter);

                // set filter
                mutableEntityType.SetQueryFilter(lambdaExpression);
            }
        }
    }
}
