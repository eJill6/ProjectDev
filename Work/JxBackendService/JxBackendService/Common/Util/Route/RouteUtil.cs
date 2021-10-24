using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Route;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JxBackendService.Common.Util.Route
{
    public static class RouteUtil
    {
        private static readonly string _routeConrollerName = "controller";
        private static readonly string _routeActionName = "action";
        public static readonly string RouteVersionName = "version";

        public static string GetVersionName()
        {
            return GetRouteValue(HttpContext.Current.Request.RequestContext.RouteData, RouteVersionName);
        }

        public static string GetControllerName()
        {
            return GetControllerName(HttpContext.Current.Request.RequestContext.RouteData);
        }

        public static string GetControllerName(RouteData routeData)
        {
            return GetRouteValue(routeData, _routeConrollerName);
        }

        public static string GetActionName()
        {
            return GetActionName(HttpContext.Current.Request.RequestContext.RouteData);
        }

        public static string GetActionName(RouteData routeData)
        {
            return GetRouteValue(routeData, _routeActionName);
        }

        public static RouteVersion GetRouteVersion()
        {
            return RouteVersion.Default;
            //return GetRouteVersion(HttpContext.Current.Request.RequestContext.RouteData);
        }

        public static RouteVersion GetRouteVersion(RouteData routeData)
        {
            return RouteVersion.Default;
            //string version = GetRouteValue(routeData, RouteVersionName);
            //return RouteVersion.GetSingle(version);
        }

        public static string ConvertToVersionViewPath(string viewPath)
        {
            RouteVersion routeVersion = GetRouteVersion();

            string returnPath = Regex.Replace(viewPath, "/Views/", RouteVersion.Default.ViewFolderRootPath, RegexOptions.IgnoreCase);

            if (routeVersion == RouteVersion.Default)
            {
                return GetMerchantFormatPath(returnPath);
            }

            //判斷是路徑還是名稱

            if (viewPath.StartsWith("~/") || viewPath.IndexOf("/") >= 0)
            {
                returnPath = Regex.Replace(viewPath, RouteVersion.Default.ViewFolderRootPath, routeVersion.ViewFolderRootPath, RegexOptions.IgnoreCase);
            }
            else
            {
                returnPath = $"{routeVersion.ViewFolderRootPath}/Shared/{viewPath}.cshtml";
            }

            return GetMerchantFormatPath(returnPath);
        }

        //public static string ConvertToVersionContentUrl(string contentUrl)
        //{
        //    RouteVersion routeVersion = GetRouteVersion();

        //    return ConvertToVersionContentUrl(contentUrl, routeVersion);
        //}

        //public static string ConvertToVersionContentUrl(string contentUrl, RouteVersion routeVersion)
        //{
        //    string returnUrl = Regex.Replace(contentUrl, routeVersion.ContentFolderRootUrl, RegexOptions.IgnoreCase);

        //    return GetMerchantFormatPath(returnUrl);
        //}

        public static string ConvertToVersionBundleVirtualPath(string virtualPath)
        {
            RouteVersion routeVersion = GetRouteVersion();

            string returnPath = Regex.Replace(virtualPath, "~/bundles/", RouteVersion.Default.BundleVirtualRootPath, RegexOptions.IgnoreCase);

            if (routeVersion != RouteVersion.Default)
            {
                returnPath = Regex.Replace(virtualPath, RouteVersion.Default.BundleVirtualRootPath, routeVersion.BundleVirtualRootPath, RegexOptions.IgnoreCase);
            }

            return GetMerchantFormatPath(returnPath);
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
            return GetRouteUrl(RouteName.Default, action, controller, routeParams, true);
        }

        public static string GetRouteUrl(RouteName routeName, string action, string controller, object routeParams)
        {
            return GetRouteUrl(routeName, action, controller, routeParams, true);
        }

        public static string GetRouteUrl(RouteName routeName, string action, string controller, object routeParams, bool hasRouteVersion)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            RouteValueDictionary routeValues = ConvertToRouteValues(routeParams);

            if (!routeValues.ContainsKey(_routeActionName))
            {
                if (action.IsNullOrEmpty())
                {
                    action = GetActionName();
                }

                routeValues.Add(_routeActionName, action);
            }

            if (!routeValues.ContainsKey(_routeConrollerName))
            {
                if (controller.IsNullOrEmpty())
                {
                    controller = GetControllerName();
                }

                routeValues.Add(_routeConrollerName, controller);
            }

            string route = routeName.Value;

            //if (hasRouteVersion)
            //{
            //    RouteVersion routeVersion = GetRouteVersion();

            //    if (routeVersion != null && routeVersion != RouteVersion.Default)
            //    {
            //        route += routeVersion.Value;
            //    }
            //}

            return urlHelper.RouteUrl(route, routeValues);
        }

        public static string GetRouteValue(string routeKey)
        {
            return GetRouteValue(HttpContext.Current.Request.RequestContext.RouteData, routeKey);
        }

        private static string GetRouteValue(RouteData routeData, string routeKey)
        {
            return routeData.Values[routeKey].ToNonNullString();
        }

        private static RouteValueDictionary ConvertToRouteValues(object routeParams)
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

            foreach (var typeProperty in typeProperties)
            {
                object value = typeProperty.GetValue(routeParams);

                if (value != null)
                {
                    routeValueDictionary.Add(typeProperty.Name, value);
                }
            }

            if (!routeValueDictionary.ContainsKey(RouteVersionName))
            {
                routeValueDictionary.Add(RouteVersionName, GetVersionName());
            }

            return routeValueDictionary;
        }

        /// <summary>
        /// 填入商戶路徑
        /// </summary>
        public static string GetMerchantFormatPath(string path)
        {
            return path.Replace(GlobalVariables.MerchantFolderCode, SharedAppSettings.PlatformMerchant.MerchantFolder)
                .Replace("//", "/").Replace("\\\\", "\\");
        }

        /// <summary>
        /// 替換原有路徑為商戶路徑
        /// </summary>
        public static string ReplaceMerchantFormatPath(string path)
        {
            string newPath = Regex.Replace(path, "/Views/", RouteVersion.Default.ViewFolderRootPath);

            return GetMerchantFormatPath(newPath);
        }

        /// <summary>
        /// 替換原有路徑為商戶路徑(多個)
        /// </summary>
        public static string[] ReplaceMerchantFormatPaths(string[] paths)
        {
            return paths.Select(ReplaceMerchantFormatPath).ToArray();
        }
    }
}
