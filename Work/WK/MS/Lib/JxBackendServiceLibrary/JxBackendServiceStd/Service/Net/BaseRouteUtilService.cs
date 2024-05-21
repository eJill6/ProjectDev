using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Route;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Enums.Route;
using System.Collections.Generic;

namespace JxBackendService.Service.Net
{
    public abstract class BaseRouteUtilService : IRouteUtilService
    {
        protected abstract string DoGetRouteValue(string routeKey);

        protected abstract string GetRouteUrl(RouteName routeName, Dictionary<string, object> routeValues);

        public string GetControllerName()
        {
            return GetRouteValue(RouteUtil.RouteControllerName);
        }

        public string GetActionName()
        {
            return GetRouteValue(RouteUtil.RouteActionName);
        }

        public string GetMiseWebTokenName()
        {
            return GetRouteValue(RouteUtil.RouteMiseWebTokenName);
        }

        public string GetRouteUrl(RouteName routeName, string action)
        {
            return GetRouteUrl(routeName, action, null, null);
        }

        public string GetRouteUrl(RouteName routeName, string action, string controller)
        {
            return GetRouteUrl(routeName, action, controller, null);
        }

        public string GetRouteUrl(string action, string controller, object routeParams)
        {
            return GetRouteUrl(RouteName.Default, action, controller, routeParams);
        }

        public string GetMiseWebTokenUrl(string action, string controller, object routeParams)
        {
            Dictionary<string, object> routeValues = RouteUtil.ConvertToRouteValues(routeParams);
            string miseWebToken = GetMiseWebTokenName();

            if (!miseWebToken.IsNullOrEmpty())
            {
                routeValues.Add(RouteUtil.RouteMiseWebTokenName, miseWebToken);
            }

            return GetRouteUrl(RouteName.MiseWebToken, action, controller, routeParams);
        }

        public string GetRouteUrl(RouteName routeName, string action, string controller, object routeParams)
        {
            Dictionary<string, object> routeValues = RouteUtil.ConvertToRouteValues(routeParams);

            if (!routeValues.ContainsKey(RouteUtil.RouteActionName))
            {
                if (action.IsNullOrEmpty())
                {
                    action = GetActionName();
                }

                routeValues.Add(RouteUtil.RouteActionName, action);
            }

            if (!routeValues.ContainsKey(RouteUtil.RouteControllerName))
            {
                if (controller.IsNullOrEmpty())
                {
                    controller = GetControllerName();
                }

                routeValues.Add(RouteUtil.RouteControllerName, controller);
            }

            return GetRouteUrl(routeName, routeValues);
        }

        private string GetRouteValue(string routeKey)
        {
            return DoGetRouteValue(routeKey).ToNonNullString();
        }
    }
}