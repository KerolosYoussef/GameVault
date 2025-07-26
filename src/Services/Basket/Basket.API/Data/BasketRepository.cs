namespace Basket.API.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDocumentSession _documentSession;

        public BasketRepository(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var basket = await _documentSession.LoadAsync<ShoppingCart>(userName, cancellationToken);
            return basket is null ? throw new BasketNotFoundException(userName) : basket;
        }
        public async Task<ShoppingCart> SetBasket(ShoppingCart cart, CancellationToken cancellationToken = default)
        {
            _documentSession.Store(cart);
            await _documentSession.SaveChangesAsync(cancellationToken);
            return cart;
        }
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            _documentSession.Delete<ShoppingCart>(userName);
            await _documentSession.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
