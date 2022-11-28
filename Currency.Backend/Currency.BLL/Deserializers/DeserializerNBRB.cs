using System.Text.Json;

namespace Currency.BLL.Deserializers
{
    public static class DeserializerNBRB
    {
        public static Domain.Models.Currency Deserialize(string jsonCurrency)
        {
            var obj = JsonSerializer.Deserialize(jsonCurrency, typeof(CurrencyFromNBRB));
            if (obj is CurrencyFromNBRB currencyFromNBRB)
            {
                return new Domain.Models.Currency
                {
                    Code = currencyFromNBRB.Cur_Abbreviation,
                    Date = currencyFromNBRB.Date,
                    Value = currencyFromNBRB.Cur_OfficialRate,
                    Amount = currencyFromNBRB.Cur_Scale
                };
            }

            return null;
        }
    }
}
