using Catalog.API.Models;
using GameVault.Common.Interfaces.CQRS.Commands;
using System.Net;

namespace Catalog.API.Products.CreateProduct
{
    internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        private readonly ILogger<CreateProductCommandHandler> _logger;

        public CreateProductCommandHandler(ILogger<CreateProductCommandHandler> logger)
        {
            _logger = logger;
        }

        public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await Task.WhenAll();

                // Create product entity type
                Product product = Product.Create(request.Name, request.Description, request.Sku, request.Status, request.ImageUrl, request.Categories, request.Warehouses, request.Attribute, request.Brand);

                // Save to database

                // Return result
                return new CreateProductResult(HttpStatusCode.OK, product);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while creating the product");
                return new(HttpStatusCode.InternalServerError, null!); 
            }
        }
    }

    public record CreateProductCommand(
        string Name,
        string Description,
        string Sku,
        bool Status,
        string ImageUrl,
        List<Category> Categories,
        List<Warehouse> Warehouses,
        List<Models.Attribute> Attribute,
        Brand Brand) : ICommand<CreateProductResult>;

    public record CreateProductResult(HttpStatusCode StatusCode, Product product);
}
