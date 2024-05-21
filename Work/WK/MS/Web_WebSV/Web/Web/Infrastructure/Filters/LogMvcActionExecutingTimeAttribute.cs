using JxBackendService.Attributes;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using Web.Helpers.Security;
using Web.Models.Base;

namespace Web.Infrastructure.Filters
{
    /// <summary>
    /// 繼承BaseController的Action可使用的Attribute
    /// </summary>
    public class LogMvcActionExecutingTimeAttribute : BaseLogActionExecutingTimeNFAttribute
    {
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
                    Application = JxApplication.FrontSideWeb,
                    LoginUser = basicUserInfo
                };

                return environmentUser;
            }
        }
    }
}