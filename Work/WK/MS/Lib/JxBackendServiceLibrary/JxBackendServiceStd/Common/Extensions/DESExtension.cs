using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace JxBackendService.Common.Extensions
{
    public static class DESExtension
    {
        public static string DESEncrypt(this string pToEncrypt, string key)
        {
            //访问数据加密标准(DES)算法的加密服务提供程序 (CSP) 版本的包装对象
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = Encoding.ASCII.GetBytes(key);　//建立加密对象的密钥和偏移量
            des.IV = Encoding.ASCII.GetBytes(key);　 //原文使用ASCIIEncoding.ASCII方法的GetBytes方法

            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);//把字符串放到byte数组中

            MemoryStream ms = new MemoryStream();//创建其支持存储区为内存的流　
            //定义将数据流链接到加密转换的流
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //上面已经完成了把加密后的结果放到内存中去

            StringBuilder ret = new StringBuilder();

            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }

            return ret.ToString();
        }

        public static string DESDecrypt(this string pToDecrypt, string key)
        {
            pToDecrypt = System.Web.HttpUtility.UrlDecode(pToDecrypt);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = Encoding.ASCII.GetBytes(key);　//建立加密对象的密钥和偏移量，此值重要，不能修改
            des.IV = Encoding.ASCII.GetBytes(key);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            return Encoding.Default.GetString(ms.ToArray());
        }
    }
}
