using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace Currency.Client.Requesters
{
    public static class RequesterToWebApi
    {
        public static async Task<string> MakeRequestAsync(string currencyCode, DateTime date)
        {
            string formatedDate = date.Year.ToString() + "-" + date.Month.ToString() + "-" + date.Day.ToString();

            if (date.Day < 10)
            {
                formatedDate = date.Year.ToString() + "-" + date.Month.ToString() + "-" + "0" + date.Day.ToString();
            }

            Uri uri = new Uri($"https://localhost:7131/api/Currency/Get?currencyCode={currencyCode}&date={formatedDate}");

            using (var client = new HttpClient())
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
