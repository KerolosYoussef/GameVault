namespace Catalog.API.Products.Dtos
{
    public record BrandDto(
        string Name,
        string? Description,
        string? ImageUrl
    );
}
