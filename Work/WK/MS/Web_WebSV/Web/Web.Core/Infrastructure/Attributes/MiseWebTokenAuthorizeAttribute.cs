using ControllerShareLib.Helpers.Security;
using ControllerShareLib.Interfaces.Service;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.ViewModel;

namespace Web.Infrastructure.Attributes
{
    public class MiseWebTokenAuthorizeAttribute : WebAuthorizeAttribute
    {
        private readonly Lazy<IMiseWebTokenService> _miseWebTokenService;

        public MiseWebTokenAuthorizeAttribute()
        {
            _miseWebTokenService = DependencyUtil.ResolveService<IMiseWebTokenService>();
        }

        protected override bool DoAuthorizeJob(HttpContext httpContext)
        {
            BasicUserInfo basicUserInfo = AuthenticationUtil.GetTokenModel();

            if (basicUserInfo.UserKey.IsNullOrEmpty())
            {
                return false;
            }

            if (!_miseWebTokenService.Value.IsCacheTokenValid(basicUserInfo))
            {
                return false;
            }

            _miseWebTokenService.Value.AddHttpContextUser(httpContext, basicUserInfo);

            return true;
        }
    }
}