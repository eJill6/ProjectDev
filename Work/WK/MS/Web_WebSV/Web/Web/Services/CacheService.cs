using System;
using Web.Helpers;
using Web.Helpers.Cache;

namespace Web.Services
{
    public class CacheService : ICacheService
    {
        private ICacheRemote _remoteInstance;
        private ICache _localInstance;

        public CacheService(ICacheRemote remoteInstance, ICache localInstance)
        {
            _remoteInstance = remoteInstance;
            _localInstance = localInstance;
        }

        public ICache RemoteInstance
        {
            get
            {
                return _remoteInstance;
            }
        }

        public ICache LocalInstance
        {
            get
            {
                return _localInstance;
            }
        }

        public T Get<T>(string key, DateTime? expiredTime = null, Func<T> getData = null)
        {
            bool slideExpire = false;
            return Get<T>(key, slideExpire, expiredTime, getData);
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
                return default(T);
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
                return default(T);
            }

            var remoteObj = RemoteInstance.Get<CachedObj<T>>(key);

            if (remoteObj != null)
            {
                return remoteObj.Value;
            }

            return default(T);
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
                return default(T);
            }

            var remoteObj = RemoteInstance.Get<T>(key);

            if (remoteObj != null)
            {
                return remoteObj;
            }

            return default(T);
        }

        public T Get<T>(string key, bool slideExpire, DateTime? expiredTime = null, Func<T> getData = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default(T);
            }

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
                    if (localObj.ExpiredTime < DateTime.Now)
                    {
                        return default(T);
                    }

                    return localObj.Value;
                }

                var remoteObj = RemoteInstance.Get<CachedObj<T>>(key);

                if (remoteObj != null)
                {
                    if (remoteObj.ExpiredTime < DateTime.Now)
                    {
                        return default(T); ;
                    }

                    _localInstance.Set(key, remoteObj, remoteObj.ExpiredTime);
                    return remoteObj.Value;
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

            return default(T);
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

        public void Del(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            RemoteInstance.Del(key);
            LocalInstance.Del(key);
        }
    }
}