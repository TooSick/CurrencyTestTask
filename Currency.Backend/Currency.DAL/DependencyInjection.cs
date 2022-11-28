using Currency.DAL.Interfaces;
using Currency.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Currency.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCurrencyDAL(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<ICacheRepository<Domain.Models.Currency>, CurrencyCacheRepository>();
            services.AddScoped<IFileRepository<Domain.Models.Currency>, CurrencyFileRepository>();

            return services;
        }
    }
}
