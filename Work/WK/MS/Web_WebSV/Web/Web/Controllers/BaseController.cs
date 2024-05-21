using System;
using System.Web.Mvc;
using Web.Infrastructure.Attributes;
using Web.Extensions;
using Web.Helpers;
using Web.Models.Base;
using Web.Helpers.Security;
using Web.Services;
using JxBackendService.Common.Util;
using Web.Infrastructure.Filters;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.User;
using JxBackendService.Common.Util.ThirdParty;
using JxBackendService.Model.Enums.User;
using JxBackendService.Interface.Service.Config;
using SLPolyGame.Web.Model;

namespace Web.Controllers
{
    [ExceptionFilter(Order = 1)]
    //[WebAuthorize(Order = 2)]
    [MiseWebTokenAuthorize(Order = 2)]
    public class BaseController : Controller
    {
        private readonly IDebugUserService _debugUserService;

        private readonly IConfigUtilService _configUtilService;

        protected readonly ICacheService _cacheService = null;

        protected readonly IUserService _userService = null;

        public BaseController(ICacheService cacheService,
            IUserService userService)
        {
            _cacheService = cacheService;
            _userService = userService;
            _debugUserService = DependencyUtil.ResolveService<IDebugUserService>();
            _configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.StompServiceUrl = GlobalCacheHelper.StompServiceUrl;
            var cookies = filterContext.HttpContext.Request.Cookies;

            if (cookies[CookieKeyHelper.UniqueID] != null)
            {
                ViewBag.UniqueID = cookies[CookieKeyHelper.UniqueID].Value;
            }

            ViewBag.StompServiceUrl = GlobalCacheHelper.StompServiceUrl;
        }

        protected UserInfo GetUserInfo(bool isForcedRefresh)
        {
            string userName = this.GetUserName();
            string key = string.Format(CacheKeyHelper.UserInfo, userName);

            //用户信息缓存5分钟
            UserInfo userInfo = _cacheService.Get(key, DateTime.Now.AddMinutes(5),
                () => _userService.GetUserInfo());

            if (userInfo == null || isForcedRefresh)
            {
                userInfo = _userService.GetUserInfo();
                _cacheService.Set(key, userInfo, DateTime.Now.AddSeconds(5));
            }

            return userInfo;
        }

        protected TicketUserData GetUserToken() => AuthenticationUtil.GetLoginUserFromCache();

        protected bool IsValidRequired(params object[] list)
        {
            bool isValid = true;

            if (list != null)
            {
                foreach (object obj in list)
                {
                    if (obj is string)
                    {
                        if (string.IsNullOrEmpty(obj.ToTrimString()))
                        {
                            isValid = false;
                            break;
                        }
                    }
                    else
                    {
                        if (obj == null)
                        {
                            isValid = false;
                            break;
                        }
                    }
                }
            }

            return isValid;
        }

        protected void SetTicketUserDataToViewBag()
        {
            ViewBag.TicketUserData = AuthenticationUtil.GetLoginUserFromCache();
        }

        protected string ToFullScreenUrlByDebugSetting(string url, bool isHideHeaderWithFullScreen, string title, LogonMode logonMode)
        {
            if (!IsFullScreen(logonMode))
            {
                return url;
            }

            return MiseLiveWebUtil.ConvertToFullScreenUrl(url, isHideHeaderWithFullScreen, title);
        }

        private bool IsFullScreen(LogonMode logonMode)
        {
            if (!logonMode.IsAllowFullScreen)
            {
                return false;
            }

            //允許全屏的情況要判斷是否為偵錯用戶
            bool isDebugger = _debugUserService.IsDebugUser(AuthenticationUtil.GetTokenModel().UserId);

            if (!isDebugger)
            {
                return true; //一般用戶使用全屏
            }

            return _configUtilService.Get("IsDebuggerNoFullScreen", "0") != "1";
        }
    }
}