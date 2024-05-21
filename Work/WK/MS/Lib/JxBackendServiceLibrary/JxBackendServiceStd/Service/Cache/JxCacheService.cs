using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Cache;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using System;

namespace JxBackendService.Service.Cache
{
    public class JxCacheService : IJxCacheService
    {
        private static readonly object s_cacheWriteLocker = new object();

        private static readonly object s_initLocker = new object();

        private static readonly string s_lockerKeyFormat = "@#$Locker_{0}";

        private static readonly int s_tempLocalMemoryCacheSeconds = 3 * 60;

        private static readonly bool s_isTempLocalMemorySliding = false;

        private static readonly string s_deleteLocalCacheChannel = "DeleteLocalCache";

        private static bool s_isSubscribeRedis = false;

        private readonly Lazy<IRedisService> _redisService;

        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        public JxCacheService()
        {
            _redisService = DependencyUtil.ResolveService<IRedisService>();

            if (!s_isSubscribeRedis)
            {
                lock (s_initLocker)
                {
                    if (s_isSubscribeRedis)
                    {
                        return;
                    }

                    _redisService.Value.Subscribe(DbIndexes.Default, s_deleteLocalCacheChannel, (message) =>
                    {
                        var deleteLocalCacheParam = message.Deserialize<DeleteLocalCacheParam>();

                        if (deleteLocalCacheParam.MachineName == Environment.MachineName &&
                            deleteLocalCacheParam.ApplicationValue == s_environmentService.Value.Application.Value)
                        {
                            return;
                        }

                        object locker = GetLocker(deleteLocalCacheParam.FullCacheKey);

                        lock (locker)
                        {
                            MemoryCacheUtil.RemoveCache(deleteLocalCacheParam.FullCacheKey);
                        }
                    });

                    s_isSubscribeRedis = true;
                }
            }
        }

        public T GetCache<T>(CacheKey cacheKey, Func<T> getCacheData = null) where T : class
        {
            return GetCache(cacheKey, isSliding: false, getCacheData);
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
                switch (GetCacheType(searchCacheParam.Key))
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
                        cacheModel = _redisService.Value.GetCache(searchCacheParam.Key.DbIndex,
                            fullCacheKey,
                            searchCacheParam.IsForceRefresh,
                            searchCacheParam.CacheSeconds,
                            searchCacheParam.IsSlidingExpiration,
                            searchCacheParam.Key.IsDoSerialize,
                            getCacheData);

                        break;

                    case CacheTypes.LocalAndCacheServer:
                        //先拿local, 沒有再拿遠端(因為多台機器情況下, 可能遠端已經被其他台寫入)
                        //local的過期時間較短,讓local過期後去遠端能更新後的資料
                        double localMemoryCacheSeconds = s_tempLocalMemoryCacheSeconds;

                        if (searchCacheParam.TempLocalMemoryCacheSeconds.HasValue)
                        {
                            localMemoryCacheSeconds = searchCacheParam.TempLocalMemoryCacheSeconds.Value;
                        }

                        if (searchCacheParam.CacheSeconds > 0 && searchCacheParam.CacheSeconds < localMemoryCacheSeconds)
                        {
                            localMemoryCacheSeconds = searchCacheParam.CacheSeconds / 2.0;

                            if (localMemoryCacheSeconds == 0)
                            {
                                localMemoryCacheSeconds = searchCacheParam.CacheSeconds;
                            }
                        }

                        cacheModel = MemoryCacheUtil.GetCache<T>(
                            fullCacheKey,
                            searchCacheParam.IsCloneInstance,
                            searchCacheParam.IsForceRefresh,
                            localMemoryCacheSeconds,
                            s_isTempLocalMemorySliding,
                            null);

                        if (cacheModel == null)
                        {
                            cacheModel = _redisService.Value.GetCache(searchCacheParam.Key.DbIndex,
                                fullCacheKey,
                                searchCacheParam.IsForceRefresh,
                                searchCacheParam.CacheSeconds,
                                searchCacheParam.IsSlidingExpiration,
                                searchCacheParam.Key.IsDoSerialize,
                                getCacheData);

                            //從遠端更新回local memory
                            if (cacheModel != null)
                            {
                                MemoryCacheUtil.SetCache(
                                    fullCacheKey,
                                    cacheModel,
                                    localMemoryCacheSeconds,
                                    s_isTempLocalMemorySliding);
                            }
                        }

                        if (searchCacheParam.IsForceRefresh)
                        {
                            PublishDeleteLocalCacheMessage(fullCacheKey);
                        }

                        break;
                }
            }

            return cacheModel;
        }

        public void SetCache<T>(SetCacheParam setCacheParam, T value) where T : class
        {
            string fullCacheKey = GetFullCacheKeyValue(setCacheParam.Key);

            switch (GetCacheType(setCacheParam.Key))
            {
                case CacheTypes.LocalMemory:
                    MemoryCacheUtil.SetCache(fullCacheKey, value, setCacheParam.CacheSeconds, setCacheParam.IsSlidingExpiration);
                    break;

                case CacheTypes.CacheServer:
                    _redisService.Value.SetCache(setCacheParam.Key.DbIndex, fullCacheKey, value, setCacheParam.CacheSeconds,
                        setCacheParam.Key.IsDoSerialize);

                    break;

                case CacheTypes.LocalAndCacheServer:
                    MemoryCacheUtil.SetCache(fullCacheKey, value, s_tempLocalMemoryCacheSeconds, s_isTempLocalMemorySliding);

                    _redisService.Value.SetCache(setCacheParam.Key.DbIndex, fullCacheKey, value, setCacheParam.CacheSeconds,
                        setCacheParam.Key.IsDoSerialize);

                    PublishDeleteLocalCacheMessage(fullCacheKey);

                    break;
            }
        }

        public void RemoveCache(CacheKey cacheKey)
        {
            string fullCacheKey = GetFullCacheKeyValue(cacheKey);

            switch (GetCacheType(cacheKey))
            {
                case CacheTypes.LocalMemory:
                    MemoryCacheUtil.RemoveCache(fullCacheKey);

                    break;

                case CacheTypes.CacheServer:
                    _redisService.Value.RemoveCache(cacheKey.DbIndex, fullCacheKey);

                    break;

                case CacheTypes.LocalAndCacheServer:
                    _redisService.Value.RemoveCache(cacheKey.DbIndex, fullCacheKey);
                    MemoryCacheUtil.RemoveCache(fullCacheKey);
                    PublishDeleteLocalCacheMessage(fullCacheKey);

                    break;
            }
        }

        //public long Enqueue<T>(CacheKey cacheKey, T value) => _redisService.Enqueue(GetFullCacheKeyValue(cacheKey), value);

        //public T Dequeue<T>(CacheKey cacheKey) => _redisService.Dequeue<T>(GetFullCacheKeyValue(cacheKey));

        public void DoWorkWithRemoteLock(CacheKey cacheKey, Action work)
        {
            if (work == null)
            {
                return;
            }

            _redisService.Value.DoWorkWithLock(GetFullCacheKeyValue(cacheKey), work);
        }

        public T DoWorkWithRemoteLock<T>(CacheKey cacheKey, Func<T> work)
        {
            if (work == null)
            {
                return default(T);
            }

            return _redisService.Value.DoWorkWithLock(GetFullCacheKeyValue(cacheKey), work);
        }

        private string GetFullCacheKeyValue(CacheKey cacheKey)
        {
            string fullCacheKey = cacheKey.Value;

            if (cacheKey.IsFormatKey)
            {
                fullCacheKey = $"{SharedAppSettings.GetEnvironmentCode().Value}.{cacheKey.Value}";
            }

            return fullCacheKey;
        }

        private static object GetLocker(string key)
        {
            string lockerCacheKey = string.Format(s_lockerKeyFormat, key);
            object locker = null;

            lock (s_cacheWriteLocker)
            {
                locker = MemoryCacheUtil.GetCache(
                    lockerCacheKey,
                    isCloneInstance: false,
                    isForceRefresh: false,
                    cacheSeconds: 300,
                    isSlidingExpiration: true,
                    getCacheData: () =>
                    {
                        return new object();
                    });
            }

            return locker;
        }

        protected virtual CacheTypes GetCacheType(CacheKey cacheKey)
        {
            return cacheKey.CacheType;
        }

        private void PublishDeleteLocalCacheMessage(string fullCacheKey)
        {
            var deleteLocalCacheParam = new DeleteLocalCacheParam()
            {
                MachineName = Environment.MachineName,
                ApplicationValue = s_environmentService.Value.Application.Value,
                FullCacheKey = fullCacheKey
            };

            _redisService.Value.Publish(DbIndexes.Default, s_deleteLocalCacheChannel, deleteLocalCacheParam.ToJsonString());
        }
    }
}