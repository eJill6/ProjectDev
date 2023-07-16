using System.Security.Cryptography;
using System.Text;

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
            using (MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }

        private static string SHA1(string input)
        {
            using (SHA1 sha1 = System.Security.Cryptography.SHA1.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha1.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}