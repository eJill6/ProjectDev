using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSRedis;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Cache;
using JxBackendService.Model.Enums;

namespace JxBackendService.Service.Cache
{
    public class RedisService : IRedisService
    {
        private static readonly int _defaultCacheSeconds = 60 * 60;
        private static readonly ConcurrentDictionary<DbIndexes, CSRedisClient> _redisClientMap = new ConcurrentDictionary<DbIndexes, CSRedisClient>();
        private readonly IAppSettingService _appSettingService;

        public RedisService(JxApplication jxApplication)
        {
            _appSettingService = DependencyUtil.ResolveServiceForModel<IAppSettingService>(jxApplication);
        }

        public T GetCache<T>(DbIndexes dbIndex, string key, bool isForceRefresh, int cacheSeconds, bool isSlidingExpiration,
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
                    SetCache(csRedisClient, key, cacheJsonString, cacheSeconds, isDoSerialize: false);                    
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

        public void SetCache<TItem>(DbIndexes dbIndex, string key, TItem value, int cacheSeconds, bool isDoSerialize)
        {
            SetCache(GetCSRedisClient(dbIndex), key, value, cacheSeconds, isDoSerialize);
        }

        public void RemoveCache(DbIndexes dbIndex, params string[] key)
        {
            GetCSRedisClient(dbIndex).Del(key);
        }

        private void SetCache<TItem>(CSRedisClient csRedisClient, string key, TItem value, int cacheSeconds, bool isDoSerialize)
        {
            if (value == null)
            {
                csRedisClient.Del(key);
                return;
            }

            if (cacheSeconds == 0)
            {
                cacheSeconds = _defaultCacheSeconds;
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

            csRedisClient.Set(key, data, new TimeSpan(0, 0, cacheSeconds));
        }

        private CSRedisClient GetCSRedisClient(DbIndexes dbIndex)
        {
            _redisClientMap.TryGetValue(dbIndex, out CSRedisClient cSRedisClient);

            if (cSRedisClient == null)
            {
                string connectionString = _appSettingService.GetRedisConnectionString(dbIndex);
                cSRedisClient = new CSRedisClient(connectionString);
                _redisClientMap.TryAdd(dbIndex, cSRedisClient);
            }

            return cSRedisClient;
        }
    }
}
