using JxBackendService.Attributes.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SLPolyGame.Web.Common;
using SLPolyGame.Web.Model;
using System.Security.Claims;
using JxBackendService.Model.Enums.Net;

namespace SLPolyGame.Web.Core.Filters
{
    public class WebSVApiAuthorize : Attribute, IAuthorizationFilter
    {
        private readonly Lazy<IJxCacheService> _jxCacheService;

        public WebSVApiAuthorize()
        {
            _jxCacheService = DependencyUtil.ResolveService<IJxCacheService>();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HasAllowAnonymous())
            {
                return;
            }

            string userId = context.HttpContext.Request.Headers["p1"];
            string userKey = context.HttpContext.Request.Headers["p2"];
            string fromApplication = context.HttpContext.Request.Headers["FromApplication"];
            JxApplication jxApplication = JxApplication.FrontSideWeb;

            if (!fromApplication.IsNullOrEmpty())
            {
                jxApplication = JxApplication.GetSingle(fromApplication);
            }

            UserInfoToken userInfoToken = GetUserInfoToken(jxApplication, userId, userKey);

            if (userInfoToken == null)
            {
                context.Result = new UnauthorizedResult();

                return;
            }

            context.HttpContext.SetItemValue(HttpContextItemKey.UserInfoToken, userInfoToken);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userKey),
                new Claim(ClaimTypes.NameIdentifier, userInfoToken.UserId.ToString()),
                new Claim(ClaimTypes.GivenName, string.Empty)
            };

            var identity = new ClaimsIdentity(claims, jxApplication.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            context.HttpContext.User = principal;
        }

        private UserInfoToken GetUserInfoToken(JxApplication jxApplication, string userId, string userKey)
        {
            if (userId.IsNullOrEmpty() || userKey.IsNullOrEmpty())
            {
                return null;
            }

            CacheObj cacheObj = _jxCacheService.Value.GetCache<CacheObj>(
                new SearchCacheParam()
                {
                    Key = CacheKey.GetFrontSideUserInfoKey(userKey),
                    CacheSeconds = jxApplication.UserKeyExpiredMinutes * 60,
                    IsSlidingExpiration = jxApplication.IsSlidingUserKeyCache,
                }, getCacheData: null);

            if (cacheObj == null)
            {
                return null;
            }

            var userInfoToken = cacheObj.Value.Deserialize<UserInfoToken>();

            if (userInfoToken != null && userInfoToken.UserId != userId.ToInt32())
            {
                return null;
            }

            return userInfoToken;
        }
    }
}