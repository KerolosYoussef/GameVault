namespace Catalog.API.Products.Dtos
{
    public record WarehouseDto(string Name, string? Description, string Address, decimal Capacity);
}
