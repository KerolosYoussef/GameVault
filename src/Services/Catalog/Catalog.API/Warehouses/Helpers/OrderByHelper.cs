using System.Linq.Expressions;

namespace Catalog.API.Warehouses.Helpers
{
    public static partial class OrderByHelper
    {
        public static Expression<Func<Warehouse, object>>? GetByOrderType(string order)
        {
            order = order.ToLower();
            return order switch
            {
                "id" => (p => p.Id),
                "name" => (p => p.Name),
                _ => (p => p.Id),
            };
        }
    }
}
