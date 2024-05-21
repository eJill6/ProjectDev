using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace SLPolyGame.Web.Common
{
    public interface ICacheBase
    {
        bool AddCache(string key, string value, int expireSeconds, bool isSliding);

        CacheObj CreateCacheObj(string key, string value, int expireSeconds, bool isSliding);

        bool DeleteCache(string key);

        Task<T> Get<T>(string key);

        string GetCache(string key);

        Dictionary<string, string> GetCache(string[] keys);
    }

    public class CacheBase : ICacheBase
    {
        private readonly Lazy<ILogUtilService> _logUtilService;

        /// <inheritdoc cref="Lazy{FreeRedis.RedisClient}"/>
        private Lazy<FreeRedis.RedisClient> _cli = null;

        public CacheBase()
        {
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();

            _cli = new Lazy<FreeRedis.RedisClient>(() =>
            {
                var redisConnection = CommUtil.CommUtil.DecryptDES(GlobalCache.RedisConnectionString);

                string paramString = $",poolsize=2000,ssl=false,writeBuffer=20480";

                var setting = redisConnection + paramString;
                var r = new FreeRedis.RedisClient(setting);

                r.Serialize = obj => JsonConvert.SerializeObject(obj);
                r.Deserialize = (json, type) => JsonConvert.DeserializeObject(json, type);
                return r;
            });
        }

        // 增加缓存数据
        public bool AddCache(string key, string value, int expireSeconds, bool isSliding)
        {
            bool result = true;
            CacheObj obj = CreateCacheObj(key, value, expireSeconds, isSliding);
            _cli.Value.Set(obj.Key, obj, obj.ExpireSeconds);

            return result;
        }

        public CacheObj CreateCacheObj(string key, string value, int expireSeconds, bool isSliding)
        {
            var obj = new CacheObj
            {
                Key = key,
                Value = value,
                ExpireSeconds = expireSeconds,
                IsSliding = isSliding,
                InsertTime = DateTime.Now
            };

            return obj;
        }

        // 删除缓存数据
        public bool DeleteCache(string key)
        {
            bool result = true;

            _cli.Value.Del(key);

            return result;
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetCache(string key)
        {
            string result = null;
            var cacheData = _cli.Value.Get<CacheObj>(key);

            if (cacheData != null && cacheData.InsertTime.AddSeconds(cacheData.ExpireSeconds) > DateTime.Now)
            {
                result = cacheData.Value;

                if (cacheData.IsSliding)
                {
                    AddCache(cacheData.Key, cacheData.Value, cacheData.ExpireSeconds, cacheData.IsSliding);
                }
            }

            return result;
        }

        /// <summary>
        /// 取得cache資料，泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> Get<T>(string key)
        {
            T result = default(T);
            await Task.Run(() =>
            {
                var method = MethodBase.GetCurrentMethod();
                try
                {
                    result = _cli.Value.Get<T>(key);
                }
                catch (Exception ex)
                {
                    _logUtilService.Value.Error($"{method.Name} fail!, key:{key}, ex:{ex}");
                }
            });

            return result;
        }

        public Dictionary<string, string> GetCache(string[] keys)
        {
            var result = new Dictionary<string, string>();
            var cacheDatas = _cli.Value.MGet<CacheObj>(keys);

            foreach (var item in cacheDatas)
            {
                var cacheData = item;
                if (cacheData != null && cacheData.InsertTime.AddSeconds(cacheData.ExpireSeconds) > DateTime.Now)
                {
                    result[item.Key] = cacheData.Value;

                    if (cacheData.IsSliding)
                    {
                        AddCache(cacheData.Key, cacheData.Value, cacheData.ExpireSeconds, cacheData.IsSliding);
                    }
                }
            }

            return result;
        }
    }
}