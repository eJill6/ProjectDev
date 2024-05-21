using System.Security.Cryptography;
using System.Text;

namespace MMService.Helpers
{
    /// <summary>
    /// 解密Des小幫手
    /// </summary>
    public static class DecryptDesHelper
    {
        /// <summary>
        /// 連線串解密的元件
        /// </summary>
        private static readonly string KeyUse = "4@2w!dsf";
        /// <summary>
        /// 連線串解密的元件
        /// </summary>
        private static readonly byte[] Keys = new byte[] { 18, 52, 86, 120, 144, 171, 205, 239 };

        /// <summary>
        /// 解密Des
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public static string DecryptDES(string decryptString)
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(KeyUse);
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            var dcsp = new DESCryptoServiceProvider();
            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memStream, dcsp.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
            cryptoStream.FlushFinalBlock();
            var str = Encoding.UTF8.GetString(memStream.ToArray());

            return str;
        }

        public static string EncryptDES(string encryptString)
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(KeyUse);
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            var dcsp = new DESCryptoServiceProvider();
            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memStream, dcsp.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
            cryptoStream.FlushFinalBlock();
            var str = Convert.ToBase64String(memStream.ToArray());

            return str;
        }
    }
}
