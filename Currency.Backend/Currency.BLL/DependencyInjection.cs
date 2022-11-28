using Currency.BLL.Interfaces;
using Currency.BLL.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Currency.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCurrencyBLL(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddHttpClient();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
