using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace PolyDataBase.Extensions
{
    public static class MemoryCacheExtension
    {
        public static async Task<T> GetOrSetAsync<T>(this MemoryCache memoryCache, string cacheKey, Func<Task<T>> func, long expiredMilliseconds) 
            where T : class
        {
            var result = memoryCache.Get(cacheKey) as T;

            if (result != null)
            {
                return result;
            }

            T res = await func();

            memoryCache.Set(cacheKey, res, DateTime.Now.AddMilliseconds(expiredMilliseconds));

            return res;
        }
    }
}