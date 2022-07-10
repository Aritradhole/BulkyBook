namespace BulkyBookWeb.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(string url, int Id);
        Task<IEnumerable<T>> GetAllAsync(string url);
        Task<bool> CreateAsync(string url, T objToCreaten);
        Task<bool> Updatesync(string url, T objToUpdate);
        Task<bool> DeleteAsync(string url, int id);

    }
}
