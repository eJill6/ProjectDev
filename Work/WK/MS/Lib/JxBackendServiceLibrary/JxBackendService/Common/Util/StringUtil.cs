using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JxBackendService.Common.Extensions;
using JxBackendService.Model.Enums;
using JxBackendService.Resource.Element;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JxBackendService.Common.Util
{
    public static class StringUtil
    {
        private static readonly string s_intThousandCommaFormat = "{0:N0}";  // EX: 1,230

        private static readonly string s_amountFormat = "{0:N2}";     // EX: 1,230.36

        private static readonly string s_amountFormatWithoutZeroAfterPoint = "#,0.##";     // EX: 1,230.00->1,230

        private static readonly string s_mobileApiPlayInfoAmountFormat = "#,0.00##";     // EX: 1,230.1200->1,230.12; 1,230.1234->1,230.1234

        private static readonly string s_thousandCommaFormat = "{0:N4}";     // EX: 1,230.3600

        private static readonly string s_amountWithoutThousandthCommaFormat = "{0:F2}"; // EX: 1230.36

        private static readonly string s_mobileApiProfitLossNumberFormat = "{0:F4}"; // EX: 1230.3600

        private static readonly decimal s_defaultValue = 0;

        private static readonly string s_nullableString = "-";

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

                return Convert.ToInt32(s_defaultValue);
            }
            else
            {
                return Convert.ToInt32(data);
            }
        }

        public static int ToInt32(this bool data) => Convert.ToInt32(data);

        public static int ToInt32(this decimal data) => Convert.ToInt32(data);

        public static int? ToInt32Nullable(this string data)
        {
            if (int.TryParse(data, out int inputNumber))
            {
                return inputNumber;
            }

            return null;
        }

        public static long ToInt64(this string data, bool hasDefaultValue = false)
        {
            if (hasDefaultValue)
            {
                if (long.TryParse(data, out long inputNumber))
                {
                    return inputNumber;
                }
                return Convert.ToInt64(s_defaultValue);
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
                    return s_defaultValue;
                }

                if (decimal.TryParse(data, out decimal inputNumber))
                {
                    return inputNumber;
                }

                return s_defaultValue;
            }
            else
            {
                return decimal.Parse(data);
            }
        }

        public static decimal? ToDecimalNullable(this string data)
        {
            if (decimal.TryParse(data, out decimal result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// 轉為貨幣格式 EX: 10,000.0000
        /// </summary>
        /// <param name="number">數值資料</param>
        /// <param name="hasThousandComma">是否加上千分位</param>
        public static string ToCurrency(this decimal number, bool hasThousandComma = true)
        {
            if (!hasThousandComma)
            {
                return number.ToString("0.0000");
            }
            else
            {
                return ToBaseFormatString(number, s_thousandCommaFormat);
            }
        }

        /// <summary> 紅利金 (四捨五入到小數點第二位) </summary>
        public static string ToDisplayBonusAmount(this decimal number)
        {
            return number.ToBonusAmount().ToAmountString();
        }

        /// <summary> 紅利金 (四捨五入到小數點第二位) </summary>
        public static decimal ToBonusAmount(this decimal number)
        {
            return Math.Round(number, 2);
        }

        /// <summary> 保證金 (四捨五入到小數點第二位) </summary>
        public static decimal ToWarrantAmount(this decimal number)
        {
            return Math.Round(number, 2);
        }

        /// <summary> 轉換成百分比 </summary>
        public static string ToPercentText(this decimal? number)
        {
            return number.GetValueOrDefault().ToPercentText();
        }

        public static string ToPercentText(this decimal number)
        {
            return number.ToString("0.####%");
        }

        /// <summary> 轉換成百分比四捨五入到小數點第二位 ex: 0.12356 => 12.36% </summary>
        public static string ToRebatePercentFormat(this decimal? number)
        {
            return number.GetValueOrDefault().ToRebatePercentFormat();
        }

        public static string ToRebatePercentFormat(this decimal number)
        {
            return number.ToString("P2");
        }

        public static string ToRebatePercentWithoutSymbolFormat(this decimal? number)
        {
            return number.GetValueOrDefault().ToRebatePercentWithoutSymbolFormat();
        }

        public static string ToRebatePercentWithoutSymbolFormat(this decimal number)
        {
            return number.ToRebatePercentFormat().Replace("%", "");
        }

        public static string ToCurrency(this decimal? number, bool hasThousandComma = true, bool isToNullableDecimalString = false)
        {
            if (isToNullableDecimalString && !number.HasValue)
            {
                return number.ToNullableDecimalString();
            }

            return number.GetValueOrDefault().ToCurrency(hasThousandComma);
        }

        public static string ToIntCurrency(this decimal? number, bool hasThousandComma = true)
        {
            return ToIntCurrency(number.GetValueOrDefault(), hasThousandComma);
        }

        public static string ToIntWithThousandComma(this int? number, bool isToNullableString = false)
        {
            if (isToNullableString && !number.HasValue)
            {
                return s_nullableString;
            }

            return number.GetValueOrDefault().ToIntWithThousandComma();
        }

        public static string ToIntWithThousandComma(this int number)
        {
            return ToBaseFormatString(number, s_intThousandCommaFormat);
        }

        public static string ToIntCurrency(this decimal number, bool hasThousandComma = true)
        {
            if (!hasThousandComma)
            {
                return number.ToString("0");
            }
            else
            {
                return ToBaseFormatString(number, s_intThousandCommaFormat);
            }
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

        private static string ToBaseToString<T>(this T source, string format)
        {
            string inputString = string.Empty;

            if (source != null)
            {
                inputString = source.ToString();
            }

            if (!inputString.IsNullOrEmpty() && decimal.TryParse(inputString, out decimal inputNumber))
            {
                return inputNumber.ToString(format);
            }
            else
            {
                return 0.ToString(format);
            }
        }

        /// <summary> EX:56,789.00 </summary>
        public static string ToAmountString<T>(this T source, bool isRemoveZeroAfterPoint = false, bool isToNullableString = false)
        {
            if (isToNullableString && source == null)
            {
                return s_nullableString;
            }

            if (isRemoveZeroAfterPoint)
            {
                return source.ToBaseToString(s_amountFormatWithoutZeroAfterPoint);
            }
            else
            {
                return source.ToBaseFormatString(s_amountFormat);
            }
        }

        public static string ToMobileApiAmountString<T>(this T source) => source.ToBaseToString(s_mobileApiPlayInfoAmountFormat);

        public static string ToAmountWithoutThousandthCommaString<T>(this T source)
        {
            return source.ToBaseFormatString(s_amountWithoutThousandthCommaFormat);
        }

        /// <summary> EX:1230.3600 </summary>
        public static string ToApiProfitLossNumberString<T>(this T source)
        {
            return source.ToBaseFormatString(s_mobileApiProfitLossNumberFormat);
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
            string[] symbols = { "!", "#", "$" }; // 只留下這幾個, 因為其他特殊符號会造成混謠
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

        public static string CreateRandomLoginPassword()
        {
            // 6碼，小寫英文(1碼)+數字(5碼) 混合, 單一數字至多【3碼】可相同
            return CreateRandomPassword(5, 1, 3, 1);
        }

        public static string CreateRandomMoneyPassword()
        {
            // 10碼，小寫英文(2碼)+數字(8碼) 混合, 單一數字至多【4碼】可相同
            return CreateRandomPassword(8, 2, 4, 2);
        }

        /// <summary>
        /// 功能：產生隨機密碼
        /// </summary>
        /// <param name="numberLength">取數字位數</param>
        /// <param name="letterLength">取小寫字母位數</param>
        /// <param name="numberMaxDuplicate">單個數字可重複次數</param>
        /// <param name="letterMaxDuplicate">單個字母可重複次數</param>
        /// <returns></returns>
        private static string CreateRandomPassword(int numberLength, int letterLength, int numberMaxDuplicate, int letterMaxDuplicate)
        {
            List<string> numbers = new List<string> { "2", "3", "4", "5", "6", "7", "8", "9" }; // 排除 "0", "1" 避免混淆
            List<string> letters = new List<string> { "a", "b", "c", "d", "e", "f", "g", "h",
                "i", "j", "k", "m", "n", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" }; // 排除 "l", "o" 避免混淆

            Random random = new Random(unchecked((int)DateTime.Now.Ticks));
            StringBuilder randomCode = new StringBuilder();

            void BuildRandomCode(List<string> elements, int maxDuplicate, int elementTakeCount)
            {
                List<string> duplicateElements = new List<string>();

                for (int i = 0; i < maxDuplicate; i++)
                {
                    duplicateElements.AddRange(elements);
                }

                if (duplicateElements.Count < elementTakeCount) // 理論上不會發生
                {
                    throw new Exception($"可取用的元素數({duplicateElements.Count})小於需要的元素數({elementTakeCount})");
                }

                for (int i = 0; i < elementTakeCount; i++)
                {
                    int index = random.Next(duplicateElements.Count);
                    randomCode.Append(duplicateElements[index]);
                    duplicateElements.RemoveAt(index);
                }
            }

            BuildRandomCode(numbers, numberMaxDuplicate, numberLength);
            BuildRandomCode(letters, letterMaxDuplicate, letterLength);

            return randomCode.ToString().Scramble();
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

        private static string Scramble(this string s)
        {
            return new string(s.ToCharArray().OrderBy(x => Guid.NewGuid()).ToArray());
        }

        public static string ToBase64String(this string value) => Convert.ToBase64String(Encoding.UTF8.GetBytes(value));

        public static string FromBase64String(this string value) => Encoding.UTF8.GetString(Convert.FromBase64String(value));

        public static string GetActionName(this bool isActive)
        {
            return isActive.GetActionText();
        }

        public static string ToCamelCase(this string str)
        {
            if (str.IsNullOrEmpty())
            {
                return str;
            }

            var tempDic = new Dictionary<string, string>
            {
                { str, null }
            };

            string json = tempDic.ToJsonString(ignoreNull: false, ignoreDefault: false, isCamelCaseNaming: true);

            return (json.Deserialize<JToken>().First() as JProperty).Name;
        }
    }
}