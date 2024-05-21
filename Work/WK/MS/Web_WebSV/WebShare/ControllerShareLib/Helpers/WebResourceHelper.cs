using JxBackendService.Common.Extensions;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Model.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace ControllerShareLib.Helpers
{
    public static class WebResourceHelper
    {
        private static readonly string s_webResourceVersion = GlobalVariables.StaticContentVersion;

        private static readonly ConcurrentDictionary<string, string> s_urlMaps = new ConcurrentDictionary<string, string>();

        private static readonly List<string> s_rejectCDNAbsolutePaths = new List<string>()
        {
            "//",
            "http://",
            "https://",
        };

        public static string Content(string resourceUrl)
        {
            return Content(resourceUrl, isAppendVersion: true, isUseRequestHost: false);
        }

        //public static string Content(string resourceUrl, bool versionControl)
        //{
        //    bool isUseMinifiedJavascriptFile = false;

        //    return Content(resourceUrl, versionControl, isUseMinifiedJavascriptFile);
        //}

        public static string Content(string resourceUrl, bool isAppendVersion, bool isUseRequestHost)
        {
            if (string.IsNullOrEmpty(resourceUrl))
            {
                return string.Empty;
            }

            string mapKey = CreateUrlMapKey(resourceUrl, isAppendVersion);

            if (s_urlMaps.ContainsKey(mapKey))
            {
                return s_urlMaps[mapKey];
            }

            string absolutePath = resourceUrl;
            var httpContextService = DependencyUtil.ResolveService<IHttpContextService>().Value;

            if (resourceUrl.StartsWith("~/"))
            {
                if (httpContextService.HasHttpContext())
                {
                    var urlHelper = DependencyUtil.ResolveService<IUrlHelper>().Value;
                    absolutePath = urlHelper.Content(resourceUrl);
                }
                else
                {
                    absolutePath = resourceUrl.TrimStart('~');
                }
            }

            string resourceHost = string.Empty;
            string versionQueryString = string.Empty;

            if (isAppendVersion)
            {
                versionQueryString = $"v={s_webResourceVersion}";

                if (absolutePath.IndexOf("?") < 0)
                {
                    versionQueryString = $"?{versionQueryString}";
                }
            }

            if ((GlobalCacheHelper.IsUseCDN &&
                !s_rejectCDNAbsolutePaths.Any(a => absolutePath.StartsWith(a, StringComparison.OrdinalIgnoreCase))) ||
                !httpContextService.HasHttpContext())
            {
                resourceHost = GetCDNHost(absolutePath);
            }
            else if (isUseRequestHost)
            {
                resourceHost = httpContextService.GetSchemeAndHost();
            }

            var httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>().Value;
            string fullUrl = httpWebRequestUtilService.CombineUrl(resourceHost, absolutePath, versionQueryString);
            s_urlMaps.TryAdd(mapKey, fullUrl);

            return fullUrl;
        }

        private static string CreateUrlMapKey(string resourceUrl, bool versionControl)
        {
            return $"{resourceUrl}.{versionControl}";
        }

        private static string GetCDNHost(string absolutePath)
        {
            bool isAESExtension = absolutePath.IsAESExtension();

            if (isAESExtension)
            {
                return GlobalCacheHelper.AESCDNSite;
            }
            else
            {
                return GlobalCacheHelper.CDNSite;
            }
        }
    }
}