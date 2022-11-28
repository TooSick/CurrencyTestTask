using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using Currency.Client.Commands;
using System.Security.Policy;
using System.Windows;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Currency.Client.Models;
using System.Globalization;
using Currency.Client.Requesters;
using System.Security.Cryptography;
using System.Net.Http;

namespace Currency.Client.ViewModels
{
    public enum CurrencyCode
    {
        USD,
        EUR,
        RUB
    }

    public class CurrencyViewModel : INotifyPropertyChanged
    {
        public DateTime TodayDate 
        {
            get
            {
                return DateTime.Now;
            } 
        }

        public ISeries[] SeriesCollection { get; set; }

        private DateTime _dateFrom;
        public DateTime DateFrom 
        {
            set
            {
                _dateFrom = value;
                OnPropertyChanged("DateFrom");
            } 
        }

        private DateTime _dateTo;
        public DateTime DateTo 
        {
            set
            {
                _dateTo = value;
                OnPropertyChanged("DateTo");
            } 
        }
        
        private CurrencyCode _selectedCurrencyCode;
        public CurrencyCode SelectedCurrencyCode 
        { 
            get { return _selectedCurrencyCode; } 
            set
            {
                _selectedCurrencyCode = value;
                OnPropertyChanged("SelectedCurrencyCode");
            } 
        }

        public IEnumerable<CurrencyCode> CurrencyCodeValues 
        {
            get
            {
                return Enum.GetValues(typeof(CurrencyCode)).Cast<CurrencyCode>();
            }  
        }

        private RelayCommand _drawChartCommand;
        public RelayCommand DrawChartCommand
        {
            get
            {
                return _drawChartCommand ??
                  (_drawChartCommand = new RelayCommand(async obj =>
                  {

                      bool isValid = ValidateData();
                      if (isValid)
                      {
                          try
                          {
                              var currencies = await MakeRequestForCurrenciesAsync();
                              DrawChart(currencies);
                              OnPropertyChanged("SeriesCollection");
                          }
                          catch (HttpRequestException)
                          {
                              MessageBox.Show("Сервер временно недоступен.");
                          }
                          catch (JsonException)
                          {
                              MessageBox.Show("Не удалось получить данные на эти дни");
                          }
                      }
                  }));
            }
        }

        private async Task<List<Models.Currency>> MakeRequestForCurrenciesAsync()
        {
            List<DateTime> dates = GetDates();
            List<string> serializedCurrencies = new List<string>();
            List<Models.Currency> currencies = new List<Models.Currency>();

            foreach (var date in dates)
            {
                string serializedCurrency = await RequesterToWebApi.MakeRequestAsync(_selectedCurrencyCode.ToString(), date);
                serializedCurrencies.Add(serializedCurrency);
            }

            foreach (var serializedCurrency in serializedCurrencies)
            {
                var obj = JsonSerializer.Deserialize(serializedCurrency, typeof(Models.Currency));

                if (obj is Models.Currency currency)
                {
                    currencies.Add(currency);
                }
            }

            return currencies;
        }

        private bool ValidateData()
        {
            if (_dateFrom == DateTime.MinValue || _dateTo == DateTime.MinValue)
            {
                MessageBox.Show("Введите дату!");
                return false;
            }
            else if (_dateFrom > DateTime.Now || _dateTo > DateTime.Now || _dateFrom > _dateTo)
            {
                MessageBox.Show("Дата введена неверно.");
                return false;
            }
            else if (DateTime.Now.Subtract(_dateFrom) > DateTime.Now.Subtract(new DateTime(DateTime.Now.Year - 5, 1, 1)))
            {
                MessageBox.Show("Возможно отобразить курс только за последние 5 лет.");
                return false;
            }    

            return true;
        }

        private void DrawChart(List<Models.Currency> currencies)
        {
            SeriesCollection = new ISeries[]
            {
                new LineSeries<decimal>
                {
                    DataLabelsFormatter = (point) => point.PrimaryValue.ToString("C2"),
                    Values = currencies.Select(c => c.value),
                    LineSmoothness = 0,
                }
            };
        }

        private List<DateTime> GetDates()
        {
            List<DateTime> dates = new List<DateTime>();

            for (DateTime date = _dateFrom; date <= _dateTo; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            return dates;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
