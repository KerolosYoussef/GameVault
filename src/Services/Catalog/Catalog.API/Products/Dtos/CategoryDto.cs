namespace Catalog.API.Products.Dtos
{
    public record CategoryDto(string Name, string? Description, string? ImageUrl, bool Status);
}
