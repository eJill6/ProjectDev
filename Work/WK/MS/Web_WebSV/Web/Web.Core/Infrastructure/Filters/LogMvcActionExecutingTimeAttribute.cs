using ControllerShareLib.Helpers.Security;
using JxBackendService.Attributes;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;

namespace Web.Infrastructure.Filters
{
    /// <summary>
    /// 繼承BaseController的Action可使用的Attribute
    /// </summary>
    public class LogMvcActionExecutingTimeAttribute : BaseLogActionExecutingTimeAttribute
    {
        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        public LogMvcActionExecutingTimeAttribute()
        {
        }

        public LogMvcActionExecutingTimeAttribute(double warningMilliseconds) : base(warningMilliseconds)
        {
        }

        protected override EnvironmentUser EnvLoginUser
        {
            get
            {
                BasicUserInfo basicUserInfo = AuthenticationUtil.GetTokenModel();

                var environmentUser = new EnvironmentUser()
                {
                    Application = s_environmentService.Value.Application,
                    LoginUser = basicUserInfo
                };

                return environmentUser;
            }
        }
    }
}