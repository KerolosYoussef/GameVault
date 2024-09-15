namespace Catalog.API.Categories.Params;
using Params = GameVault.Common.Parameters.Params;

public class CategoryParams : Params
{
    public string? CategoryName { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public bool? Status { get; set; }
}
