using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Web;

namespace SportGame.Client.Extensions
{
    public static class ObjectUtil
    {
        public static IDictionary<string, string> ToDictionary(object data)
        {
            var jsonStr = JsonConvert.SerializeObject(data);

            return JsonConvert.DeserializeObject<IDictionary<string, string>>(jsonStr);
        }

        public static string ToUrlQueryParamsString(object data)
        {
            var dataType = data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanRead);
            var dataParams = dataType.ToDictionary((t) => t.Name, (t) => t.GetValue(data, new object[0]))
                .Where(t => t.Value != null)
                .Select(t => $"{t.Key}={HttpUtility.UrlEncode(t.Value.ToString())}");

            return string.Join("&", dataParams);
        }
    }
}