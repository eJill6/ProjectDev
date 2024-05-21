using BackSideWeb.Filters;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Web.BackSideWeb;
using JxBackendService.Model.Common;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ViewModel;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BackSideWeb.Controllers.Base
{
    [CustomizedAuthorize]
    [CheckUserPasswordExpiration]
    public class BaseAuthController : BaseController
    {
        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        private static readonly List<RabbitMQWebSocketSetting> s_rabbitMQWebSocketSettings;

        private EnvironmentUser _envLoginUser;

        private readonly Lazy<IJxCacheService> _jxCacheService;

        protected IJxCacheService JxCacheService => _jxCacheService.Value;

        private readonly Lazy<IBackSideWebUserService> _backSideWebLoginUserService;

        protected override EnvironmentUser EnvLoginUser
        {
            get
            {
                _envLoginUser = AssignValueOnceUtil.GetAssignValueOnce(_envLoginUser, () =>
                {
                    BackSideWebUser backSideWebUser = _backSideWebLoginUserService.Value.GetUser();

                    return new EnvironmentUser()
                    {
                        Application = Application,
                        LoginUser = backSideWebUser
                    };
                });

                return _envLoginUser;
            }
        }

        static BaseAuthController()
        {
            var appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(s_environmentService.Value.Application, SharedAppSettings.PlatformMerchant).Value;
            s_rabbitMQWebSocketSettings = appSettingService.GetEndUserRabbitMQWebSocketSettings();
        }

        public BaseAuthController()
        {
            _jxCacheService = DependencyUtil.ResolveService<IJxCacheService>();
            _backSideWebLoginUserService = DependencyUtil.ResolveService<IBackSideWebUserService>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            BackSideWebUser backSideWebUser = EnvLoginUser.LoginUser as BackSideWebUser;

            ViewBag.BackSideWebUser = backSideWebUser;
            ViewBag.UserMenuMap = _backSideWebLoginUserService.Value.GetUserMenuMap(backSideWebUser.UserId);
            ViewBag.EndUserRabbitMQWebSocketSettings = s_rabbitMQWebSocketSettings;
        }
    }
}