using JxBackendService.Common.Util;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace JxBackendServiceNF.Common.Extensions
{
    public static class URLExtensions
    {
        public static string ToFullPath(this HttpRequestBase httpRequestBase, string path)
        {
            return new Uri(new Uri(httpRequestBase.Url.Scheme + "://" + httpRequestBase.Url.Authority), path).ToString();
        }
    }
}