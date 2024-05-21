using Microsoft.Extensions.Caching.Memory;

namespace MS.Core.Extensions
{
    public static class MemoryCacheExtension
    {
        public static async Task<T?> GetOrSetAsync<T>(this IMemoryCache memoryCache, string cacheKey, Func<Task<T?>> func, long expiredMilliseconds)
            where T : class
        {
            var result = memoryCache.Get(cacheKey) as T;

            if (result != null)
            {
                return result;
            }

            T? res = await func();
            if(res == null)
            {
                return null;
            }

            memoryCache.Set(cacheKey, res, DateTime.Now.AddMilliseconds(expiredMilliseconds));

            return res;
        }
    }
}
