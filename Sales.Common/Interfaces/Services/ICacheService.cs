using Sales.Common.Entities;

namespace Sales.Common.Interfaces.Services
{
    public interface ICacheService
    {
        void UpsertItem(CacheItemEntity cacheItem);

        object GetItem(CacheRegions region, string key);

        T GetItem<T>(CacheRegions region, string key);
    }
}
