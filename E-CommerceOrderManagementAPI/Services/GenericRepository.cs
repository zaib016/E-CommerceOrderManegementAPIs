using E_CommerceOrderManagementAPI.Data;
using E_CommerceOrderManagementAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceOrderManagementAPI.Services
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private ApplicationDbContext _dbContext;
        private ICacheRepository _cache;

        public GenericRepository(ApplicationDbContext dbContext, ICacheRepository cacheRepository)
        {
            _dbContext = dbContext;
            _cache = cacheRepository;
        }

        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            _cache.Remove($"{typeof(T).Name}");
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            string cacheKey = $"{typeof(T).Name}_List";
            var cachedData = _cache.Get<T>(cacheKey);

            if(cachedData != null)
            {
                return cachedData;
            }
            
            var data = await _dbContext.Set<T>().FindAsync(id);
            _cache.Set(cacheKey, data, 5);
            return  data;
        }

        public async Task<List<T>> GetListAsync()
        {
            string cacheKey = $"{typeof(T).Name}_List";
            var cahcedData = _cache.Get<List<T>>(cacheKey);

            if (cahcedData != null)
            {
                return cahcedData;
            }
            Console.WriteLine("Cache Hit");

            var data = await _dbContext.Set<T>().ToListAsync();
            _cache.Set(cacheKey, data, 5);
            return data;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _cache.Remove($"{typeof(T).Name}");
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
