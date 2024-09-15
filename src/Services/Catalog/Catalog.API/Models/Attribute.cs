namespace Catalog.API.Models
{
    public class Attribute : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string? CustomTypes { get; set; } = string.Empty;
        public Price Price { get; set; } = new();
        public Guid PriceId { get; set; }
        public Product Product { get; set; } = new();
        public Guid ProductId { get; set; }

    }
}
