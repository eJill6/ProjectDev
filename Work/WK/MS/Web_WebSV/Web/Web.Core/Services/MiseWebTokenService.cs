using ControllerShareLib.Services;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Net;

namespace Web.Core.Services
{
    public class MiseWebTokenService : BaseMiseWebTokenService
    {
        protected override string GetMiseWebToken()
        {
            var routeUtilService = DependencyUtil.ResolveService<IRouteUtilService>().Value;
            string token = routeUtilService.GetMiseWebTokenName();

            return token;
        }
    }
}