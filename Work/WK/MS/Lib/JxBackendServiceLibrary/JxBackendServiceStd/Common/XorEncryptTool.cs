using System;
using System.Text;

namespace JxBackendService.Common
{
    public static class XorEncryptTool
    {
        /// <summary>開放讓前端Vue能夠直接取用，改為public</summary>
        public static string Key => "ku3ikqienNb0kqao";

        public static string XorEncryptToString(byte[] bytes)
        {
            return Convert.ToBase64String(XorEncrypt(bytes));
        }
        
        public static string XorEncryptToString(string text)
        {
            return XorEncryptToString(Encoding.UTF8.GetBytes(text));
        }

        public static byte[] XorEncrypt(byte[] bytes)
        {
            byte[] encrypted = new byte[bytes.Length];

            for (int i = 0; i < bytes.Length; i++)
            {
                encrypted[i] = (byte)(bytes[i] ^ Key[i % Key.Length]);
            }

            return encrypted;
        }

        public static string XorDecryptToString(string text)
        {
            return Encoding.UTF8.GetString(XorDecrypt(text));
        }

        public static byte[] XorDecrypt(string text)
        {
            var decoded = Convert.FromBase64String(text);

            byte[] result = new byte[decoded.Length];

            for (int c = 0; c < decoded.Length; c++)
            {
                result[c] = (byte)((uint)decoded[c] ^ (uint)Key[c % Key.Length]);
            }

            return result;
        }
    }
}