using Microsoft.OpenApi.Models;
using Currency.BLL;
using Currency.DAL;
using Currency.BLL.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Description = "Currency Web Api v1",
        Title = "Currency Web Api",
        Version = "1.0.0",
    });
});

builder.Services.AddCurrencyBLL();
builder.Services.AddCurrencyDAL();

var app = builder.Build();

app.Lifetime.ApplicationStarted.Register(OnStarted);

void OnStarted()
{
    using (var scope = app.Services.CreateScope())
    {
        var currencyService = scope.ServiceProvider.GetService<ICurrencyService>();
        currencyService?.LoadCurrenciesFromFileToCache();
    }
}

app.UseSwagger();

app.UseSwaggerUI();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
