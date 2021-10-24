using JxBackendService.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace JxBackendService.Common.Extensions
{
    public static class URLExtensions
    {
        public static string ToKeyValueURL(this object obj)
        {
            var keyvalues = obj.GetType().GetProperties()
                .ToList()
                .Select(p => $"{p.Name}={p.GetValue(obj)}")
                .ToArray();

            return string.Join("&", keyvalues);
        }

        public static string ToFullPath(this HttpRequestBase httpRequestBase, string path)
        {
            return new Uri(new Uri(httpRequestBase.Url.Scheme + "://" + httpRequestBase.Url.Authority), path).ToString();
        }
    }
}
