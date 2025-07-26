namespace Basket.API.Data
{
    public class CachedBasketRepository : IBasketRepository
    {
        private readonly IBasketRepository _repository;
        private readonly IDistributedCache _cache;

        public CachedBasketRepository(IBasketRepository repository, IDistributedCache cache)
        {
            _repository = repository;
            _cache = cache;
        }
        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            // get the basket from cache first
            var cachedBasket = await _cache.GetStringAsync(userName, cancellationToken);

            // if it exists in cache, return it
            if (!string.IsNullOrEmpty(cachedBasket))
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

            // otherwise, get it from the repository and cache it and return it
            var basket = await _repository.GetBasket(userName, cancellationToken);
            await _cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);
            return basket;
        }

        public async Task<ShoppingCart> SetBasket(ShoppingCart cart, CancellationToken cancellationToken = default)
        {
            // get the basket from the repository
            var basket = await _repository.SetBasket(cart, cancellationToken);

            // cache the basket after setting it or updating it
            await _cache.SetStringAsync(cart.UserName, JsonSerializer.Serialize(basket), cancellationToken);

            // return the basket
            return basket;
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            // delete the basket from cache first
            await _cache.RemoveAsync(userName, cancellationToken);

            // delete the basket from the repository
            await _repository.DeleteBasket(userName, cancellationToken);

            return true;
        }

    }
}
