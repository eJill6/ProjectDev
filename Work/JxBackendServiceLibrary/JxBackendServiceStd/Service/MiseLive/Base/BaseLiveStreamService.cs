using JxBackendService.Common.Util.Cache;
using JxBackendService.Interface.Model.MiseLive.ViewModel;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.MiseLive;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty.MiseLive;
using JxBackendService.Service.Base;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.MiseLive.Base
{
    public abstract class BaseLiveStreamService : BaseService, ITPLiveStreamService
    {
        private static readonly int s__anchorsTempLocalMemoryCacheSeconds = 3;

        private readonly IJxCacheService _jxCacheService;

        protected BaseLiveStreamService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _jxCacheService = ResolveServiceForModel<IJxCacheService>(EnvLoginUser.Application);
        }

        protected abstract PlatformProduct Product { get; }

        protected IJxCacheService JxCacheService => _jxCacheService;

        public List<IMiseLiveAnchor> GetAnchors()
        {
            var searchCacheParam = new SearchCacheParam()
            {
                Key = CacheKey.TPAnchors(Product),
                CacheSeconds = int.MaxValue,
                TempLocalMemoryCacheSeconds = s__anchorsTempLocalMemoryCacheSeconds,
                IsCloneInstance = false,
            };

            var anchors = JxCacheService.GetCache<List<MiseLiveAnchor>>(searchCacheParam);

            if (anchors == null)
            {
                anchors = new List<MiseLiveAnchor>();
            }

            return anchors.Select(s => s as IMiseLiveAnchor).ToList();
        }

        public BaseReturnModel CrawlAnchors()
        {
            List<IMiseLiveAnchor> miseLiveAnchors = GetRemoteAnchors();

            _jxCacheService.SetCache(
                new SetCacheParam()
                {
                    Key = CacheKey.TPAnchors(Product),
                    CacheSeconds = int.MaxValue,
                    TempLocalMemoryCacheSeconds = s__anchorsTempLocalMemoryCacheSeconds
                },
                miseLiveAnchors);

            return new BaseReturnModel(ReturnCode.Success);
        }

        protected abstract List<IMiseLiveAnchor> GetRemoteAnchors();
    }
}