using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace JxBackendService.Common
{
    public class DESTool
    {
        private readonly string _key = null;
        private readonly string _iv = null;

        public DESTool(string key) : this(key, key) { }

        public DESTool(string key, string iv)
        {
            _key = key;
            _iv = iv;
        }

        public string DESEnCode(string pToEncrypt)
        {
            return DESEnCode(pToEncrypt, null, false);
        }

        public string DESEnCode(string pToEncrypt, CipherMode? cipherMode, bool isReturnBase64String)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(_key),
                IV = Encoding.ASCII.GetBytes(_iv),                
            };

            if (cipherMode.HasValue)
            {
                des.Mode = cipherMode.Value;
            }

            byte[] inputByteArray = Encoding.GetEncoding("UTF-8").GetBytes(pToEncrypt);


            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            if (isReturnBase64String)
            {
                return Convert.ToBase64String(ms.ToArray());
            }

            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }

            return ret.ToString();
        }


        /// <summary>
        /// DES解密
        /// </summary>
        public string DESDeCode(string pToDecrypt)
        {
            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = ASCIIEncoding.ASCII.GetBytes(_key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(_iv);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();

            return System.Text.Encoding.UTF8.GetString(ms.ToArray());
        }

    }    
}