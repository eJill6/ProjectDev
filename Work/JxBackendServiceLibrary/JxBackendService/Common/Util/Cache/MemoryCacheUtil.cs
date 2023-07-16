using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace JxBackendService.Common.Util.Cache
{
    public static class MemoryCacheUtil
    {
        private static readonly int _defaultCacheSeconds = 60 * 10;
        private static readonly MemoryCache _memoryCache = MemoryCache.Default;
        
        //public static T GetCache<T>(string key)
        //{
        //    return _memoryCache.Get<T>(key);
        //}

        //public static T GetCache<T>(string key, int cacheSeconds, Func<T> func)
        //{
        //    return GetCache(key, false, false, cacheSeconds, func);
        //}

        public static T GetCache<T>(string key, bool isCloneInstance, bool isForceRefresh, int cacheSeconds, bool isSlidingExpiration, Func<T> getCacheData)
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

        public static void SetCache<TItem>(string key, TItem value, int cacheSeconds, bool isSlidingExpiration)
        {
            if (cacheSeconds == 0)
            {
                cacheSeconds = _defaultCacheSeconds;
            }

            CacheItemPolicy policy = new CacheItemPolicy();

            if (isSlidingExpiration)
            {
                policy.SlidingExpiration = new TimeSpan(0, 0, cacheSeconds);
            }
            else
            {
                policy.AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(cacheSeconds);
            }

            _memoryCache.Set(key, value, policy);
        }

        public static void RemoveCache(string key)
        {
            _memoryCache.Remove(key);
        }

        private static T Get<T>(string key)
        {
            object tempCacheObject = _memoryCache.Get(key);

            if (tempCacheObject is T)
            {
                return (T)tempCacheObject;
            }

            return default(T);
        }
    }
}
