namespace Catalog.API.Models
{
    public class Brand : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<Product> Products { get; set; } = new();
    }
}
