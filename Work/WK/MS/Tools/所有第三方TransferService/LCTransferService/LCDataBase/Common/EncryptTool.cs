using System;
using System.Security.Cryptography;
using System.Text;

namespace LCDataBase.Common
{
    public class EncryptTool
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] result = md5.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < result.Length; i++)
                ret += result[i].ToString("x").PadLeft(2, '0');
            return ret;
        }

        /// <summary>
        ///  AES 加密
        /// </summary>
        /// <param name="str">需加密字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="isEncodeUrl">是否需要URL编码</param>
        /// <returns></returns>
        public static string AESEncrypt(string str, string key, bool isEncodeUrl = true)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            var base64Str = Convert.ToBase64String(resultArray, 0, resultArray.Length);

            return isEncodeUrl ? System.Web.HttpUtility.UrlEncode(base64Str) : base64Str;
        }

        /// <summary>
        ///  AES 解密
        /// </summary>
        /// <param name="str">需解密字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="isDecodeUrl">是否需要URL解码</param>
        /// <returns></returns>
        public static string AESDecrypt(string str, string key, bool isDecodeUrl = true)
        {
            if (string.IsNullOrEmpty(str)) return null;
            if (isDecodeUrl)
                str = System.Web.HttpUtility.UrlDecode(str);

            Byte[] toEncryptArray = Convert.FromBase64String(str);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }
    }
}
