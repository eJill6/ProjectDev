using JxBackendService.Model.Enums.Route;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JxBackendService.Common.Util.Route
{
    public static class RouteUtil
    {
        private static readonly string s_routeControllerName = "controller";

        private static readonly string s_routeActionName = "action";

        private static readonly string s_routeMiseWebTokenName = "MiseWebToken";

        public static string RouteControllerName => s_routeControllerName;

        public static string RouteActionName => s_routeActionName;

        public static string RouteMiseWebTokenName => s_routeMiseWebTokenName;

        public static string GetControllerName()
        {
            return GetControllerName(HttpContext.Current.Request.RequestContext.RouteData);
        }

        public static string GetControllerName(RouteData routeData)
        {
            return GetRouteValue(routeData, s_routeControllerName);
        }

        public static string GetActionName()
        {
            return GetActionName(HttpContext.Current.Request.RequestContext.RouteData);
        }

        public static string GetActionName(RouteData routeData)
        {
            return GetRouteValue(routeData, s_routeActionName);
        }

        public static string GetMiseWebTokenName()
        {
            return GetMiseWebTokenName(HttpContext.Current.Request.RequestContext.RouteData);
        }

        public static string GetMiseWebTokenName(RouteData routeData)
        {
            return GetRouteValue(routeData, s_routeMiseWebTokenName);
        }

        public static string GetRouteUrl(RouteName routeName, string action)
        {
            return GetRouteUrl(routeName, action, null, null);
        }

        public static string GetRouteUrl(RouteName routeName, string action, string controller)
        {
            return GetRouteUrl(routeName, action, controller, null);
        }

        public static string GetRouteUrl(string action, string controller, object routeParams)
        {
            return GetRouteUrl(RouteName.Default, action, controller, routeParams);
        }

        public static string GetMiseWebTokenUrl(string action, string controller, object routeParams)
        {
            RouteValueDictionary routeValues = ConvertToRouteValues(routeParams);
            string miseWebToken = GetMiseWebTokenName();

            if (!miseWebToken.IsNullOrEmpty())
            {
                routeValues.Add(s_routeMiseWebTokenName, miseWebToken);
            }

            return GetRouteUrl(RouteName.MiseWebToken, action, controller, routeParams);
        }

        public static string GetRouteUrl(RouteName routeName, string action, string controller, object routeParams)
        {
            RouteValueDictionary routeValues = ConvertToRouteValues(routeParams);

            if (!routeValues.ContainsKey(s_routeActionName))
            {
                if (action.IsNullOrEmpty())
                {
                    action = GetActionName();
                }

                routeValues.Add(s_routeActionName, action);
            }

            if (!routeValues.ContainsKey(s_routeControllerName))
            {
                if (controller.IsNullOrEmpty())
                {
                    controller = GetControllerName();
                }

                routeValues.Add(s_routeControllerName, controller);
            }

            string route = routeName.Value;
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            return urlHelper.RouteUrl(route, routeValues);
        }

        public static RouteValueDictionary ConvertToRouteValues(object routeParams, Func<PropertyInfo, bool> ignoreJob = null)
        {
            if (routeParams is RouteValueDictionary)
            {
                return (RouteValueDictionary)routeParams;
            }

            var routeValueDictionary = new RouteValueDictionary();

            if (routeParams == null)
            {
                return routeValueDictionary;
            }

            List<PropertyInfo> typeProperties = ModelUtil.TypePropertiesCache(routeParams.GetType());

            foreach (PropertyInfo typeProperty in typeProperties)
            {
                if (ignoreJob != null && ignoreJob.Invoke(typeProperty))
                {
                    continue;
                }

                object value = typeProperty.GetValue(routeParams);

                if (value != null)
                {
                    routeValueDictionary.Add(typeProperty.Name, value);
                }
            }

            return routeValueDictionary;
        }

        public static string RemoveControllerNameSuffix(this string controllerName)
        {
            if (controllerName.IsNullOrEmpty())
            {
                return controllerName;
            }

            if (controllerName.EndsWith(s_routeControllerName, System.StringComparison.OrdinalIgnoreCase))
            {
                return controllerName.Substring(0, controllerName.Length - s_routeControllerName.Length);
            }

            return controllerName;
        }

        private static string GetRouteValue(RouteData routeData, string routeKey)
        {
            return routeData.Values[routeKey].ToNonNullString();
        }
    }
}