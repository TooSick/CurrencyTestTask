
namespace Currency.Domain.Models
{
    public class Currency
    {
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public decimal Amount { get; set; }
    }
}
