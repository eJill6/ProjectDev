using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace JxBackendService.Common
{
    /// <summary>
    /// 第三方OB 使用
    /// </summary>
    public class AESTool
    {
        public static string Encrypt(string toEncrypt, string key)
        {
            return Encrypt(toEncrypt, key, isEncodeUrl: false);
        }

        public static string Encrypt(string toEncrypt, string key, bool isEncodeUrl)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(key);
            byte[] bytes2 = Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rijndaelManaged = new RijndaelManaged
            {
                Key = bytes,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor();
            byte[] array = cryptoTransform.TransformFinalBlock(bytes2, 0, bytes2.Length);

            var base64Str = Convert.ToBase64String(array, 0, array.Length);

            if (isEncodeUrl)
            {
                return HttpUtility.UrlEncode(base64Str);
            }

            return base64Str;
        }

        public static string Decrypt(string toDecrypt, string key)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(key);
            byte[] array = Convert.FromBase64String(toDecrypt);
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Key = bytes;
            rijndaelManaged.Mode = CipherMode.ECB;
            rijndaelManaged.Padding = PaddingMode.PKCS7;
            ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor();
            byte[] bytes2 = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
            return Encoding.UTF8.GetString(bytes2);
        }

        public static string AESEncrypt(string Data, string Key)
        {
            MemoryStream memoryStream = new MemoryStream();
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            byte[] bytes = Encoding.UTF8.GetBytes(Data);
            byte[] array = new byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(array.Length)), array, array.Length);
            rijndaelManaged.Mode = CipherMode.ECB;
            rijndaelManaged.Padding = PaddingMode.PKCS7;
            rijndaelManaged.KeySize = 128;
            rijndaelManaged.Key = array;
            CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write);
            try
            {
                cryptoStream.Write(bytes, 0, bytes.Length);
                cryptoStream.FlushFinalBlock();
                return Convert.ToBase64String(memoryStream.ToArray());
            }
            finally
            {
                cryptoStream.Close();
                memoryStream.Close();
                rijndaelManaged.Clear();
            }
        }

        public static string AESDecrypt(string Data, string Key)
        {
            byte[] array = Convert.FromBase64String(Data);
            byte[] array2 = new byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(array2.Length)), array2, array2.Length);
            MemoryStream memoryStream = new MemoryStream(array);
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Mode = CipherMode.ECB;
            rijndaelManaged.Padding = PaddingMode.PKCS7;
            rijndaelManaged.KeySize = 128;
            rijndaelManaged.Key = array2;
            CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Read);
            try
            {
                byte[] array3 = new byte[array.Length + 32];
                int num = cryptoStream.Read(array3, 0, array.Length + 32);
                byte[] array4 = new byte[num];
                Array.Copy(array3, 0, array4, 0, num);
                return Encoding.UTF8.GetString(array4);
            }
            finally
            {
                cryptoStream.Close();
                memoryStream.Close();
                rijndaelManaged.Clear();
            }
        }

        public static string AesEncrypt(string content, string aesKey, string aesIV, Func<string, string> replaceJob = null)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(aesKey);
            byte[] bytes2 = Encoding.UTF8.GetBytes(aesIV);
            byte[] bytes3 = Encoding.UTF8.GetBytes(content);
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Padding = PaddingMode.PKCS7;
            rijndaelManaged.Mode = CipherMode.CBC;
            rijndaelManaged.Key = bytes;
            rijndaelManaged.IV = bytes2;
            ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor(bytes, bytes2);
            byte[] inArray = cryptoTransform.TransformFinalBlock(bytes3, 0, bytes3.Length);
            cryptoTransform.Dispose();
            string result = Convert.ToBase64String(inArray);

            if (replaceJob != null)
            {
                result = replaceJob.Invoke(result);
            }

            return result;
        }
    }
}