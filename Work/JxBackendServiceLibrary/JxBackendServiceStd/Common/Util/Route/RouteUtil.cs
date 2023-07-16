using System;
using System.Collections.Generic;
using System.Reflection;

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

        public static Dictionary<string, object> ConvertToRouteValues(object routeParams, Func<PropertyInfo, bool> ignoreJob = null)
        {
            if (routeParams is Dictionary<string, object>)
            {
                return (Dictionary<string, object>)routeParams;
            }

            var routeValueDictionary = new Dictionary<string, object>();

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
    }
}