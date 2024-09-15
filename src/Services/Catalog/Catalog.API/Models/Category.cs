namespace Catalog.API.Models
{
    public class Category : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool Status { get; set; }
        public Category? ParentCategory { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public List<Product> Products { get; set; } = new();

        public Category() { }

        private Category(
            string name,
            string? description,
            string? imageUrl,
            bool status,
            Guid? parentCategoryId)
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Status = status;
            ParentCategoryId = parentCategoryId;
        }

        public static Category Create(string name, string? description, string? imageUrl, bool status, Guid? parentCategoryId) 
            => new(name, description,imageUrl, status, parentCategoryId);
    }
}
