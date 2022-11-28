using Currency.DAL.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Currency.DAL.Repositories
{
    public class CurrencyCacheRepository : ICacheRepository<Domain.Models.Currency>
    {
        private readonly IMemoryCache _cache;

        public CurrencyCacheRepository(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void AddItems(params Domain.Models.Currency[] currencies)
        {
            foreach (var currency in currencies)
            {
                _cache.Set(currency.Code + currency.Date, currency);
            }
        }

        public void DeleteItems(params Domain.Models.Currency[] currencies)
        {
            foreach (var currency in currencies)
            {
                _cache.Remove(currency.Code + currency.Date.ToString());
            }
        }

        public Domain.Models.Currency GetItem(string currencyCode, DateTime date)
        {
            var currency = new Domain.Models.Currency();

            if (_cache.TryGetValue(currencyCode + date.ToString(), out currency))
            {
                return currency;
            }

            return null;
        }
    }
}
