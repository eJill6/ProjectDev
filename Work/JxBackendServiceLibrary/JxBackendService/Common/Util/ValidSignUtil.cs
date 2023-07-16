using JxBackendService.Common.Extensions;
using JxBackendService.Model.Attributes.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JxBackendService.Common.Util
{
    public static class ValidSignUtil
    {
        public static bool IsSignValid<T>(T values, string clientSign, string hashType = "Md5")
        {
            string localSign = CreateSign(values);

            return localSign.Equals(clientSign, StringComparison.OrdinalIgnoreCase);
        }

        public static string CreateSign<T>(T values, string hashType = "Md5")
        {
            PropertyInfo[] properties = values.GetType().GetProperties();
            var signKeyValueMap = new Dictionary<string, string>();
            var orderByMap = new Dictionary<string, int>();

            foreach (PropertyInfo propertyInfo in properties)
            {
                SignAttribute signAttribute = propertyInfo.GetCustomAttributes(typeof(SignAttribute), false).SingleOrDefault() as SignAttribute;

                if (signAttribute == null)
                {
                    continue;
                }

                string key = propertyInfo.Name;

                if (signAttribute.IsCamelCase)
                {
                    key = key[0].ToString().ToLower() + key.Substring(1);
                }

                string value = propertyInfo.GetValue(values).ToNonNullString();
                signKeyValueMap.Add(key, value);
                orderByMap.Add(key, signAttribute.SortNo);
            }

            List<KeyValuePair<string, string>> keyValuePairs = signKeyValueMap.OrderBy(o => orderByMap[o.Key]).ThenBy(t => t.Key).ToList();
            string queryString = string.Join("&", keyValuePairs.Select(s => $"{s.Key}={s.Value}"));

            string sign = null;

            if (hashType == "Md5")
            {
                sign = HashExtension.MD5(queryString);
            }
            else
            {
                throw new NotImplementedException();
            }

            sign = sign.ToLower();

            return sign;
        }
    }
}