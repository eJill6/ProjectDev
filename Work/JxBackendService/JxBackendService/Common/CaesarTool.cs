using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Common
{
    public static class CaesarTool
    {
        public static string CaesarEncryption(string PlainText, int Offset)
        {
            //需要置換的拉丁字母
            char[] Encyclopedia = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            StringBuilder sb = new StringBuilder();
            foreach (char c in PlainText.ToUpper())
            {
                int idx = c - 'A';//65
                if (idx >= 0 && idx < Encyclopedia.Length)
                {
                    sb.Append(Encyclopedia[((idx + Offset) % 26)]);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string ToCaesarEncode(this string plainText)
        {
            return CaesarEncryption(plainText, 5);
        }

        public static string ToCaesarDecode(this string plainText)
        {
            return CaesarEncryption(plainText, 21);
        }
    }
}
