using JxBackendService.Model.Enums;
using System;
using System.Collections.Concurrent;
using System.Web;

namespace Web.Helpers
{
    public static class WebResourceHelper
    {
        private static readonly string s_webResourceVersion = GlobalVariables.StaticContentVersion;

        private static readonly string s_cdnSite = System.Configuration.ConfigurationManager.AppSettings["CDNSite"];

        private static readonly bool s_isUseCDN = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["UseCDN"]);

        private static readonly ConcurrentDictionary<string, string> s_urlMaps = new ConcurrentDictionary<string, string>();

        public static string Content(string resourceUrl)
        {
            return Content(resourceUrl, versionControl: true);
        }

        public static string Content(string resourceUrl, bool versionControl)
        {
            bool isUseMinifiedJavascriptFile = false;

            return Content(resourceUrl, versionControl, isUseMinifiedJavascriptFile);
        }

        public static string Content(string resourceUrl, bool versionControl, bool isUseMinifiedJavascriptFile)
        {
            if (string.IsNullOrEmpty(resourceUrl))
            {
                return string.Empty;
            }

            string mapKey = CreateUrlMapKey(resourceUrl, versionControl, isUseMinifiedJavascriptFile);

            if (s_urlMaps.ContainsKey(mapKey))
            {
                return s_urlMaps[mapKey];
            }

            string url = "";
            bool isAppRelative = resourceUrl[0] == '~';

            if (isAppRelative)
            {
                url = VirtualPathUtility.ToAbsolute(resourceUrl, HttpRuntime.AppDomainAppVirtualPath);
            }

            if (isUseMinifiedJavascriptFile)
            {
                //¿À¨dextension
                if (url.EndsWith(".js", StringComparison.OrdinalIgnoreCase) && !url.EndsWith(".min.js", StringComparison.OrdinalIgnoreCase))
                {
                    url = url.Replace(".js", ".min.js");
                }
            }

            if (versionControl)
            {
                url += "?v=" + s_webResourceVersion;
            }

            //string url = resourceUrl.Replace("~","");
            string returnUrl = null;

            if (!s_isUseCDN)
            {
                returnUrl = url;
            }
            else
            {
                returnUrl = s_cdnSite + url;
            }

            s_urlMaps.TryAdd(mapKey, returnUrl);

            return returnUrl;
        }

        private static string CreateUrlMapKey(string resourceUrl, bool versionControl, bool isUseMinifiedJavascriptFile)
        {
            return $"{resourceUrl}.{versionControl}.{isUseMinifiedJavascriptFile}";
        }
    }
}