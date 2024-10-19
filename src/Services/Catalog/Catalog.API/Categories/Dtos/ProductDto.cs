namespace Catalog.API.Categories.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Sku { get; set; } = string.Empty;
        public bool Status { get; set; }
        public string? ImageUrl { get; set; }
        public List<Models.Attribute> Attributes { get; set; }
    }
}
