using System.Security.Cryptography;
using System.Text;

namespace ControllerShareLib.Helpers.Security
{
    public class DESTool
    {
        private string DES_Key = "12345678";

        public DESTool(string key)
        {
            DES_Key = key;
        }

        #region DESEnCode DES加密
        public string DESEnCode(string pToEncrypt, CipherMode type = CipherMode.CBC)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = type;
            byte[] inputByteArray = Encoding.GetEncoding("UTF-8").GetBytes(pToEncrypt);

            des.Key = Encoding.ASCII.GetBytes(DES_Key);
            des.IV = Encoding.ASCII.GetBytes(DES_Key);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }
        #endregion

        #region DESDeCode DES解密
        public string DESDeCode(string pToDecrypt, CipherMode type = CipherMode.CBC)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = type;
            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }

            des.Key = Encoding.ASCII.GetBytes(DES_Key);
            des.IV = Encoding.ASCII.GetBytes(DES_Key);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();

            return Encoding.UTF8.GetString(ms.ToArray());
        }
        #endregion

    }
}