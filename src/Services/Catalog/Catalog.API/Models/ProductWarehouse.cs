namespace Catalog.API.Models
{
    public class ProductWarehouse
    {
        public Guid WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; } = default!;

        public Guid ProductId { get; set; }
        public Product Product { get; set; } = new();
        public decimal Quantity { get; set; }
    }
}
