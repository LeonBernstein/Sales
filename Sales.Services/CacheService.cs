using Microsoft.Extensions.Caching.Memory;
using Sales.Common;
using Sales.Common.Entities;
using Sales.Common.Interfaces.Services;

namespace Sales.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache) => _cache = cache;

        public void UpsertItem(CacheItemEntity cacheItem)
        {
            string key = cacheItem.Region + "-" + cacheItem.Key;
            _cache.Set(key, cacheItem.Item, cacheItem.Expiration);
        }

        public object GetItem(CacheRegions region, string key)
        {
            return GetItem<object>(region, key);
        }

        public T GetItem<T>(CacheRegions region, string key)
        {
            key = region.Value + "-" + key;
            return (T)_cache.Get(key);
        }

        public void RemoveItem(CacheRegions region, string key)
        {
            key = region.Value + "-" + key;
            _cache.Remove(key);
        }
    }
}
