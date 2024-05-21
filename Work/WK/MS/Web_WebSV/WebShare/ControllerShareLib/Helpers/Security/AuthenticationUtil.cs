using ControllerShareLib.Interfaces.Service;
using ControllerShareLib.Models.Base;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace ControllerShareLib.Helpers.Security
{
    public static class AuthenticationUtil
    {
        public static string GetUserKey()
        {
            BasicUserInfo basicUserInfo = GetTokenModel();

            return basicUserInfo.UserKey;
        }

        public static int GetUserId()
        {
            BasicUserInfo basicUserInfo = GetTokenModel();

            return basicUserInfo.UserId;
        }

        public static BasicUserInfo GetTokenModel()
        {
            return DependencyUtil.ResolveService<IMiseWebTokenService>().Value.GetTokenModel();
        }

        public static void LogOff()
        {
            var httpContextAccessor = DependencyUtil.ResolveService<IHttpContextAccessor>().Value;

            httpContextAccessor.HttpContext.SignOutAsync().Wait();

            foreach (string key in httpContextAccessor.HttpContext.Request.Cookies.Keys)
            {
                httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
            }

            httpContextAccessor.HttpContext.Session.Clear();
        }

        public static string CreateMiseWebToken(BasicUserInfo basicUserInfo)
        {
            return DependencyUtil.ResolveService<IMiseWebTokenService>().Value.CreateToken(basicUserInfo);
        }

        public static TicketUserData GetLoginUserFromCache()
        {
            ICacheService cacheService = DependencyUtil.ResolveService<ICacheService>().Value;
            string key = string.Format(CacheKeyHelper.UserToken, GetUserKey());

            var ticketUserData = cacheService.GetByRedisRawData<TicketUserData>(key);

            if (ticketUserData == null)
            {
                return new TicketUserData();
            }

            return ticketUserData;
        }

        public static string MWTHeaderName => "mwt";
    }
}