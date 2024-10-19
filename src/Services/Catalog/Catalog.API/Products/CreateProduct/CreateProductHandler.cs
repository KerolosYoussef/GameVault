using Catalog.API.Models;
using GameVault.Common.Interfaces.CQRS.Commands;
using GameVault.Common.Interfaces.Helpers;

namespace Catalog.API.Products.CreateProduct
{
    internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        private readonly ILogger<CreateProductCommandHandler> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IImageHandler _imageHandler;

        public CreateProductCommandHandler(ILogger<CreateProductCommandHandler> logger, ApplicationDbContext context, IImageHandler imageHandler)
        {
            _logger = logger;
            _context = context;
            _imageHandler = imageHandler;
        }

        public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            using var transaction = _context.Database.BeginTransaction();
            string imageUrl = null!;

            if (!string.IsNullOrEmpty(request.ImageUrl))
            {
                imageUrl = _imageHandler.SaveImageFromBase64(request.ImageUrl, $"Products/Images");
            }

            // create product entity type
            Product product = Product.Create(request.Name, request.Description, request.Sku, request.Status, imageUrl, null!, null!, request.Attributes, null!);

            // add brand to product
            product.BrandId = request.BrandId;

            // save product to database
            await _context.Set<Product>().AddAsync(product);
            await _context.SaveChangesAsync();

            // add category relation
            await _context.Set<CategoryProduct>().AddRangeAsync(request.CategoriesId.Select(id => new CategoryProduct()
            {
                CategoryId = id,
                ProductId = product.Id
            }));

            // add category relation
            await _context.Set<ProductWarehouse>().AddRangeAsync(request.WarehousesId.Select(id => new ProductWarehouse()
            {
                WarehouseId = id,
                ProductId = product.Id
            }));

            // presist database
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // Return result
            return new CreateProductResult(product.Id);
        }
    }

    public record CreateProductCommand(
        string Name,
        string Description,
        string Sku,
        bool Status,
        string ImageUrl,
        List<Guid> CategoriesId,
        List<Guid> WarehousesId,
        List<Models.Attribute> Attributes,
        Guid BrandId) : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);
}
