using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace JxBackendService.Common
{
    public class TripleDESTool
    {
        private static readonly byte[] NULL_IV = Convert.FromBase64String("AAAAAAAAAAA=");

        private TripleDESTool()
        { }

        /// <summary>
        /// 解密
        /// </summary>
        public static string Decrypt(string data, string base64edKey, string base64edIv)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Mode = CipherMode.CBC;
            des.BlockSize = 64;
            des.Padding = PaddingMode.PKCS7;
            des.Key = Convert.FromBase64String(base64edKey);
            des.IV = base64edIv == null ? NULL_IV : Convert.FromBase64String(base64edIv);

            MemoryStream stream = new MemoryStream(Convert.FromBase64String(data));

            CryptoStream encStream = new CryptoStream(stream, des.CreateDecryptor(des.Key, des.IV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(encStream, System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();

            sr.Close();
            encStream.Close();
            stream.Close();

            return result;
        }

        /// <summary>
        /// 加密
        /// </summary>
        public static string Encrypt(string data, string base64Key, string base64edIv)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Mode = CipherMode.CBC;
            des.BlockSize = 64;
            des.Padding = PaddingMode.PKCS7;
            des.Key = Convert.FromBase64String(base64Key);
            des.IV = base64edIv == null ? NULL_IV : Convert.FromBase64String(base64edIv);

            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            MemoryStream stream = new MemoryStream();

            CryptoStream encStream = new CryptoStream(stream, des.CreateEncryptor(), CryptoStreamMode.Write);
            encStream.Write(dataBytes, 0, dataBytes.Length);
            encStream.FlushFinalBlock();

            string result = Convert.ToBase64String(stream.ToArray());
            encStream.Close();
            stream.Close();
            return result;
        }
    }
}