using JxBackendService.Common.Util;
using SLPolyGame.Web.Interface;
using System;
using Web.Extensions;
using Web.PublicApiService;

namespace Web.Helpers.Cache
{
    public class DistributeMemcached : ICacheRemote
    {
        private IPublicApiWebSVService _publicApiWebSVService;

        public DistributeMemcached(IPublicApiWebSVService publicApiWebSVService)
        {
            _publicApiWebSVService = publicApiWebSVService;
        }

        public bool Del(string key)
        {
            if (key.IsNullOrEmpty())
            {
                return false;
            }

            _publicApiWebSVService.RemoveRemoteCacheForOldService(key);

            // redisService這邊是void 因此這邊直接回true
            return true;
        }

        public T Get<T>(string key)
        {
            if (key.IsNullOrEmpty())
            {
                return default(T);
            }

            string cacheValue = _publicApiWebSVService.GetRemoteCacheForOldService(key);
            if (cacheValue.IsNullOrEmpty())
            {
                return default(T);
            }

            return cacheValue.Deserialize<T>();
        }

        public bool Set<T>(string key, T obj)
        {
            return Set(key, obj, DateTime.Now.AddDays(GlobalCacheHelper.DefaultCacheExpireDays)); //預設30天
        }

        public bool Set<T>(string key, T obj, DateTime expired)
        {
            if (key.IsNullOrEmpty())
            {
                return false;
            }

            if (obj == null)
            {
                return true;
            }

            // 計算距離expired還有多少秒
            double cacheSeconds = expired.Subtract(DateTime.Now).TotalSeconds;

            // 因為這邊的cache時間都是用截止時間去計算的, 不是用延長的方式, 所以 isSlidingExpiration 都設成false
            bool isSlidingExpiration = false;
            string value = obj.ToJsonString();

            _publicApiWebSVService.SetRemoteCacheForOldService(
                new SLPolyGame.Web.Model.SetRemoteCacheParam()
                {
                    Key = key,
                    CacheSeconds = Convert.ToInt32(cacheSeconds),
                    IsSlidingExpiration = isSlidingExpiration,
                    Value = value
                });

            // redisService這邊是void 因此這邊直接回true
            return true;
        }
    }
}