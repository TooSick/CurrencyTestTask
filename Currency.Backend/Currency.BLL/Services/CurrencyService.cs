using AutoMapper;
using Currency.BLL.Common.Exceptions;
using Currency.BLL.Deserializers;
using Currency.BLL.Interfaces;
using Currency.BLL.Models;
using Currency.BLL.Requesters;
using Currency.DAL.Interfaces;
using Currency.DAL.Repositories;
using System.Globalization;
using System.Reflection;

namespace Currency.BLL.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICacheRepository<Domain.Models.Currency> _cache;
        private readonly IFileRepository<Domain.Models.Currency> _fileRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;

        public CurrencyService(ICacheRepository<Domain.Models.Currency> cache, IFileRepository<Domain.Models.Currency> fileRepository, 
            IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _cache = cache;
            _fileRepository = fileRepository;
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }

        public async Task<CurrencyViewModel> GetCurrencyAsync(string currencyCode, string date)
        {
            CurrencyViewModel currencyViewModel;
            DateTime formatedDate = GetDate(date);

            var currency = _cache.GetItem(currencyCode, formatedDate);

            if (currency == null)
            {
                string jsonCurrency = await RequesterToNBRBApi.MakeRequestAsync(currencyCode, formatedDate, _httpClientFactory);
                currency = DeserializerNBRB.Deserialize(jsonCurrency);

                if (currency is null)
                {
                    throw new DeserializeException("Deserialize exception in DeserializerNBRB");
                }

                _cache.AddItems(currency);
                _fileRepository.AddItems(currency);
                    
                currencyViewModel = _mapper.Map<CurrencyViewModel>(currency);
            }
            else
            {
                currencyViewModel = _mapper.Map<CurrencyViewModel>(currency);
            }

            return currencyViewModel;
        }

        public async void LoadCurrenciesFromFileToCache()
        {
            CurrencyFileRepository.DirPath = GetPathToDir();
            var currencies = await _fileRepository.GetAllItemsAsync();

            if (currencies.Any())
            {
                _cache.AddItems(currencies.ToArray());
            }
        }

        private DateTime GetDate(string date)
        {
            List<DateTime> dates = new List<DateTime>();
            string targetDateFormat = "yyyy-MM-dd";
            DateTime formatedDate = DateTime.ParseExact(date, targetDateFormat, CultureInfo.InvariantCulture);

            return formatedDate;
        }

        private string GetPathToDir()
        {
            var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            int indexOfChar = appDir.IndexOf("\\WebApi");
            appDir = appDir.Remove(indexOfChar, appDir.Length - indexOfChar);

            var relativePath = appDir + "\\JsonCurrencies\\";

            if (!Directory.Exists(relativePath))
            {
                Directory.CreateDirectory(relativePath);
            }

            return relativePath;
        }
    }
}
