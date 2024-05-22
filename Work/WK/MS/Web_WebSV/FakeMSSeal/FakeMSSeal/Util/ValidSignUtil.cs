using FakeMSSeal.Models;
using JxBackendService.Common.Util;
using JxBackendService.Model.Attributes.Security;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace FakeMSSeal.Util
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
                sign = MD5.HashData(Encoding.UTF8.GetBytes(queryString)).ToHexString();
            }
            else
            {
                throw new NotImplementedException();
            }

            sign = sign.ToLower();

            return sign;
        }

        private static string ToHexString(this byte[] vs)
        {
            return string.Join(string.Empty, vs.Select(x => x.ToString("X2")));
        }

        private static string GetSignRaw(object model)
        {
            var salt = $"{ToCamelCase(nameof(GlobalValue.Salt))}={GlobalValue.Salt}";

            if (model == null)
            {
                return salt;
            }

            var elements = model.GetType().GetProperties().OrderBy(x => x.Name).ToArray();
            var list = new List<string>();
            foreach (var element in elements)
            {
                if (element.PropertyType.IsArray)
                {
                    continue;
                }
                
                object? value = element.GetValue(model);
                if(value is Enum)
                {
                    value = (int) value;
                }
                list.Add($"{ToCamelCase(element.Name)}={value}");
            }
            list.Add(salt);
            return string.Join("&", list);
        }

        private static string ToCamelCase(string text)
        {
            if (!string.IsNullOrEmpty(text) && text.Length > 1)
            {
                return char.ToLowerInvariant(text[0]) + text.Substring(1);
            }
            return text.ToLowerInvariant();
        }

        public static string GetSign(object model)
        {
            return ToHexString(MD5.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(GetSignRaw(model))))
                .ToLower();
        }
    }
}