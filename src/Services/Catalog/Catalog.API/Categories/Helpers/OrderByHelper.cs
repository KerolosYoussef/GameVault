using System.Linq.Expressions;
using System.Security;

namespace Catalog.API.Categories.Helpers
{
    public static partial class OrderByHelper
    {
        public static Expression<Func<Category, object>>? GetByOrderType(string order)
        {
            order = order.ToLower();
            return order switch
            {
                "id" => (p => p.Id),
                "name" => (p => p.Name),
                "status" => (p => p.Status),
                _ => (p => p.Id),
            };
        }
    }
}
