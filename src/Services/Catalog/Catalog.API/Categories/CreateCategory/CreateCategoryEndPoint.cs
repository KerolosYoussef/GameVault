namespace Catalog.API.Categories.CreateCategory
{
    public record CreateCategoryRequest(string Name, string? Description, string? ImageUrl, bool Status, Guid? ParentCategoryId);
    public record CreateCategoryResponse(Guid Id);
    public class CreateCategoryEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/categories", CreateCategory)
                .WithName("CreateCategory")
                .Produces<CreateCategoryResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Category")
                .WithDescription("Create Category");
        }

        private static async Task<IResult> CreateCategory(CreateCategoryRequest request, ISender sender)
        {
            // Map request to command
            CreateCategoryCommand createCategoryCommand = request.Adapt<CreateCategoryCommand>();

            // send command using mediator
            var result = await sender.Send(createCategoryCommand);

            // map result to response
            var response = result.Adapt<CreateCategoryResponse?>();

            // return result
            return Results.Created($"/categories/{response?.Id}",response);
        }
    }
}
