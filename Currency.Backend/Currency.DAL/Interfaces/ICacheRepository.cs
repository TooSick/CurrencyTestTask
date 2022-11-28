
namespace Currency.DAL.Interfaces
{
    public interface ICacheRepository<T>
    {
        T GetItem(string currencyCode, DateTime date);
        void AddItems(params T[] items);
        void DeleteItems(params T[] items);
    }
}
