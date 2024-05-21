using System;
using System.Runtime.Caching;

namespace Web.Helpers.Cache
{

    public class LocalMemoryCached : ICache
    {
        private static readonly ObjectCache cache = MemoryCache.Default;

        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default(T);
            }

            object cacheValue = cache.Get(key);

            if (cacheValue is T)
            {
                return (T)cacheValue;
            }

            return default(T);
        }

        public bool Set<T>(string key, T obj)
        {
            return Set(key, obj, DateTime.Now.AddDays(GlobalCacheHelper.DefaultCacheExpireDays));
        }

        public bool Set<T>(string key, T obj, DateTime expired)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            if (obj == null)
            {
                return true;
            }

            CacheItemPolicy policy = new CacheItemPolicy
            {
                AbsoluteExpiration = new DateTimeOffset(expired)
            };

            cache.Set(key, obj, policy);

            return true;
        }

        public bool Del(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            var obj = cache.Remove(key);
            return obj != null;
        }
    }
}