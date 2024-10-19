
using Catalog.API.Categories.CreateCategory;

namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductRequest(string Name,
        string Description,
        string Sku,
        bool Status,
        string ImageUrl,
        List<Guid> CategoriesId,
        List<Guid> WarehousesId,
        List<Models.Attribute> Attributes,
        Guid BrandId);

    public record CreateProductResponse(Guid Id);
    public class CreateProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", CreateProduct)
                .WithName("CreateProduct")
                .Produces<CreateProductResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Product")
                .WithDescription("Create Product");
        }

        private static async Task<IResult> CreateProduct(CreateProductRequest request, ISender sender)
        {
            // map from request to command
            CreateProductCommand createProductCommand = request.Adapt<CreateProductCommand>();

            // send this command using mediator
            var result = await sender.Send(createProductCommand);

            // map from result to response
            var response = result.Adapt<CreateProductResponse>();

            // return result
            return Results.Created($"/products/{response.Id}", response);
        }
    }
}
