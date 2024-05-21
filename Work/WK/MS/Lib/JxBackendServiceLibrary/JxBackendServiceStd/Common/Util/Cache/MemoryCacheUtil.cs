using System;
using System.Runtime.Caching;

namespace JxBackendService.Common.Util.Cache
{
    public static class MemoryCacheUtil
    {
        private static readonly double s_defaultCacheSeconds = 60 * 10;

        private static readonly MemoryCache s_memoryCache = MemoryCache.Default;

        //public static T GetCache<T>(string key)
        //{
        //    return _memoryCache.Get<T>(key);
        //}

        //public static T GetCache<T>(string key, double cacheSeconds, Func<T> func)
        //{
        //    return GetCache(key, false, false, cacheSeconds, func);
        //}

        public static T GetCache<T>(string key, bool isCloneInstance, bool isForceRefresh, double cacheSeconds, bool isSlidingExpiration, Func<T> getCacheData)
        {
            T cacheObject = default(T);

            if (!isForceRefresh)
            {
                cacheObject = Get<T>(key);
            }

            bool isSetCache = false;

            if (cacheObject == null && getCacheData != null)
            {
                cacheObject = getCacheData.Invoke();

                if (cacheObject != null)
                {
                    isSetCache = true;
                }
            }
            else if (isSlidingExpiration)
            {
                isSetCache = true;
            }

            if (cacheObject != null && isSetCache)
            {
                SetCache(key, cacheObject, cacheSeconds, isSlidingExpiration);
            }

            if (isCloneInstance)
            {
                return cacheObject.CloneByJson();
            }

            return cacheObject;
        }

        public static void SetCache<TItem>(string key, TItem value, double cacheSeconds, bool isSlidingExpiration)
        {
            if (value == null)
            {
                RemoveCache(key);

                return;
            }

            if (cacheSeconds == 0)
            {
                cacheSeconds = s_defaultCacheSeconds;
            }

            CacheItemPolicy policy = new CacheItemPolicy();

            if (isSlidingExpiration)
            {
                policy.SlidingExpiration = TimeSpan.FromSeconds(cacheSeconds);
            }
            else
            {
                policy.AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(cacheSeconds);
            }

            s_memoryCache.Set(key, value, policy);
        }

        public static void RemoveCache(string key)
        {
            if (!key.IsNullOrEmpty())
            {
                s_memoryCache.Remove(key);
            }
        }

        private static T Get<T>(string key)
        {
            object tempCacheObject = s_memoryCache.Get(key);

            if (tempCacheObject is T)
            {
                return (T)tempCacheObject;
            }

            return default(T);
        }
    }
}