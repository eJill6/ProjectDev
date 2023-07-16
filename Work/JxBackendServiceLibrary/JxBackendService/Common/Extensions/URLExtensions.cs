using JxBackendService.Common.Util;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace JxBackendService.Common.Extensions
{
    public static class URLExtensions
    {
        public static string ToKeyValueURL(this object obj, bool isCamelCase = false)
        {
            var keyvalues = obj.GetType().GetProperties()
                .ToList()
                .Select(p => $"{ToCamelCase(p.Name, isCamelCase)}={p.GetValue(obj)}")
                .ToArray();

            return string.Join("&", keyvalues);
        }

        public static string ToFullPath(this HttpRequestBase httpRequestBase, string path)
        {
            return new Uri(new Uri(httpRequestBase.Url.Scheme + "://" + httpRequestBase.Url.Authority), path).ToString();
        }

        public static bool IsValidUrl(this string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public static string ExtendQueryParam(this string url, string queryParamName, string queryParamValue)
        {
            var uri = new Uri(url);
            var uriBuilder = new UriBuilder(uri);

            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[queryParamName] = queryParamValue;
            uriBuilder.Query = query.ToString();

            if (uri.IsDefaultPort)
            {
                uriBuilder.Port = -1; //避免轉出來的Url有帶預設port
            }

            return uriBuilder.ToString();
        }

        private static string ToCamelCase(string name, bool isToCamelCase)
        {
            if (!isToCamelCase)
            {
                return name;
            }

            return name.ToCamelCase();
        }
    }
}