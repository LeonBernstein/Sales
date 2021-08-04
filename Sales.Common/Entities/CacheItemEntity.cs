using System;

namespace Sales.Common.Entities
{
    public class CacheItemEntity
    {
        public string Key { get; private set; }
        public string Region { get; private set; }
        public object Item { get; private set; }
        public TimeSpan Expiration { get; private set; }

        private CacheItemEntity(string key, string region, object item, TimeSpan expiration)
        {
            Key = key;
            Region = region;
            Item = item;
            Expiration = expiration;
        }

        public static CacheItemEntity CreateOTPCacheItem(string key, string item, int expirationInMins)
        {
            return new CacheItemEntity(key, CacheRegions.OPT.Value, item, TimeSpan.FromMinutes(expirationInMins));
        }
    }
}
