namespace E_CommerceOrderManagementAPI.Services.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetListAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}
