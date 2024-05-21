using System;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.MessageQueue;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Service.DelayJob
{
    public class CacheDelayJobService : BaseDelayJobService<DelaySetCacheParam>, ICacheDelayJobService
    {
        private readonly Lazy<IJxCacheService> _jxCacheService;

        public CacheDelayJobService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _jxCacheService = DependencyUtil.ResolveService<IJxCacheService>();
        }

        protected override void DoJob(DelaySetCacheParam param)
        {
            if (param.CacheValue == null)
            {
                _jxCacheService.Value.RemoveCache(param.SetCacheParam.Key);
            }
            else
            {
                _jxCacheService.Value.SetCache(param.SetCacheParam, param.CacheValue);
            }
        }

        public void AddDeleteDelayJobParam(CacheKey cacheKey, int delaySeconds)
        {
            var param = new DelaySetCacheParam()
            {
                SetCacheParam = new SetCacheParam()
                {
                    Key = cacheKey
                }
            };

            AddDelayJobParam(param, delaySeconds);
        }
    }
}