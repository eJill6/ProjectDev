using JxBackendService.Common.Util;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace JxBackendService.Common.Extensions
{
    public static class HashExtension
    {
        public static string ToPasswordHash(this string password)
        {
            string result = password;
            result = MD5(result).ToLower();
            result = result.Substring(3, 16);
            result = SHA1(result).ToLower();
            result = result.Substring(4, 14);
            return result;
        }

        public static string MD5(string input)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(input, "MD5");
        }

        private static string SHA1(string input)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(input, "SHA1");
        }
    }
}
