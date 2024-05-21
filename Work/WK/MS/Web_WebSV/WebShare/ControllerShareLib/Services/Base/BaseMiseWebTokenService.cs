using ControllerShareLib.Helpers;
using ControllerShareLib.Interfaces.Service;
using JxBackendService.Common;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Net;
using JxBackendService.Model.ViewModel;
using Microsoft.AspNetCore.Http;
using SLPolyGame.Web.Model;
using System.Security.Claims;

namespace ControllerShareLib.Services
{
    public abstract class BaseMiseWebTokenService : IMiseWebTokenService
    {
        private readonly Lazy<IConfigUtilService> _configUtilService;

        private readonly Lazy<IEnvironmentService> _environmentService;

        private readonly Lazy<IJxCacheService> _jxCacheService;

        public BaseMiseWebTokenService()
        {
            _configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();
            _environmentService = DependencyUtil.ResolveService<IEnvironmentService>();
            _jxCacheService = DependencyUtil.ResolveService<IJxCacheService>();
        }

        protected abstract string GetMiseWebToken();

        public string CreateToken(BasicUserInfo basicUserInfo)
        {
            DESTool tool = new DESTool(GetAuthenticationTokenKey());
            string token = tool.DESEnCode(basicUserInfo.ToJsonString(isFormattingNone: true));

            return token;
        }

        public BasicUserInfo GetTokenModel()
        {
            string token = GetMiseWebToken();

            if (token.IsNullOrEmpty())
            {
                return new BasicUserInfo();
            }

            try
            {
                //加入快取降低解密的成本
                string cacheKey = CacheKeyHelper.RouteToken(token);

                BasicUserInfo basicUserInfo = MemoryCacheUtil.GetCache(cacheKey,
                    isCloneInstance: false,
                    isForceRefresh: false,
                    cacheSeconds: 60 * 60,
                    isSlidingExpiration: true,
                    getCacheData: () =>
                    {
                        var tool = new DESTool(GetAuthenticationTokenKey());

                        return tool.DESDeCode(token).Deserialize<BasicUserInfo>();
                    });

                return basicUserInfo;
            }
            catch (Exception ex)
            {
                DependencyUtil.ResolveService<ILogUtilService>().Value.Error(ex);

                return new BasicUserInfo();
            }
        }

        public bool IsCacheTokenValid(BasicUserInfo basicUserInfo)
        {
            //防止併發下狂打redis
            CacheKey userInfoCacheKey = CacheKey.GetFrontSideUserInfoKey(basicUserInfo.UserKey);
            JxApplication application = _environmentService.Value.Application;
            double cacheSeconds = application.UserKeyExpiredMinutes * 60;
            int? tempLocalMemoryCacheSeconds;

            if (application.IsSlidingUserKeyCache)
            {
                tempLocalMemoryCacheSeconds = Convert.ToInt32(cacheSeconds * 0.5);
            }
            else if (!basicUserInfo.Ts.HasValue)
            {
                tempLocalMemoryCacheSeconds = 60 * 10;//舊資料, 無法計算還剩多久，固定先塞10分鐘
            }
            else
            {
                //直接計算是否過期
                decimal remainSeconds = (decimal)cacheSeconds - ((DateTime.UtcNow.ToUnixOfTime() - basicUserInfo.Ts.Value) / 1000m);

                return remainSeconds > 0;
            }

            var cacheObj = _jxCacheService.Value.GetCache<CacheObj>(
                new SearchCacheParam()
                {
                    Key = userInfoCacheKey,
                    CacheSeconds = cacheSeconds,
                    TempLocalMemoryCacheSeconds = tempLocalMemoryCacheSeconds,
                    IsSlidingExpiration = application.IsSlidingUserKeyCache,
                    IsCloneInstance = false,
                },
                getCacheData: null);

            UserInfoToken? userInfoToken = null;

            if (cacheObj != null && !cacheObj.Value.IsNullOrEmpty())
            {
                userInfoToken = cacheObj.Value.Deserialize<UserInfoToken>();
            }

            if (userInfoToken == null || (userInfoToken != null && userInfoToken.UserId != basicUserInfo.UserId))
            {
                return false;
            }

            return true;
        }

        public void AddHttpContextUser(HttpContext httpContext, BasicUserInfo basicUserInfo)
        {
            httpContext.SetItemValue(HttpContextItemKey.UserInfoToken, basicUserInfo);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, basicUserInfo.UserKey),
                new Claim(ClaimTypes.NameIdentifier, basicUserInfo.UserId.ToString()),
            };

            var identity = new ClaimsIdentity(claims, _environmentService.Value.Application.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            httpContext.User = principal;
        }

        private string GetAuthenticationTokenKey()
        {
            return _configUtilService.Value.Get("AuthenticationTokenKey");
        }
    }
}