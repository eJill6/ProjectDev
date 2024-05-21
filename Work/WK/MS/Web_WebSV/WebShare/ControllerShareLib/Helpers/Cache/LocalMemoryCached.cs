using JxBackendService.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Runtime.Caching;
using System.Text;

namespace ControllerShareLib.Helpers.Cache
{

    public class LocalMemoryCached : ICache
    {
        private static readonly ObjectCache cache = MemoryCache.Default;
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new ConcurrentDictionary<string, SemaphoreSlim>();
        private readonly Lazy<IDistributedCache> _distributedCache = null;

        public LocalMemoryCached()
        {
            _distributedCache = DependencyUtil.ResolveService<IDistributedCache>();
        }

        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }

            object cacheValue = cache.Get(key);

            if (cacheValue is T)
            {
                return (T)cacheValue;
            }

            return default;
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

        public async Task<T> GetAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }

            try
            {
                var bytes = await _distributedCache.Value.GetAsync(key);
                return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
            }
            catch (Exception ex)
            {

            }

            return default;
        }

        public async Task<bool> SetAsync<T>(string key, T obj, DateTime expired)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            if (obj == null)
            {
                return true;
            }

            var str = JsonConvert.SerializeObject(obj);
            var bytes = Encoding.UTF8.GetBytes(str);
            await _distributedCache.Value.SetAsync(key, bytes, options: new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = expired
            });
            return await Task.FromResult(true);
        }



        public async Task<T> GetOrAddAsync<T>(string funcName, string key, Func<Task<T>> invoke, DateTime expireTime)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }
            T result = default;
            var locker = _locks.GetOrAdd<SemaphoreSlim>(funcName, (key, old) => old, new SemaphoreSlim(1, 1));
            try
            {
                locker.Wait();
                var query = await GetAsync<CachedObj<T>>(key);
                if (query != null && query.ExpiredTime > DateTime.Now)
                {
                    result = query.Value;
                }
                else if (invoke != null)
                {
                    T quertResult = await invoke();
                    if (quertResult != null)
                    {
                        await SetAsync(key, new CachedObj<T>()
                        {
                            ExpiredTime = expireTime,
                            Value = quertResult
                        }, expireTime);
                        result = quertResult;
                    }
                }
            }
            finally
            {
                locker.Release();
            }

            return result;
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