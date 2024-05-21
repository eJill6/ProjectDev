using SLPolyGame.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace SLPolyGame.Web.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static T GetRequestPropertyValue<T>(this HttpRequestMessage request, RequestMessagePropertyKey key) where T : class
        {
            if (request.Properties.ContainsKey(key.Value))
            {
                return request.Properties[key.Value] as T;
            }

            return default(T);
        }

        public static void SetRequestPropertyValue<T>(this HttpRequestMessage request, RequestMessagePropertyKey key, T value) where T : class
        {
            request.Properties[key.Value] = value;
        }
    }
}