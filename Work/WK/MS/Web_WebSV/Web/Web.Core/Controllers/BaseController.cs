using ControllerShareLib.Helpers;
using ControllerShareLib.Helpers.Cache;
using ControllerShareLib.Helpers.Security;
using ControllerShareLib.Interfaces.Service;
using ControllerShareLib.Models.Base;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.ThirdParty;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Interface.Service.User;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.User;
using JxBackendService.Model.Exceptions;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SLPolyGame.Web.Model;
using System.Text.Json;
using Web.Infrastructure.Attributes;

namespace Web.Controllers
{
    [MiseWebTokenAuthorize()]
    public class BaseController : Controller
    {
        private readonly Lazy<ICache> _localInstance;

        private static readonly int s_userInfoCacheMinutes = 5;

        private readonly Lazy<IDebugUserService> _debugUserService;

        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        protected JxApplication Application => s_environmentService.Value.Application;

        private readonly Lazy<IHttpContextUserService> _httpContextUserService;

        private readonly Lazy<ICacheService> _cacheService;

        private readonly Lazy<IUserService> _userService;

        private readonly Lazy<IConfigUtilService> _configUtilService;

        private static readonly List<RabbitMQWebSocketSetting> s_rabbitMQWebSocketSettings;

        protected ICacheService CacheService => _cacheService.Value;

        protected IUserService UserService => _userService.Value;

        protected IConfigUtilService ConfigUtilService => _configUtilService.Value;

        static BaseController()
        {
            s_rabbitMQWebSocketSettings = s_rabbitMQWebSocketSettings.GetAssignValueOnce(() =>
            {
                var appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(s_environmentService.Value.Application, SharedAppSettings.PlatformMerchant).Value;

                return appSettingService.GetEndUserRabbitMQWebSocketSettings();
            });
        }

        public BaseController()
        {
            _cacheService = ResolveService<ICacheService>();
            _userService = ResolveService<IUserService>();
            _httpContextUserService = ResolveService<IHttpContextUserService>();
            _debugUserService = ResolveService<IDebugUserService>();
            _configUtilService = ResolveService<IConfigUtilService>();
            _localInstance = ResolveService<ICache>();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewBag.EndUserRabbitMQWebSocketSettings = s_rabbitMQWebSocketSettings;
        }

        protected UserInfo GetUserInfo(bool isForcedRefresh)
        {
            int userId = _httpContextUserService.Value.GetUserId();
            string key = string.Format(CacheKeyHelper.UserInfo, userId);
            UserInfo userInfo;

            if (isForcedRefresh)
            {
                userInfo = UserService.GetUserInfo();
                CacheService.Set(key, userInfo, DateTime.Now.AddMinutes(s_userInfoCacheMinutes));
            }
            else
            {
                //用户信息缓存5分钟
                userInfo = CacheService.Get(key, DateTime.Now.AddMinutes(s_userInfoCacheMinutes),
                   () => UserService.GetUserInfo());
            }

            return userInfo;
        }

        protected TicketUserData GetUserToken() => AuthenticationUtil.GetLoginUserFromCache();

        protected int GetUserId() => AuthenticationUtil.GetUserId();

        protected async Task<UserInfo> GetUserInfoWithoutAvailable(int userId)
        {
            string key = string.Format(CacheKeyHelper.UserInfoWithoutAvailable, userId);
            UserInfo userInfo = await _localInstance.Value.GetOrAddAsync<UserInfo>(nameof(GetUserInfoWithoutAvailable), key, async () =>
            {
                return await UserService.GetUserInfoWithoutAvailable(userId);
            }, DateTime.Now.AddHours(1));

            if (userInfo == null)
            {
                throw new HttpStatusException(System.Net.HttpStatusCode.Unauthorized);
            }

            return userInfo;
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

        protected JsonResult PascalCaseJson(object? data)
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null
            };

            return Json(data, jsonSerializerOptions);
        }

        protected EnvironmentUser EnvLoginUser => new EnvironmentUser()
        {
            Application = Application,
            LoginUser = new BasicUserInfo()
        };

        protected Lazy<T> ResolveService<T>() => DependencyUtil.ResolveService<T>();

        private bool IsFullScreen(LogonMode logonMode)
        {
            if (!logonMode.IsAllowFullScreen)
            {
                return false;
            }

            //允許全屏的情況要判斷是否為偵錯用戶
            bool isDebugger = _debugUserService.Value.IsDebugUser(AuthenticationUtil.GetTokenModel().UserId);

            if (!isDebugger)
            {
                return true; //一般用戶使用全屏
            }

            return ConfigUtilService.Get("IsDebuggerNoFullScreen", "0") != "1";
        }
    }
}