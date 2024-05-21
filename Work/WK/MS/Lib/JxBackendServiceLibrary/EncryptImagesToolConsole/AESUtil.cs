using System.Security.Cryptography;
using System.Text;

namespace EncryptImagesTool
{
    public static class AESUtil
    {
        private static readonly byte[] s_keyContents = Encoding.UTF8.GetBytes("94a4b778g01ca4ab");

        public static byte[] Encrypt(byte[] data)
        {
            // AES 加密
            using (var aes = Aes.Create())
            {
                aes.Key = s_keyContents;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                // 加密
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, rgbIV: null);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.FlushFinalBlock();

                        return memoryStream.ToArray();
                    }
                }
            }
        }

        public static byte[] Decrypt(byte[] data)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = s_keyContents;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, rgbIV: null);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.FlushFinalBlock();

                        return memoryStream.ToArray();
                    }
                }
            }
        }
    }
}