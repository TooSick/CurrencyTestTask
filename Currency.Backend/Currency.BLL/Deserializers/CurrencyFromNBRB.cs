
namespace Currency.BLL.Deserializers
{
    public class CurrencyFromNBRB
    {
        public DateTime Date { get; set; }
        public string Cur_Abbreviation { get; set; }
        public decimal Cur_Scale { get; set; }
        public decimal Cur_OfficialRate { get; set; }
    }
}
