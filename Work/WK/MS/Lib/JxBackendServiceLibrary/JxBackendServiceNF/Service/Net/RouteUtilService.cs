using JxBackendService.Common.Util;
using JxBackendService.Model.Enums.Route;
using JxBackendService.Service.Net;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JxBackendServiceNF.Service.Net
{
    public class RouteUtilService : BaseRouteUtilService
    {
        protected override string DoGetRouteValue(string routeKey)
        {
            return GetRouteValue(HttpContext.Current.Request.RequestContext.RouteData, routeKey);
        }

        protected override string GetRouteUrl(RouteName routeName, Dictionary<string, object> routeValues)
        {
            RouteValueDictionary routeValueDictionary = ConvertToRouteValueDictionary(routeValues);
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            return urlHelper.RouteUrl(routeName.Value, routeValueDictionary);
        }

        private string GetRouteValue(RouteData routeData, string routeKey)
        {
            return routeData.Values[routeKey].ToNonNullString();
        }

        private RouteValueDictionary ConvertToRouteValueDictionary(Dictionary<string, object> routeValues)
        {
            var routeValueDictionary = new RouteValueDictionary();

            foreach (string key in routeValues.Keys)
            {
                routeValueDictionary.Add(key, routeValues[key]);
            }

            return routeValueDictionary;
        }
    }
}