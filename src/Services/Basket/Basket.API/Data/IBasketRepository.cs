namespace Basket.API.Data
{
    public interface IBasketRepository
    {
        public Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default);
        public Task<ShoppingCart> SetBasket(ShoppingCart cart, CancellationToken cancellationToken = default);
        public Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default);
    }
}
