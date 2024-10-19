namespace Catalog.API.Products.Params;
using Params = GameVault.Common.Parameters.Params;

public class ProductParams : Params
{
    public string? Name { get; set; }
    public string? Sku { get; set; }
    public bool? Status { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? WarehouseId { get; set; }
    public List<Guid>? AttributesId { get; set; }
    public Guid? BrandId { get; set; }
}
