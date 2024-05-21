using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.MiseLive.ViewModel;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.MiseLive;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty.MiseLive;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JxBackendService.Service.MiseLive.Base
{
    public abstract class BaseLiveStreamService : BaseService, ITPLiveStreamService
    {
        private static readonly int s_syncTempLocalMemoryCacheSeconds = 5;

        private readonly Lazy<IJxCacheService> _jxCacheService;

        protected BaseLiveStreamService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _jxCacheService = DependencyUtil.ResolveService<IJxCacheService>();
        }

        protected abstract PlatformProduct Product { get; }

        protected IJxCacheService JxCacheService => _jxCacheService.Value;

        public List<IMiseLiveAnchor> GetAnchors()
        {
            //取得直播列表，吃內存，另外有Task做定時同步
            var searchCacheParam = new SearchCacheParam()
            {
                Key = CacheKey.DunplicateTPAnchors(Product),
                CacheSeconds = int.MaxValue,
                IsCloneInstance = false,
            };

            var anchors = JxCacheService.GetCache<List<MiseLiveAnchor>>(searchCacheParam);

            if (anchors == null)
            {
                anchors = new List<MiseLiveAnchor>();
            }

            return anchors.Select(s => s as IMiseLiveAnchor).ToList();
        }

        /// <summary>同步遠端快取回內存</summary>
        public void StartNewSyncCachedAnchorsJob()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    ErrorMsgUtil.DoWorkWithErrorHandle(EnvLoginUser, () =>
                    {
                        var searchCacheParam = new SearchCacheParam()
                        {
                            Key = CacheKey.TPAnchors(Product),
                            IsCloneInstance = false,
                        };

                        var anchors = JxCacheService.GetCache<List<MiseLiveAnchor>>(searchCacheParam);

                        if (anchors != null)
                        {
                            var setCacheParam = new SetCacheParam()
                            {
                                Key = CacheKey.DunplicateTPAnchors(Product),
                                CacheSeconds = int.MaxValue,
                                IsSlidingExpiration = false,
                            };

                            JxCacheService.SetCache(setCacheParam, anchors);
                        }
                    });

                    TaskUtil.DelayAndWait(s_syncTempLocalMemoryCacheSeconds * 1000);
                }
            });
        }

        public BaseReturnModel CrawlAnchors()
        {
            List<IMiseLiveAnchor> miseLiveAnchors = GetRemoteAnchors();

            _jxCacheService.Value.SetCache(
                new SetCacheParam()
                {
                    Key = CacheKey.TPAnchors(Product),
                    CacheSeconds = int.MaxValue,
                    TempLocalMemoryCacheSeconds = s_syncTempLocalMemoryCacheSeconds
                },
                miseLiveAnchors);

            return new BaseReturnModel(ReturnCode.Success);
        }

        protected abstract List<IMiseLiveAnchor> GetRemoteAnchors();
    }
}