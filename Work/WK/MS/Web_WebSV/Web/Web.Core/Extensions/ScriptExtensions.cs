using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Net;
using Microsoft.AspNetCore.Mvc;

namespace Web.Extensions
{
    public static class ScriptExtensions
    {
        public static string GetMiseWebTokenUrl(this IUrlHelper urlHelper, string action, string controller, object routeParams = null)
        {
            return DependencyUtil.ResolveService<IRouteUtilService>().Value.GetMiseWebTokenUrl(action, controller, routeParams);
        }
    }
}