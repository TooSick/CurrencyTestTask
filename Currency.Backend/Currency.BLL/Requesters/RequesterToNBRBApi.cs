
namespace Currency.BLL.Requesters
{
    public static class RequesterToNBRBApi
    {
        public static async Task<string> MakeRequestAsync(string currencyCode, DateTime date, IHttpClientFactory clientFactory)
        {
            string formatedDate = date.Year.ToString() + "-" + date.Month.ToString() + "-" + date.Day.ToString();

            Uri uri = new Uri($"https://www.nbrb.by/api/exrates/rates/{currencyCode}?parammode=2&ondate=" +
                    $"{formatedDate}&periodicity=0");

            using (var client = clientFactory.CreateClient())
            {
                var response = await client.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
