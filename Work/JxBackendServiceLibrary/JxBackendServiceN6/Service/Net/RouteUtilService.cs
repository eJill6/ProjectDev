using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.Enums.Route;
using JxBackendService.Service.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyModel;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace JxBackendServiceN6.Service.Net
{
    public class RouteUtilService : BaseRouteUtilService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IUrlHelper _urlHelper;

        public RouteUtilService()
        {
            _httpContextAccessor = DependencyUtil.ResolveService<IHttpContextAccessor>();
            _urlHelper = DependencyUtil.ResolveService<IUrlHelper>();
        }

        protected override string DoGetRouteValue(string routeKey)
        {
            RouteData routeData = _httpContextAccessor.HttpContext.GetRouteData();

            return routeData.Values[routeKey] as string;
        }

        protected override string GetRouteUrl(RouteName routeName, Dictionary<string, object> routeValues)
        {
            return _urlHelper.RouteUrl(routeName.Value, routeValues);
        }
    }
}