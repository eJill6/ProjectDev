using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using RestSharp;
using SLPolyGame.Web.Model;
using System;

namespace WebApiImpl
{
    public abstract class BaseWebApiService
    {
        private readonly Lazy<ILogUtilService> _logUtilService;

        private readonly Lazy<IHttpContextUserService> _httpContextUserService;

        private readonly Lazy<IJxCacheService> _jxCacheService;

        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        protected ILogUtilService LogUtilService => _logUtilService.Value;

        protected IJxCacheService JxCacheService => _jxCacheService.Value;

        protected JxApplication Application => s_environmentService.Value.Application;

        protected EnvironmentUser EnvLoginUser
        {
            get
            {
                BasicUserInfo basicUserInfo = _httpContextUserService.Value.GetBasicUserInfo();

                if (basicUserInfo == null)
                {
                    basicUserInfo = new BasicUserInfo();
                }

                return new EnvironmentUser()
                {
                    Application = Application,
                    LoginUser = basicUserInfo
                };
            }
        }

        public BaseWebApiService()
        {
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
            _httpContextUserService = DependencyUtil.ResolveService<IHttpContextUserService>();
            _jxCacheService = DependencyUtil.ResolveService<IJxCacheService>();
        }

        protected UserInfoToken GetUserInfoToken()
        {
            CacheObj cacheObj = JxCacheService.GetCache<CacheObj>(CacheKey.GetFrontSideUserInfoKey(EnvLoginUser.LoginUser.UserKey), getCacheData: null);

            if (cacheObj == null)
            {
                return null;
            }

            return cacheObj.Value.Deserialize<UserInfoToken>();
        }
    }
}