namespace Catalog.API.Products.Dtos
{
    public record ProductDto(
        string Name,
        string? Description,
        string Sku,
        bool Status,
        string? ImageUrl,
        List<CategoryDto> Categories,
        List<WarehouseDto> Warehouses,
        List<AttributeDto> Attributes,
        BrandDto Brand
    );
}
