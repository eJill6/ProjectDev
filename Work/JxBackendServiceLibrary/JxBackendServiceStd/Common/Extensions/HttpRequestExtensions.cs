using JxBackendService.Common.Util;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Specialized;
using System.Linq;

namespace JxBackendService.Common.Extensions
{
    public static class HttpRequestExtensions
    {
        public static Uri ToUri(this HttpRequest request)
        {
            return new Uri($"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}");
        }
    }
}