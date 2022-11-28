using System;

namespace Currency.Client.Models
{
    public class Currency
    {
        public string code { get; set; }
        public DateTime date { get; set; }
        public decimal value { get; set; }
        public decimal amount { get; set; }
    }
}
