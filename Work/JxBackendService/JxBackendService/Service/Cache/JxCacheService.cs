using System;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Cache;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;

namespace JxBackendService.Service.Cache
{
    public class JxCacheService : IJxCacheService
    {
        private readonly JxApplication _jxApplication;
        private static object _cacheWriteLocker = new object();
        private static readonly string _lockerKeyFormat = "@#$Locker_{0}";
        private static readonly int _tempLocalMemoryCacheSeconds = 3 * 60;
        private static readonly bool _isTempLocalMemorySliding = false;
        private readonly IRedisService _redisService;


        public JxCacheService(JxApplication jxApplication)
        {
            _jxApplication = jxApplication;
            _redisService = DependencyUtil.ResolveServiceForModel<IRedisService>(jxApplication);
        }

        public T GetCache<T>(CacheKey cacheKey, Func<T> getCacheData = null) where T : class
        {
            return GetCache(cacheKey, false, getCacheData);
        }

        public T GetCache<T>(CacheKey cacheKey, bool isSliding, Func<T> getCacheData) where T : class
        {
            return GetCache(new SearchCacheParam()
            {
                Key = cacheKey,
                IsSlidingExpiration = isSliding
            }, getCacheData);
        }

        public T GetCache<T>(SearchCacheParam searchCacheParam) where T : class
        {
            return GetCache<T>(searchCacheParam, null);
        }

        public T GetCache<T>(SearchCacheParam searchCacheParam, Func<T> getCacheData) where T : class
        {
            T cacheModel = default(T);
            string fullCacheKey = GetFullCacheKeyValue(searchCacheParam.Key);

            //取得資料的時候加上lock功能避免併發DB
            object locker = GetLocker(fullCacheKey);

            lock (locker)
            {
                switch (searchCacheParam.Key.CacheType)
                {
                    case CacheTypes.LocalMemory:
                        cacheModel = MemoryCacheUtil.GetCache(
                            fullCacheKey,
                            searchCacheParam.IsCloneInstance,
                            searchCacheParam.IsForceRefresh,
                            searchCacheParam.CacheSeconds,
                            searchCacheParam.IsSlidingExpiration,
                            getCacheData);
                        break;
                    case CacheTypes.CacheServer:
                        cacheModel = _redisService.GetCache(searchCacheParam.Key.DbIndex,
                            fullCacheKey,
                            searchCacheParam.IsForceRefresh,
                            searchCacheParam.CacheSeconds,
                            searchCacheParam.IsSlidingExpiration,
                            searchCacheParam.Key.IsDoSerialize,
                            getCacheData);

                        //cacheModel = CacheServerUtil.GetCache(
                        //    fullCacheKey,
                        //    searchCacheParam.IsForceRefresh,
                        //    searchCacheParam.CacheSeconds,
                        //    searchCacheParam.IsSlidingExpiration,
                        //    searchCacheParam.Key.IsFormatKey,
                        //    getCacheData);

                        break;
                    case CacheTypes.LocalAndCacheServer:
                        //先拿local, 沒有再拿遠端(因為多台機器情況下, 可能遠端已經被其他台寫入)
                        //local的過期時間較短,讓local過期後去遠端能更新後的資料
                        int localMemoryCacheSeconds = _tempLocalMemoryCacheSeconds;

                        if (searchCacheParam.CacheSeconds < localMemoryCacheSeconds)
                        {
                            localMemoryCacheSeconds = searchCacheParam.CacheSeconds / 2;

                            if (localMemoryCacheSeconds == 0)
                            {
                                localMemoryCacheSeconds = 1;
                            }
                        }

                        cacheModel = MemoryCacheUtil.GetCache<T>(
                            fullCacheKey,
                            searchCacheParam.IsCloneInstance,
                            searchCacheParam.IsForceRefresh,
                            localMemoryCacheSeconds,
                            _isTempLocalMemorySliding,
                            null);

                        if (cacheModel == null)
                        {
                            cacheModel = _redisService.GetCache(searchCacheParam.Key.DbIndex,
                                fullCacheKey,
                                searchCacheParam.IsForceRefresh,
                                searchCacheParam.CacheSeconds,
                                searchCacheParam.IsSlidingExpiration,
                                searchCacheParam.Key.IsDoSerialize,
                                getCacheData);

                            //cacheModel = CacheServerUtil.GetCache(
                            //    fullCacheKey,
                            //    searchCacheParam.IsForceRefresh,
                            //    searchCacheParam.CacheSeconds,
                            //    searchCacheParam.IsSlidingExpiration,
                            //    searchCacheParam.Key.IsFormatKey,
                            //    getCacheData);

                            //從遠端更新回local memory
                            if (cacheModel != null)
                            {
                                MemoryCacheUtil.SetCache(
                                    fullCacheKey,
                                    cacheModel,
                                    localMemoryCacheSeconds,
                                    _isTempLocalMemorySliding);
                            }
                        }

                        break;
                }
            }

            return cacheModel;
        }

        public void SetCache<T>(SetCacheParam setCacheParam, T value) where T : class
        {
            string fullCacheKey = GetFullCacheKeyValue(setCacheParam.Key);

            switch (setCacheParam.Key.CacheType)
            {
                case CacheTypes.LocalMemory:
                    MemoryCacheUtil.SetCache(fullCacheKey, value, setCacheParam.CacheSeconds, setCacheParam.IsSlidingExpiration);
                    break;
                case CacheTypes.CacheServer:
                    _redisService.SetCache(setCacheParam.Key.DbIndex, fullCacheKey, value, setCacheParam.CacheSeconds,
                        setCacheParam.Key.IsDoSerialize);
                    //CacheServerUtil.SetCache(fullCacheKey, value, setCacheParam.CacheSeconds, setCacheParam.IsSlidingExpiration,
                    //    setCacheParam.Key.IsDoSerialize);
                    break;
                case CacheTypes.LocalAndCacheServer:
                    MemoryCacheUtil.SetCache(fullCacheKey, value, _tempLocalMemoryCacheSeconds, setCacheParam.IsSlidingExpiration);
                    _redisService.SetCache(setCacheParam.Key.DbIndex, fullCacheKey, value, setCacheParam.CacheSeconds,
                        setCacheParam.Key.IsDoSerialize);
                    //CacheServerUtil.SetCache(fullCacheKey, value, setCacheParam.CacheSeconds, setCacheParam.IsSlidingExpiration,
                    //    setCacheParam.Key.IsDoSerialize);
                    break;
            }
        }

        public void RemoveCache(CacheKey cacheKey)
        {
            string fullCacheKey = GetFullCacheKeyValue(cacheKey);

            switch (cacheKey.CacheType)
            {
                case CacheTypes.LocalMemory:
                    MemoryCacheUtil.RemoveCache(fullCacheKey);
                    break;
                case CacheTypes.CacheServer:
                    _redisService.RemoveCache(cacheKey.DbIndex, fullCacheKey);
                    //CacheServerUtil.RemoveCache(fullCacheKey);
                    break;
                case CacheTypes.LocalAndCacheServer:
                    //CacheServerUtil.RemoveCache(fullCacheKey);
                    _redisService.RemoveCache(cacheKey.DbIndex, fullCacheKey);
                    MemoryCacheUtil.RemoveCache(fullCacheKey);
                    break;
            }
        }

        private string GetFullCacheKeyValue(CacheKey cacheKey)
        {
            string fullCacheKey = cacheKey.Value;

            if (cacheKey.IsFormatKey)
            {
                fullCacheKey = $"{SharedAppSettings.GetEnvironmentCode(_jxApplication).Value}.{cacheKey.Value}";
            }

            return fullCacheKey;
        }

        private static object GetLocker(string key)
        {
            string lockerCacheKey = string.Format(_lockerKeyFormat, key);
            object locker = null;

            lock (_cacheWriteLocker)
            {
                locker = MemoryCacheUtil.GetCache(lockerCacheKey, false, false, 300, true, () =>
                {
                    return new object();
                });
            }

            return locker;
        }
    }
}
