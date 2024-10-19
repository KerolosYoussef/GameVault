namespace Catalog.API.Products.Dtos
{
    public record AttributeDto(
        string Name,
        string Color,
        string Size,
        string? CustomTypes,
        Price Price
    );
}
