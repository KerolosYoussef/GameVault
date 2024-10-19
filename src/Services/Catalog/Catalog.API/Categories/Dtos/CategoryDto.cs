namespace Catalog.API.Categories.Dtos
{
    public class CategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool Status { get; set; }
        public Category? ParentCategory { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
