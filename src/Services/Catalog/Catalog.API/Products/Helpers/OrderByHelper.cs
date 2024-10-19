using System.Linq.Expressions;

namespace Catalog.API.Products.Helpers
{
    public static partial class OrderByHelper
    {
        public static Expression<Func<Product, object>>? GetByOrderType(string order)
        {
            order = order.ToLower();
            return order switch
            {
                "id" => (p => p.Id),
                "sku" => (p => p.Sku),
                "name" => (p => p.Name),
                "status" => (p => p.Status),
                _ => (p => p.Id),
            };
        }
    }
}
