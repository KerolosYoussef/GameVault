namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);
    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart cannot be null.");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("Cart username cannot be empty");
        }
    }
    public class StoreBasketHandler : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        private readonly IBasketRepository _repository;
        private readonly Discount.Grpc.DiscountService.DiscountServiceClient _discountServiceClient;

        public StoreBasketHandler(IBasketRepository repository, Discount.Grpc.DiscountService.DiscountServiceClient discountServiceClient)
        {
            _repository = repository;
            _discountServiceClient = discountServiceClient;
        }

        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            // Communicate with the discount service to apply discounts if needed
            await DeductDiscount(command.Cart, cancellationToken);
            var cart = command.Cart;
            var result = await _repository.SetBasket(cart);
            return new StoreBasketResult(result.UserName);
        }

        private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
        {
            foreach (var item in cart.Items)
            {
                var discountRequest = new Discount.Grpc.GetDiscountRequest { ProductName = item.ProductName };
                var discountResponse = await _discountServiceClient.GetDiscountAsync(discountRequest, cancellationToken: cancellationToken);
                if (discountResponse != null && discountResponse.Amount > 0)
                {
                    item.Price -= discountResponse.Amount; // Apply the discount to the item price
                }
            }
        }
    }
}
