namespace Catalog.API.Models
{
    public class Product : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Sku { get; set; } = string.Empty;
        public bool Status { get; set; }
        public string? ImageUrl { get; set; }
        public List<Category> Categories { get; set; } 
        public List<Warehouse> Warehouses { get; set; }
        public List<Attribute> Attributes { get; set; }
        public Brand Brand { get; set; }
        public Guid BrandId { get; set; }

        public Product() { }

        private Product(
            string name,
            string description,
            string sku,
            bool status,
            string imageUrl,
            List<Category> categories,
            List<Warehouse> warehouses,
            List<Attribute> attributes,
            Brand brand) 
        {
            Name = name;
            Description = description;
            Sku = sku;
            Status = status;
            ImageUrl = imageUrl;
            Categories = categories;
            Warehouses = warehouses;
            Attributes = attributes;
            Brand = brand;
        }

        public static Product Create(
            string name,
            string description,
            string sku,
            bool status,
            string imageUrl,
            List<Category> categories,
            List<Warehouse> warehouses,
            List<Attribute> attributes,
            Brand brand)
        {
            return new(name, description, sku, status, imageUrl, categories, warehouses, attributes, brand);
        }
    }
}
