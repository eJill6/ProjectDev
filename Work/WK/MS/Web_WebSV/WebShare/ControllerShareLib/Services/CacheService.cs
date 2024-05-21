using ControllerShareLib.Helpers;
using ControllerShareLib.Helpers.Cache;
using ControllerShareLib.Interfaces.Service;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;

namespace ControllerShareLib.Services
{
    public class CacheService : ICacheService
    {
        private static readonly string s_lockerKeyFormat = "@#$Locker_{0}";

        private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        private readonly Lazy<ICacheRemote> _remoteInstance;

        private readonly Lazy<ICache> _localInstance;

        public CacheService()
        {
            _remoteInstance = DependencyUtil.ResolveService<ICacheRemote>();
            _localInstance = DependencyUtil.ResolveService<ICache>(); ;
        }

        public ICacheRemote RemoteInstance
        {
            get
            {
                return _remoteInstance.Value;
            }
        }

        public ICache LocalInstance => _localInstance.Value;

        public T Get<T>(string key, DateTime? expiredTime = null, Func<T> getData = null)
        {
            bool slideExpire = false;

            return Get(key, slideExpire, expiredTime, getData);
        }

        /// <summary>
        /// 如果 Redis 沒有就重取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="getData"></param>
        /// <param name="expiredTime"></param>
        /// <returns></returns>
        public T GetOrSetByRedis<T>(string key, Func<T> getData, DateTime expiredTime)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }

            var remoteObj = RemoteInstance.Get<CachedObj<T>>(key);

            if (remoteObj != null)
            {
                return remoteObj.Value;
            }

            T data = getData();

            if (data != null)
            {
                Set(key, data, expiredTime);
            }

            return data;
        }

        /// <summary>
        /// 如果直接從Redis取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetByRedis<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }

            var remoteObj = RemoteInstance.Get<CachedObj<T>>(key);

            if (remoteObj != null)
            {
                return remoteObj.Value;
            }

            return default;
        }

        public Dictionary<string, T> GetByRedis<T>(string[] keys)
        {
            var result = new Dictionary<string, T>();
            if (keys.Length <= 0)
            {
                return result;
            }

            var remoteObjs = RemoteInstance.Get<CachedObj<T>>(keys);

            foreach (var item in remoteObjs)
            {
                var remoteObj = item.Value;
                if (remoteObj != null)
                {
                    result[item.Key] = remoteObj.Value;
                }
            }

            return result;
        }

        public async Task<Dictionary<string, T>> GetByRedisAsync<T>(string[] keys)
        {
            var result = new Dictionary<string, T>();
            if (keys.Length <= 0)
            {
                return result;
            }

            var remoteObjs = await RemoteInstance.GetAsync<CachedObj<T>>(keys);

            foreach (var item in remoteObjs)
            {
                var remoteObj = item.Value;
                if (remoteObj != null)
                {
                    result[item.Key] = remoteObj.Value;
                }
            }

            return result;
        }

        /// <summary>
        /// 如果直接從Redis取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetByRedisRawData<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }

            var remoteObj = RemoteInstance.Get<T>(key);

            if (remoteObj != null)
            {
                return remoteObj;
            }

            return default;
        }

        public T Get<T>(string key, bool slideExpire, DateTime? expiredTime = null, Func<T> getData = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }

            //加上lock功能避免併發後續相關的服務
            var locker = GetLocker(key).ConfigureAwait(false).GetAwaiter().GetResult();

            try
            {
                locker.Wait();
                if (slideExpire)
                {
                    var remoteObj = RemoteInstance.Get<CachedObj<T>>(key);

                    if (remoteObj != null)
                    {
                        RemoteInstance.Set(key, remoteObj, DateTime.Now.AddSeconds(remoteObj.ExpiredSeconds));

                        return remoteObj.Value;
                    }
                }
                else
                {
                    var localObj = LocalInstance.Get<CachedObj<T>>(key);

                    if (localObj != null)
                    {
                        if (localObj.ExpiredTime >= DateTime.Now)
                        {
                            return localObj.Value;
                        }
                    }

                    var remoteObj = RemoteInstance.Get<CachedObj<T>>(key);

                    if (remoteObj != null)
                    {
                        if (remoteObj.ExpiredTime >= DateTime.Now)
                        {
                            _localInstance.Value.Set(key, remoteObj, remoteObj.ExpiredTime);

                            return remoteObj.Value;
                        }
                    }
                }

                if (getData != null)
                {
                    if (!expiredTime.HasValue)
                    {
                        expiredTime = DateTime.Now.AddDays(GlobalCacheHelper.DefaultCacheExpireDays);
                    }

                    T data = getData.Invoke();

                    if (data != null)
                    {
                        Set(key, data, expiredTime.Value);
                    }

                    return data;
                }
            }
            finally
            {
                locker.Release();
            }

            return default;
        }

        public T GetLocalCache<T>(string key,
            Func<T> getData = null, Func<T, DateTime> expiredTimeLogic = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }

            var locker = GetLocker(key).ConfigureAwait(false).GetAwaiter().GetResult();

            try
            {
                locker.Wait();

                var localObj = LocalInstance.Get<CachedObj<T>>(key);

                if (localObj != null)
                {
                    if (localObj.ExpiredTime >= DateTime.Now)
                    {
                        return localObj.Value;
                    }
                }

                if (getData != null)
                {
                    T data = getData.Invoke();

                    if (data != null)
                    {
                        var expiredTime = DateTime.Now.AddDays(GlobalCacheHelper.DefaultCacheExpireDays);
                        if (expiredTimeLogic != null)
                        {
                            expiredTime = expiredTimeLogic(data);
                        }
                        SetLocalCache(key, data, expiredTime);
                    }

                    return data;
                }
            }
            finally
            {
                locker.Release();
            }

            return default;
        }

        public bool Set<T>(string key, T value)
        {
            return Set(key, value, DateTime.Now.AddDays(GlobalCacheHelper.DefaultCacheExpireDays));
        }

        public bool Set<T>(string key, T value, DateTime expired)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            var cachedOjb = new CachedObj<T>
            {
                Value = value,
                ExpiredTime = expired,
                ExpiredSeconds = expired.Subtract(DateTime.Now).TotalSeconds
            };

            RemoteInstance.Set(key, cachedOjb, expired);

            return LocalInstance.Set(key, cachedOjb, expired);
        }
        
        public bool SetLocalCache<T>(string key, T value, DateTime expired)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            var cachedOjb = new CachedObj<T>
            {
                Value = value,
                ExpiredTime = expired,
                ExpiredSeconds = expired.Subtract(DateTime.Now).TotalSeconds
            };

            return LocalInstance.Set(key, cachedOjb, expired);
        }

        public void Del(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            RemoteInstance.Del(key);
            LocalInstance.Del(key);
        }

        public async Task<T> GetLocalCacheAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }

            var localObj = await LocalInstance.GetAsync<CachedObj<T>>(key);

            if (localObj != null)
            {
                if (localObj.ExpiredTime >= DateTime.Now)
                {
                    return localObj.Value;
                }
            }

            return default;
        }

        public async Task<bool> SetLocalCacheAsync<T>(string key, T value, DateTime expired)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            var cachedOjb = new CachedObj<T>
            {
                Value = value,
                ExpiredTime = expired,
                ExpiredSeconds = expired.Subtract(DateTime.Now).TotalSeconds
            };

            return await LocalInstance.SetAsync(key, cachedOjb, expired);
        }
        
        public async Task<bool> SAddAsync(string key, object[] values, DateTime expired)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }

            return await RemoteInstance.SAddAsync(key, values, expired);
        }
        
        public async Task<T[]> SMembersAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }
            
            return await RemoteInstance.SMembersAsync<T>(key);
        }

        private async Task<SemaphoreSlim> GetLocker(string key)
        {
            string lockerCacheKey = string.Format(s_lockerKeyFormat, key);
            SemaphoreSlim locker = null;

            try
            {
                await _lock.WaitAsync();

                locker = MemoryCacheUtil.GetCache(
                    lockerCacheKey,
                    isCloneInstance: false,
                    isForceRefresh: false,
                    cacheSeconds: 300,
                    isSlidingExpiration: true,
                    getCacheData: () =>
                    {
                        return new SemaphoreSlim(1, 1);
                    });
            }
            finally
            {
                _lock.Release();
            }

            return locker;
        }
    }
}