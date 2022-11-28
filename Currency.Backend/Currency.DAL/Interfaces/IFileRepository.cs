
namespace Currency.DAL.Interfaces
{
    public interface IFileRepository<T>
    {
        public static string DirPath { get; set; }
        Task<IEnumerable<T>> GetAllItemsAsync();
        void AddItems(params T[] items);
        void DeleteItems(params T[] items);
    }
}
