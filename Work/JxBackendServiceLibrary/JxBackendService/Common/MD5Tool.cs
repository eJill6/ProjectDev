using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using JxBackendService.Common.Util;

namespace JxBackendService.Common
{
    public class MD5Tool
    {
        /// <summary>
        /// 第三方All Bet使用
        /// </summary>
        public static string Base64edMd5(string data)
        {
            return Convert.ToBase64String(Md5(Encoding.UTF8.GetBytes(data)));
        }

        public static byte[] Md5(byte[] data)
        {
            MD5CryptoServiceProvider md5Crp = new MD5CryptoServiceProvider();

            return md5Crp.ComputeHash(data);
        }

        /// <summary> 第三方OB使用 </summary>
        public static string MD5EncodingForOBGameProvider(string rawPass)
        {
            MD5 mD = MD5.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(rawPass);
            byte[] hashBytes = mD.ComputeHash(bytes);
            var stringBuilder = new StringBuilder();

            foreach (byte hashByte in hashBytes)
            {
                stringBuilder.Append(hashByte.ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        /// <summary> 第三方IMBG使用 </summary>
		public static string MD5EncodingForIMBGGameProvider(string content)
        {
            var encoding = new UTF8Encoding();
            byte[] bytes = encoding.GetBytes(content);
            byte[] hashBytes = Md5(bytes);

            // Convert the encrypted bytes back to a string (base 16)
            string encryptString = hashBytes.Aggregate("", (current, t) => current + Convert.ToString(t, 16).PadLeft(2, '0'));

            return encryptString.PadLeft(32, '0').ToLower();
        }

        /// <summary>第三方龍城棋牌使用 MD5加密方法 utf8编码</summary>
        public static string MD5EncodingForLCGameProvider(string content)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = Encoding.UTF8.GetBytes(content);
            byte[] result = md5.ComputeHash(data);
            string ret = string.Empty;

            for (int i = 0; i < result.Length; i++)
            {
                ret += result[i].ToString("x").PadLeft(2, '0');
            }

            return ret;
        }

        /// <summary> 用於舊D3加密密碼，從舊D3搬過來的源碼 </summary>
        public static string MD5EncodingForOldD3(string content, string salt = null)
        {
            using (MD5 md5 = MD5.Create())
            {
                if (!salt.IsNullOrEmpty())
                {
                    content = $"{content}:{salt}";
                }

                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(content));
                var stringBuilder = new StringBuilder();
                foreach (byte hashByte in hashBytes)
                {
                    stringBuilder.Append(hashByte.ToString("x2"));
                }

                return stringBuilder.ToString().ToUpperInvariant();
            }
        }

        public static string ToMD5(string data, bool isToUpper)
        {
            return BaseToMD5(data, (hashBytes) =>
            {
                var stringBuilder = new StringBuilder();
                string format = "x2";

                if (isToUpper)
                {
                    format = "X2";
                }

                foreach (byte hashByte in hashBytes)
                {
                    stringBuilder.Append(hashByte.ToString(format));
                }

                //取得 MD5
                string md5Result = stringBuilder.ToString();

                return md5Result;
            });
        }

        private static string BaseToMD5(string data, Func<byte[], string> doConvert)
        {
            //將字串編碼成 UTF8 位元組陣列
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hashBytes = Md5(bytes);

            return doConvert.Invoke(hashBytes);
        }
    }
}