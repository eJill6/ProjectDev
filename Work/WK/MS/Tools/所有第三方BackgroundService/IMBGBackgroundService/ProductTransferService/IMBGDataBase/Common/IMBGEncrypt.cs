using System.Security.Cryptography;
using System.Text;

namespace ProductTransferService.IMBGDataBase.Common
{
    public class IMBGEncrypt
    {
        /// <summary>
        /// DES加密方法
        /// </summary>
        public static string DESEncrypt(string text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);// 密匙
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);// 初始化向量
            MemoryStream ms = new MemoryStream();
            des.Mode = CipherMode.ECB;
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            var retB = Convert.ToBase64String(ms.ToArray());
            return retB;
        }
    }
}