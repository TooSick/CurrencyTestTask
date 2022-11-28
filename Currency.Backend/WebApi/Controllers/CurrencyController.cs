using Microsoft.AspNetCore.Mvc;
using Currency.BLL.Models;
using Currency.BLL.Interfaces;

namespace Currency.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrencyViewModel>>> Get([FromQuery]string currencyCode, [FromQuery]string date)
        {
            var currency = await _currencyService.GetCurrencyAsync(currencyCode, date);

            return Ok(currency);
        }
    }
}
