using JxBackendService.Common.Util;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;

namespace JxBackendService.Common.Extensions
{
    public static class URLExtensions
    {
        private static readonly string s_aesExtension = ".aes";

        public static string ToKeyValueURL(this object obj, bool isCamelCase = false)
        {
            var keyvalues = obj.GetType().GetProperties()
                .ToList()
                .Select(p => $"{ToCamelCase(p.Name, isCamelCase)}={p.GetValue(obj)}")
                .ToArray();

            return string.Join("&", keyvalues);
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

        public static bool IsAESExtension(this string url)
        {
            if (url.IsNullOrEmpty())
            {
                return false;
            }

            string urlExtension = Path.GetExtension(url);

            return urlExtension.ToLower().Equals(s_aesExtension);
        }

        public static string ConvertToAESExtension(this string url)
        {
            if (url.IsNullOrEmpty())
            {
                return string.Empty;
            }

            string urlExtension = Path.GetExtension(url);

            return url.Replace(urlExtension, s_aesExtension);
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