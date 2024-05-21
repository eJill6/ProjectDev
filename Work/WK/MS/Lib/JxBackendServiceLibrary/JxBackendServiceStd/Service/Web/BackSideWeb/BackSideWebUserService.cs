using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Interface.Service.Web.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Enums.Net;
using JxBackendService.Model.Param.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace JxBackendServiceN6.Service.Web.BackSideWeb
{
    public class BackSideWebUserService : HttpContextUserService, IBackSideWebUserService
    {
        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        private static readonly JxApplication s_application = s_environmentService.Value.Application;

        private readonly Lazy<IJxCacheService> _jxCacheService;

        private readonly Lazy<IPermissionKeyDetailService> _permissionKeyDetailService;

        public BackSideWebUserService()
        {
            _jxCacheService = DependencyUtil.ResolveService<IJxCacheService>();
            _permissionKeyDetailService = DependencyUtil.ResolveService<IPermissionKeyDetailService>();
        }

        public bool HasPermission(PermissionKeys permissionKey, AuthorityTypes authorityType)
        {
            BackSideWebUser backSideWebUser = GetUser();

            if (backSideWebUser == null)
            {
                return false;
            }

            var permissionKeyDetail = _permissionKeyDetailService.Value.GetSingle(permissionKey);

            if (!backSideWebUser.PermissionMap.TryGetValue(permissionKeyDetail.Value, out HashSet<int> authorityTypeSet))
            {
                return false;
            }

            //判斷Authority
            if (!authorityTypeSet.Contains((int)authorityType))
            {
                return false;
            }

            return true;
        }

        public BackSideWebUser GetUser() => GetUser(GetUserId());

        public BackSideWebUser GetUser(int userId)
        {
            if (userId == 0)
            {
                return new BackSideWebUser();
            }

            IHttpContextAccessor httpContextAccessor = HttpContextAccessor;

            if (httpContextAccessor.HttpContext.Items[HttpContextItemKey.BackSideWebUser] != null)
            {
                return httpContextAccessor.HttpContext.Items[HttpContextItemKey.BackSideWebUser] as BackSideWebUser;
            }

            CacheKey cacheKey = CacheKey.BackSideUser(userId);

            var searchCacheParam = new SearchCacheParam()
            {
                Key = cacheKey,
                CacheSeconds = s_application.UserKeyExpiredMinutes * 60,
                IsCloneInstance = false,
                IsSlidingExpiration = true,
            };

            var backSideWebUser = _jxCacheService.Value.GetCache<BackSideWebUser>(searchCacheParam, getCacheData: null);

            if (backSideWebUser == null) //加上此判斷可達到後登入者把前登入者失效 || backSideWebUser.UserKey != HttpContextAccessor.HttpContext.User.Identity.Name
            {
                return new BackSideWebUser();
            }

            httpContextAccessor.HttpContext.Items[HttpContextItemKey.BackSideWebUser] = backSideWebUser;

            return backSideWebUser;
        }

        public RedirectToActionResult Logout(Action doHttpSignOut)
        {
            IHttpContextAccessor httpContextAccessor = HttpContextAccessor;

            if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                int userId = GetUserId();
                _jxCacheService.Value.RemoveCache(CacheKey.BackSideUser(userId));
                _jxCacheService.Value.RemoveCache(CacheKey.CheckUserPasswordExpiration(userId));
                doHttpSignOut.Invoke();
            }

            return new RedirectToActionResult("Login", "Authority", routeValues: null);
        }

        public Dictionary<MenuType, List<PermissionKeyDetail>> GetUserMenuMap(int userId)
        {
            return _jxCacheService.Value.GetCache(
                new SearchCacheParam()
                {
                    Key = CacheKey.BackSideUserMenu(userId),
                    IsCloneInstance = false,
                    IsSlidingExpiration = true,
                    CacheSeconds = s_application.UserKeyExpiredMinutes * 60,
                },
                () =>
                {
                    BackSideWebUser user = GetUser(userId);

                    Dictionary<string, HashSet<int>> userReadPermissionMaps = user.PermissionMap
                        .Where(w => w.Value.Contains(AuthorityTypeDetail.Read.Value))
                        .ToDictionary(d => d.Key, d => d.Value);

                    List<PermissionKeyDetail> permissionKeyDetails = userReadPermissionMaps.Keys
                        .Select(s => _permissionKeyDetailService.Value.GetSingle(s))
                        .Where(w => w != null)
                        .ToList();

                    var menuMap = new Dictionary<MenuType, List<PermissionKeyDetail>>();

                    foreach (MenuType menuType in permissionKeyDetails.Select(s => s.MenuType).Distinct().OrderBy(o => o.Sort))
                    {
                        menuMap.Add(menuType, permissionKeyDetails.Where(w => w.MenuType == menuType).OrderBy(o => o.Sort).ToList());
                    }

                    return menuMap;
                });
        }

        public void SetLoginCache(BackSideWebUser backSideWebUser)
        {
            var setCacheParam = new SetCacheParam()
            {
                Key = CacheKey.BackSideUser(backSideWebUser.UserId),
                CacheSeconds = s_application.UserKeyExpiredMinutes * 60,
                IsSlidingExpiration = true,
            };

            _jxCacheService.Value.SetCache(setCacheParam, backSideWebUser);
        }

        public void SignIn(BackSideWebUser backSideWebUser, Action<ClaimsPrincipal, AuthenticationProperties> doHttpSignIn)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, backSideWebUser.UserKey),
                new Claim(ClaimTypes.NameIdentifier, backSideWebUser.UserId.ToString()),
                new Claim(ClaimTypes.GivenName, backSideWebUser.UserName),
            };

            var identity = new ClaimsIdentity(claims, s_application.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            doHttpSignIn.Invoke(
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(s_application.UserKeyExpiredMinutes)
                });
        }
    }
}