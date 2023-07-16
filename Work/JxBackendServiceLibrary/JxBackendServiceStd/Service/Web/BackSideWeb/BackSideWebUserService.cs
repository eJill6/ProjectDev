using JxBackendService.Common.Util;
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace JxBackendServiceN6.Service.Web.BackSideWeb
{
    public class BackSideWebUserService : HttpContextUserService, IBackSideWebUserService
    {
        private static readonly JxApplication s_application = JxApplication.BackSideWeb;

        private readonly IJxCacheService _jxCacheService;

        private readonly IPermissionKeyDetailService _permissionKeyDetailService;

        public BackSideWebUserService()
        {
            _jxCacheService = DependencyUtil.ResolveServiceForModel<IJxCacheService>(s_application);
            _permissionKeyDetailService = DependencyUtil.ResolveService<IPermissionKeyDetailService>();
        }

        public bool HasPermission(PermissionKeys permissionKey, AuthorityTypes authorityType)
        {
            BackSideWebUser backSideWebUser = GetUser();

            if (backSideWebUser == null)
            {
                return false;
            }

            var permissionKeyDetail = _permissionKeyDetailService.GetSingle(permissionKey);

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

        public BackSideWebUser GetUser() => GetUser(GetUserKey());

        public BackSideWebUser GetUser(string userKey)
        {
            if (userKey.IsNullOrEmpty())
            {
                return new BackSideWebUser();
            }

            if (HttpContextAccessor.HttpContext.Items[HttpContextItemKey.BackSideWebUser.Value] != null)
            {
                return HttpContextAccessor.HttpContext.Items[HttpContextItemKey.BackSideWebUser] as BackSideWebUser;
            }

            CacheKey cacheKey = CacheKey.BackSideUser(userKey);

            var searchCacheParam = new SearchCacheParam()
            {
                Key = cacheKey,
                CacheSeconds = s_application.UserKeyExpiredMinutes * 60,
                IsCloneInstance = false,
                IsSlidingExpiration = true,
            };

            var backSideWebUser = _jxCacheService.GetCache<BackSideWebUser>(searchCacheParam, getCacheData: null);

            if (backSideWebUser == null)
            {
                return new BackSideWebUser();
            }

            HttpContextAccessor.HttpContext.Items[HttpContextItemKey.BackSideWebUser] = backSideWebUser;

            return backSideWebUser;
        }

        public RedirectToActionResult Logout(Action doHttpSignOut)
        {
            if (HttpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                string userKey = GetUserKey();
                _jxCacheService.RemoveCache(CacheKey.BackSideUser(userKey));
                doHttpSignOut.Invoke();
            }

            return new RedirectToActionResult("Login", "Authority", routeValues: null);
        }

        public Dictionary<MenuType, List<PermissionKeyDetail>> GetUserMenuMap(string userKey)
        {
            return _jxCacheService.GetCache(
                new SearchCacheParam()
                {
                    Key = CacheKey.BackSideUserMenu(userKey),
                    IsCloneInstance = false,
                    IsSlidingExpiration = true,
                    CacheSeconds = JxApplication.BackSideWeb.UserKeyExpiredMinutes * 60,
                },
                () =>
                {
                    BackSideWebUser user = GetUser(userKey);

                    Dictionary<string, HashSet<int>> userReadPermissionMaps = user.PermissionMap
                        .Where(w => w.Value.Contains(AuthorityTypeDetail.Read.Value))
                        .ToDictionary(d => d.Key, d => d.Value);

                    List<PermissionKeyDetail> permissionKeyDetails = userReadPermissionMaps.Keys
                        .Select(s => _permissionKeyDetailService.GetSingle(s))
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

        public void SetLoginCache(string userKey, BackSideWebUser backSideWebUser)
        {
            var setCacheParam = new SetCacheParam()
            {
                Key = CacheKey.BackSideUser(userKey),
                CacheSeconds = s_application.UserKeyExpiredMinutes * 60,
                IsSlidingExpiration = true,
            };

            _jxCacheService.SetCache(setCacheParam, backSideWebUser);
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