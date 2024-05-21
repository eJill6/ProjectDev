using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyDataBase.Extensions
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        public static string FromBase64String(this string value) => Encoding.UTF8.GetString(Convert.FromBase64String(value));

        public static string ToBase64String(this string value) => Convert.ToBase64String(Encoding.UTF8.GetBytes(value));

        public static string ToCamelCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return char.ToLowerInvariant(str[0]) + str.Substring(1);
            }
            return str.ToLowerInvariant();
        }
    }
}