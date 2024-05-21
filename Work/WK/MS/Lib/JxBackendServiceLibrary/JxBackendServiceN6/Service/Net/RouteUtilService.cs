using JxBackendService.DependencyInjection;
using JxBackendService.Model.Enums.Route;
using JxBackendService.Service.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace JxBackendServiceN6.Service.Net
{
    public class RouteUtilService : BaseRouteUtilService
    {
        private readonly Lazy<IHttpContextAccessor> _httpContextAccessor;

        private readonly Lazy<IUrlHelper> _urlHelper;

        public RouteUtilService()
        {
            _httpContextAccessor = DependencyUtil.ResolveService<IHttpContextAccessor>();
            _urlHelper = DependencyUtil.ResolveService<IUrlHelper>();
        }

        protected override string DoGetRouteValue(string routeKey)
        {
            RouteData routeData = _httpContextAccessor.Value.HttpContext.GetRouteData();

            return routeData.Values[routeKey] as string;
        }

        protected override string GetRouteUrl(RouteName routeName, Dictionary<string, object> routeValues)
        {
            return _urlHelper.Value.RouteUrl(routeName.Value, routeValues);
        }
    }
}