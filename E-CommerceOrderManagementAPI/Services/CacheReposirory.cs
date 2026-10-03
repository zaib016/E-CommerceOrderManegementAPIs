using E_CommerceOrderManagementAPI.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace E_CommerceOrderManagementAPI.Services
{
    public class CacheReposirory : ICacheRepository
    {
        private IMemoryCache _cache;

        public CacheReposirory(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T? Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public void Set<T>(string key, T value, int minutes)
        {
            _cache.Set(key, value, TimeSpan.FromMinutes(minutes));
        }
    }
}
