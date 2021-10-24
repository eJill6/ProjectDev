using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JxBackendService.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JxBackendService.Common.Util
{
    public static class StringUtil
    {
        private static readonly string _amountFormat = "{0:N2}";
        private static readonly string _amountWithoutThousandthCommaFormat = "{0:F2}";
        private static readonly string _usdtAmountFormat = "{0:N6}";
        private static readonly string _usdtRateFormat = "{0:N3}";
        private static readonly string _apiProfitLossNumberFormat = "{0:F4}";
        private static readonly string _thousandCommaFormat = "{0:N4}";
        private static readonly decimal _defaultValue = 0;

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static string TrimStart(this string value, string trimString)
        {
            return value.ToNonNullString().TrimStart(trimString.ToCharArray());
        }

        public static string TrimEnd(this string value, string trimString)
        {
            return value.ToNonNullString().TrimEnd(trimString.ToCharArray());
        }

        public static string ToTrimString(this object obj)
        {
            return obj.ToNonNullString().Trim();
        }

        public static string ToNonNullString(this object obj)
        {
            if (obj != null)
            {
                return obj.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public static int ToInt32(this string data, bool hasDefaultValue = false)
        {
            if (hasDefaultValue)
            {
                if (int.TryParse(data, out int inputNumber))
                {
                    return inputNumber;
                }
                return Convert.ToInt32(_defaultValue);
            }
            else
            {
                return Convert.ToInt32(data);
            }
        }

        public static long ToInt64(this string data, bool hasDefaultValue = false)
        {
            if (hasDefaultValue)
            {
                if (long.TryParse(data, out long inputNumber))
                {
                    return inputNumber;
                }
                return Convert.ToInt64(_defaultValue);
            }
            else
            {
                return Convert.ToInt64(data);
            }
        }

        public static decimal ToDecimal(this string data, bool hasDefaultValue = false)
        {
            if (hasDefaultValue)
            {
                if (data.IsNullOrEmpty())
                {
                    return _defaultValue;
                }

                if (decimal.TryParse(data, out decimal inputNumber))
                {
                    return inputNumber;
                }

                return _defaultValue;
            }
            else
            {
                return decimal.Parse(data);
            }
        }

        /// <summary>
        /// 轉為貨幣格式
        /// </summary>        
        /// <param name="number">數值資料</param>
        /// <param name="hasThousandComma">是否加上千分位</param>
        public static string ToCurrency(this decimal number, bool hasThousandComma = false)
        {
            if (!hasThousandComma)
            {
                return number.ToString("0.0000");
            }
            else
            {
                return ToBaseFormatString(number, _thousandCommaFormat);
            }
        }

        public static string ToCurrency(this decimal? number, bool hasThousandComma = false)
        {
            return number.GetValueOrDefault().ToCurrency(hasThousandComma);
        }

        public static string RemoveStartsWith(this string str, string value)
        {
            if (str == null)
            {
                return null;
            }

            if (str.StartsWith(value))
            {
                return str.Substring(value.Length);
            }
            else
            {
                return str;
            }
        }

        public static bool IsValidJson(string strInput)
        {
            strInput = strInput.ToTrimString();

            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException)
                {
                    //Exception in parsing json
                    return false;
                }
                catch (Exception) //some other exception
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static string ToShortString(this string str, int length)
        {
            if (str.IsNullOrEmpty())
            {
                return string.Empty;
            }
            else
            {
                return (str.Length > length ? str.Substring(0, length) + "..." : str);
            }
        }

        private static string ToBaseFormatString<T>(this T source, string format)
        {
            string inputString = string.Empty;

            if (source != null)
            {
                inputString = source.ToString();
            }

            if (!inputString.IsNullOrEmpty() && decimal.TryParse(inputString, out decimal inputNumber))
            {
                return string.Format(format, inputNumber);
            }
            else
            {
                return string.Format(format, 0);
            }
        }

        public static string ToAmountString<T>(this T source)
        {
            return source.ToBaseFormatString(_amountFormat);
        }

        public static string ToAmountWithoutThousandthCommaString<T>(this T source)
        {
            return source.ToBaseFormatString(_amountWithoutThousandthCommaFormat);
        }

        public static string ToUSDTAmountString<T>(this T source)
        {
            return source.ToBaseFormatString(_usdtAmountFormat);
        }

        public static string ToUSDTRateString<T>(this T source)
        {
            return source.ToBaseFormatString(_usdtRateFormat);
        }

        public static string ToApiProfitLossNumberString<T>(this T source)
        {
            return source.ToBaseFormatString(_apiProfitLossNumberFormat);
        }

        public static string CreateRandomString(int randomKeyLength)
        {
            string randomString = new RandomStringCreator.StringCreator().Get(randomKeyLength);
            return randomString;
        }

        /// <summary> 功能：产生数字和字符混合的随机字符串 </summary>
        public static string CreateRandomCode(int numberLength, int lowerLetterLength, int upperLetterLength, int symbolsLength)
        {
            // 数字和字符混合字符串
            string[] numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            string[] lowerLetters = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            string[] upperLetters = lowerLetters.Select(s => s.ToUpper()).ToArray();
            string[] symbols = { "!", "#", "$", "&"}; // 只留下這幾個, 因為其他特殊符號会造成混謠
            //string[] symbols = { "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "+", "?", ",", ".", ">" }; // 移除 "<",
                                                                                                                   // 因為發現此符號與任意字母搭配如: "<a" 都會出現錯誤 原因待排查

            //随机数实例
            System.Random rand = new System.Random(unchecked((int)DateTime.Now.Ticks));
            StringBuilder randomCode = new StringBuilder();

            var contentMap = new List<KeyValuePair<int, string[]>>
            {
                new KeyValuePair<int, string[]>(numberLength, numbers),
                new KeyValuePair<int, string[]>(lowerLetterLength, lowerLetters),
                new KeyValuePair<int, string[]>(upperLetterLength, upperLetters),
                new KeyValuePair<int, string[]>(symbolsLength, symbols),
            };

            foreach (KeyValuePair<int, string[]> kvp in contentMap)
            {
                string[] source = kvp.Value;
                int codeLength = kvp.Key;

                for (int i = 0; i < codeLength; i++)
                {
                    int index = rand.Next(source.Length);
                    randomCode.Append(source[index]);
                }
            }

            return randomCode.ToString().Scramble();
        }

        public static string Scramble(this string s)
        {
            return new string(s.ToCharArray().OrderBy(x => Guid.NewGuid()).ToArray());
        }

        public static string MaskUSDT(string WalletAddr)
        {
            string maskWalletAddr = "";
            if (WalletAddr.Length > 10)
            {
                maskWalletAddr = WalletAddr.Substring(0, 5);
                for (int i = 0; i < WalletAddr.Length - 10; i++)
                {
                    maskWalletAddr += "*";
                }
                maskWalletAddr += WalletAddr.Substring(WalletAddr.Length - 5, 5);
            }
            else
            {
                maskWalletAddr = WalletAddr;
            }
            return maskWalletAddr;
        }

        public static string MaskBankCardContent(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }

            int beginPosition = 0;
            int endPosition = 0;
            string masks = string.Empty;

            if (str.Length > 8)
            {
                beginPosition = 4;
                endPosition = 4;
            }
            else
            {
                int num = 0;
                num = (int)(Math.Floor(Convert.ToDouble(str.Length) / 3));
                beginPosition = num;
                endPosition = num;
            }

            masks = MaskContent(str, beginPosition, endPosition);
            str = str.Substring(0, beginPosition) + masks + str.Substring((str.Length - endPosition), endPosition);
            return str;
        }

        private static string MaskContent(string str, int beginPosition, int endPosition)
        {
            string masks = string.Empty;
            for (int i = beginPosition; i < (str.Length - endPosition); i++)
            {
                masks += "*";
            }

            return masks;
        }

        public static string ToFullFileUrl(string host, string filePath)
        {
            string fullUrl = Path.Combine(host, filePath);

            //處理版本
            fullUrl = string.Concat(fullUrl, "?v=", GlobalVariables.StaticContentVersion);

            return fullUrl;
        }
    }

    //public static string ToCamelCase(this string str)
    //{
    //    if (str.IsNullOrEmpty())
    //    {
    //        return str;
    //    }

    //    var tempDic = new Dictionary<string, string>
    //    {
    //        { str, null }
    //    };

    //    string json = tempDic.ToJsonString(false, false, true, true);

    //    return (json.Deserialize<JToken>().First() as JProperty).Name;
    //}
}
