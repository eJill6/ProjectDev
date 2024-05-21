using JxBackendService.Common.Util.Cache;
using System;

namespace JxBackendService.Interface.Service
{
    public interface IJxCacheService
    {
        T GetCache<T>(CacheKey cacheKey, Func<T> getCacheData = null) where T : class;

        T GetCache<T>(CacheKey cacheKey, bool isSliding, Func<T> getCacheData) where T : class;

        T GetCache<T>(SearchCacheParam searchCacheParam) where T : class;

        T GetCache<T>(SearchCacheParam searchCacheParam, Func<T> getCacheData) where T : class;

        void RemoveCache(CacheKey cacheKey);

        void SetCache<T>(SetCacheParam setCacheParam, T value) where T : class;

        //long Enqueue<T>(CacheKey cacheKey, T value);

        //T Dequeue<T>(CacheKey cacheKey);

        void DoWorkWithRemoteLock(CacheKey cacheKey, Action work);

        T DoWorkWithRemoteLock<T>(CacheKey cacheKey, Func<T> work);
    }
}