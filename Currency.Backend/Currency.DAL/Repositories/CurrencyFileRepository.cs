using Currency.DAL.Interfaces;
using System.Text.Json;

namespace Currency.DAL.Repositories
{
    public class CurrencyFileRepository : IFileRepository<Domain.Models.Currency>
    { 
        public static string DirPath { get; set; }

        public async void AddItems(params Domain.Models.Currency[] items)
        {
            foreach (var item in items)
            {
                string filePath = DirPath + item.Code + item.Date.Year.ToString() + item.Date.Month.ToString() + item.Date.Day.ToString() + ".json";

                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    await JsonSerializer.SerializeAsync(fs, item);
                }
            }
        }

        public void DeleteItems(params Domain.Models.Currency[] items)
        {
            foreach (var item in items)
            {
                string filePath = DirPath + item.Code + item.Date.Year.ToString() + item.Date.Month.ToString() + item.Date.Day.ToString() + ".json";

                if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath);   
                }
            }
        }

        public async Task<IEnumerable<Domain.Models.Currency>> GetAllItemsAsync()
        {
            var files = Directory.GetFiles(DirPath);
            List<Domain.Models.Currency> currencies = new List<Domain.Models.Currency>();

            foreach (var file in files)
            {
                using (FileStream fs = new FileStream(file, FileMode.Open))
                {
                    var currency = await JsonSerializer.DeserializeAsync<Domain.Models.Currency>(fs);
                    currencies.Add(currency);
                }
            }

            return currencies;
        }
    }
}
