using ControllerShareLib.Helpers.Security;
using ControllerShareLib.Services;
using JxBackendService.DependencyInjection;

namespace M.Core.Services
{
    public class MiseWebTokenService : BaseMiseWebTokenService
    {
        

        protected override string GetMiseWebToken()
        {
            IHttpContextAccessor httpContextAccessor = DependencyUtil.ResolveService<IHttpContextAccessor>().Value;

            IHeaderDictionary headers = httpContextAccessor.HttpContext.Request.Headers;

            if (headers.ContainsKey(AuthenticationUtil.MWTHeaderName))
            {
                return headers[AuthenticationUtil.MWTHeaderName].ToString();
            }

            return string.Empty;
        }
    }
}