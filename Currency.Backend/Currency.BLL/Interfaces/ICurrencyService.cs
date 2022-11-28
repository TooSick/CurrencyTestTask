using Currency.BLL.Models;

namespace Currency.BLL.Interfaces
{
    public interface ICurrencyService
    {
        void LoadCurrenciesFromFileToCache();
        Task<CurrencyViewModel> GetCurrencyAsync(string currencyCode, string date);
    }
}
