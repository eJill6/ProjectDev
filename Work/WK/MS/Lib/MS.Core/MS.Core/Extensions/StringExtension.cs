using MS.Core.Utils;

namespace MS.Core.Extensions
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static string TrimStart(this string value, string trimString)
        {
            return ObjUtil.ToNonNullString(value).TrimStart(trimString.ToCharArray());
        }

        public static string TrimEnd(this string value, string trimString)
        {
            return ObjUtil.ToNonNullString(value).TrimEnd(trimString.ToCharArray());
        }

        /// <summary>
        /// 轉成camel
        /// </summary>
        /// <param name="text">元字串</param>
        /// <returns>camel字串</returns>
        public static string ToCamelCase(this string text)
        {
            if (!string.IsNullOrEmpty(text) && text.Length > 1)
            {
                return char.ToLowerInvariant(text[0]) + text.Substring(1);
            }
            return text.ToLowerInvariant();
        }

        /// <summary>
        /// 分割带国别的手机号码
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public static (string prefixes, string number) SplitWholePhoneNo(this string phoneNo) 
        {
            if (phoneNo.Length != 15 && phoneNo.Length != 14)
                return ("", phoneNo);
            else if (phoneNo.Contains(" "))
            {
                return (phoneNo.Split(' ')[0].Replace("+", ""), phoneNo.Split(' ')[1]);
            }
            else if(phoneNo.Contains("-")) 
            {
                return (phoneNo.Split('-')[0].Replace("+", ""), phoneNo.Split('-')[1]);
            }
            return ("", phoneNo);
        } 
    }
}
