namespace JxBackendService.Common.Util.Cache
{
    public class SetCacheParam
    {
        public CacheKey Key { get; set; }

        public int CacheSeconds { get; set; }

        public int? TempLocalMemoryCacheSeconds { get; set; }

        public bool IsSlidingExpiration { get; set; }
    }

    public class SearchCacheParam : SetCacheParam
    {
        public bool IsForceRefresh { get; set; }

        public bool IsCloneInstance { get; set; } = true;
    }
}