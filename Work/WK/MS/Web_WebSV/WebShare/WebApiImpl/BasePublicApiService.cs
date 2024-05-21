using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using SLPolyGame.Web.Common;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiImpl
{
    public class BasePublicApiService : IPublicApiWebSVService
    {
        private readonly Lazy<ICacheBase> _cacheBase;

        private readonly Lazy<IJxCacheService> _jxCacheService;

        public BasePublicApiService()
        {
            _cacheBase = DependencyUtil.ResolveService<ICacheBase>();
            _jxCacheService = DependencyUtil.ResolveService<IJxCacheService>();
        }

        public async Task<string> GetRemoteCacheForOldService(string key)
        {
            return await Task.FromResult(_cacheBase.Value.GetCache(key));
        }

        public async Task RemoveRemoteCacheForOldService(string key)
        {
            _cacheBase.Value.DeleteCache(key);

            await Task.CompletedTask;
        }

        public async Task SetRemoteCacheForOldService(SetRemoteCacheParam setRemoteCacheParam)
        {
            _cacheBase.Value.AddCache(
                setRemoteCacheParam.Key,
                setRemoteCacheParam.Value,
                setRemoteCacheParam.CacheSeconds,
                setRemoteCacheParam.IsSlidingExpiration);

            await Task.CompletedTask;
        }

        public async Task<bool> IsAllowExecutingActionWithLock(string key, double cacheSeconds)
        {
            CacheKey cacheKey = CacheKey.AllowExecutingActionWithLock(key);
            bool isAllow = _jxCacheService.Value.DoWorkWithRemoteLock(cacheKey, () => IsAllowExecutingActionRemoteOrLocal(key, cacheSeconds, isUseLocalMemory: false).ConfigureAwait(false).GetAwaiter().GetResult());

            return await Task.FromResult(isAllow);
        }

        public async Task<bool> IsAllowExecutingAction(string key, double cacheSeconds)
        {
            return await IsAllowExecutingActionRemoteOrLocal(key, cacheSeconds, isUseLocalMemory: true);
        }

        private async Task<bool> IsAllowExecutingActionRemoteOrLocal(string key, double cacheSeconds, bool isUseLocalMemory)
        {
            CacheKey cacheKey = null;

            if (isUseLocalMemory)
            {
                cacheKey = CacheKey.AllowExecutingActionBySingleServer(key);
            }
            else
            {
                cacheKey = CacheKey.AllowExecutingActionByMultipleServers(key);
            }

            bool isAllowExecuting = false;

            _jxCacheService.Value.GetCache(new SearchCacheParam()
            {
                Key = cacheKey,
                CacheSeconds = cacheSeconds,
            },
            () =>
            {
                isAllowExecuting = true;

                return "1";
            });

            return await Task.FromResult(isAllowExecuting);
        }

        public async Task<Dictionary<string, string>> GetRemoteCacheForOldService(string[] keys)
        {
            return await Task.FromResult(_cacheBase.Value.GetCache(keys));
        }
    }
}