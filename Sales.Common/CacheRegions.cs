
namespace Sales.Common
{
    public class CacheRegions
    {
        public string Value { get; private set; }

        private CacheRegions(string value) => Value = value;

        public static CacheRegions OPT { get { return CreateNewRegion("OPT"); } }

        private static CacheRegions CreateNewRegion(string region)
        {
            return new CacheRegions(region);
        }
    }
}
