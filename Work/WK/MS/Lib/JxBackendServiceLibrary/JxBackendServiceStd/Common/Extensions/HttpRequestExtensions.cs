using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Specialized;
using System.Web;

namespace JxBackendService.Common.Extensions
{
    public static class HttpRequestExtensions
    {
        public static Uri ToUri(this HttpRequest request)
        {
            return new Uri($"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}");
        }

        public static string ToQueryString(this NameValueCollection nameValueCollection)
        {
            NameValueCollection queryStringCollection = HttpUtility.ParseQueryString(string.Empty);
            queryStringCollection.Add(nameValueCollection);

            return queryStringCollection.ToString();
        }
    }
}