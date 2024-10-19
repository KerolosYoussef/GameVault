namespace Catalog.API.Models
{
    public class ProductWarehouse
    {
        public Guid WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public decimal Quantity { get; set; }
    }
}
