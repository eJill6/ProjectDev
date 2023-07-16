using System;
using System.Collections.Concurrent;
using CSRedis;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Cache;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;

namespace JxBackendService.Service.Cache
{
    public class RedisService : IRedisService
    {
        private static readonly double _defaultCacheSeconds = 60 * 60;

        private static readonly int _blockTimeoutSeconds = 5;

        private static readonly int _getLockTimeoutSeconds = 30;

        private static readonly ConcurrentDictionary<DbIndexes, CSRedisClient> _redisClientMap = new ConcurrentDictionary<DbIndexes, CSRedisClient>();

        private readonly IAppSettingService _appSettingService;

        public RedisService(JxApplication jxApplication)
        {
            _appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(jxApplication, SharedAppSettings.PlatformMerchant);
        }

        public T GetCache<T>(DbIndexes dbIndex, string key, bool isForceRefresh, double cacheSeconds, bool isSlidingExpiration,
            bool isDoSerialize, Func<T> getCacheData) where T : class
        {
            T cacheObject = default(T);
            string cacheJsonString = null;

            CSRedisClient csRedisClient = GetCSRedisClient(dbIndex);

            //強制更新就不用去遠端取, 直接拿最新的資料回寫
            if (!isForceRefresh)
            {
                cacheJsonString = csRedisClient.Get(key);
            }

            if (!cacheJsonString.IsNullOrEmpty())
            {
                if (isSlidingExpiration)
                {
                    csRedisClient.Expire(key, TimeSpan.FromSeconds(GetCacheSecondsOrDefault(cacheSeconds)));
                    //SetCache(csRedisClient, key, cacheJsonString, cacheSeconds, isDoSerialize: false);
                }

                if (isDoSerialize)
                {
                    cacheObject = cacheJsonString.Deserialize<T>();
                }
                else
                {
                    cacheObject = cacheJsonString as T;
                }
            }
            else
            {
                if (getCacheData != null)
                {
                    cacheObject = getCacheData.Invoke();
                }

                if (cacheObject != null)
                {
                    SetCache(csRedisClient, key, cacheObject, cacheSeconds, isDoSerialize);
                }
            }

            return cacheObject;
        }

        public void SetCache<TItem>(DbIndexes dbIndex, string key, TItem value, double cacheSeconds, bool isDoSerialize)
        {
            SetCache(GetCSRedisClient(dbIndex), key, value, cacheSeconds, isDoSerialize);
        }

        public void RemoveCache(DbIndexes dbIndex, params string[] key)
        {
            GetCSRedisClient(dbIndex).Del(key);
        }

        public long Enqueue<T>(string key, T value)
        {
            CSRedisClient csRedisClient = GetCSRedisClient(DbIndexes.Helper);
            return csRedisClient.RPush(key, value);
        }

        public T Dequeue<T>(params string[] key)
        {
            CSRedisClient csRedisClient = GetCSRedisClient(DbIndexes.Helper);
            string value = csRedisClient.BLPop(_blockTimeoutSeconds, key);

            return value.Deserialize<T>();
        }

        public bool Expire(DbIndexes dbIndex, string key, int seconds)
        {
            CSRedisClient csRedisClient = GetCSRedisClient(dbIndex);

            return csRedisClient.Expire(key, seconds);
        }

        public void DoWorkWithLock(string key, Action work)
        {
            DoWorkWithLock(key, () =>
            {
                work.Invoke();
                return true;
            });
        }

        public T DoWorkWithLock<T>(string key, Func<T> work)
        {
            if (work == null)
            {
                return default(T);
            }

            CSRedisClient csRedisClient = GetCSRedisClient(DbIndexes.Helper);
            using (CSRedisClientLock csRedisClientLock = csRedisClient.Lock(key, _getLockTimeoutSeconds))
            {
                if (csRedisClientLock != null)
                {
                    return work.Invoke();
                }
            }

            return default(T);
        }

        private void SetCache<TItem>(CSRedisClient csRedisClient, string key, TItem value, double cacheSeconds, bool isDoSerialize)
        {
            if (value == null)
            {
                csRedisClient.Del(key);
                return;
            }

            string data;

            if (isDoSerialize)
            {
                data = value.ToJsonString(ignoreNull: true);
            }
            else
            {
                data = value.ToString();
            }

            csRedisClient.Set(key, data, TimeSpan.FromSeconds(GetCacheSecondsOrDefault(cacheSeconds)));
        }

        private CSRedisClient GetCSRedisClient(DbIndexes dbIndex)
        {
            if (!_redisClientMap.TryGetValue(dbIndex, out CSRedisClient csRedisClient))
            {
                string connectionString = _appSettingService.GetRedisConnectionString(dbIndex);
                csRedisClient = new CSRedisClient(connectionString);
                RedisHelper.Initialization(csRedisClient);
                _redisClientMap.TryAdd(dbIndex, csRedisClient);
            }

            return csRedisClient;
        }

        private double GetCacheSecondsOrDefault(double cacheSeconds)
        {
            if (cacheSeconds == 0)
            {
                return _defaultCacheSeconds;
            }

            return cacheSeconds;
        }
    }
}