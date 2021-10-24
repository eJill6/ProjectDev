using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JxBackendService.Common.Util
{
    public static class MaskUtil
    {
        private static readonly string _maskToken = "*";

        /// <summary>
        /// 顯示前後三碼
        /// </summary>
        public static string ToMaskPhoneNumber(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            if (input.Length <= 3)
            {
                return input;
            }

            HashSet<int> visibleIndexList = new HashSet<int>();
            if (input.Length > 6)
            {
                new List<int> { 0, 1, 2,
                    input.Length - 3,
                    input.Length - 2,
                    input.Length - 1 }.ForEach(f => visibleIndexList.Add(f));
            }
            else
            {
                int visiblePartLength = visiblePartLength = (int)(Math.Floor(Convert.ToDouble(input.Length) / 3));

                for (int i = 0; i < visiblePartLength; i++)
                {
                    visibleIndexList.Add(i);
                }

                for (int i = input.Length - visiblePartLength; i < input.Length; i++)
                {
                    visibleIndexList.Add(i);
                }
            }

            return BaseMaskString(input, false, visibleIndexList);
        }

        /// <summary>
        /// 郵箱@前隱碼後三碼，三個字元內的，只顯示第一碼
        /// </summary>
        public static string ToMaskEmail(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            int foundIndex = input.IndexOf("@");

            if (foundIndex < 0)
            {
                return input;
            }

            string emailAccount = input.Substring(0, foundIndex);

            if (emailAccount.Length == 0)
            {
                throw new FormatException("email=" + input);
            }

            HashSet<int> maskIndexList = new HashSet<int>();

            if (emailAccount.Length <= 3)
            {
                if (emailAccount.Length == 1)
                {
                    maskIndexList.Add(0);
                }
                else
                {
                    for (int i = 1; i < emailAccount.Length; i++)
                    {
                        maskIndexList.Add(i);
                    }
                }
            }
            else
            {
                maskIndexList.Add(emailAccount.Length - 1);
                maskIndexList.Add(emailAccount.Length - 2);
                maskIndexList.Add(emailAccount.Length - 3);
            }

            return BaseMaskString(input, true, maskIndexList);
        }

        /// <summary>
        /// 卡號顯示前四後四碼，中間隱碼
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToMaskBankCardNo(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            HashSet<int> visibleIndexList = new HashSet<int>();

            if (input.Length > 8)
            {
                new List<int> { 0, 1, 2, 3,
                    input.Length - 4,
                    input.Length - 3,
                    input.Length - 2,
                    input.Length - 1 }.ForEach(f => visibleIndexList.Add(f));
            }
            else
            {
                int visiblePartLength = visiblePartLength = (int)(Math.Floor(Convert.ToDouble(input.Length) / 3));

                for (int i = 0; i < visiblePartLength; i++)
                {
                    visibleIndexList.Add(i);
                }

                for (int i = input.Length - visiblePartLength; i < input.Length; i++)
                {
                    visibleIndexList.Add(i);
                }
            }

            return BaseMaskString(input, false, visibleIndexList);
        }

        /// <summary>
        /// 卡號遮罩，只留最後四碼
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToMaskFrontSideBankCardNo(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            //先塞成 16 位
            input = CardNoMaskProcess(input);

            HashSet<int> visibleIndexList = new HashSet<int>();

            if (input.Length > 8)
            {
                new List<int> {
                    input.Length - 4,
                    input.Length - 3,
                    input.Length - 2,
                    input.Length - 1 }.ForEach(f => visibleIndexList.Add(f));
            }
            else
            {
                int visiblePartLength = visiblePartLength = (int)(Math.Floor(Convert.ToDouble(input.Length) / 3));

                for (int i = 0; i < visiblePartLength; i++)
                {
                    visibleIndexList.Add(i);
                }

                for (int i = input.Length - visiblePartLength; i < input.Length; i++)
                {
                    visibleIndexList.Add(i);
                }
            }

            var processedCardNo = BaseMaskString(input, false, visibleIndexList);

            return PadWithSpace(processedCardNo);
        }

        private static string PadWithSpace(string processedCardNo)
        {
            //追加空白 20200427 Thomas
            var numbers = new List<string>();

            var splitCahtCount = 4;
            for (int point = 0; point < processedCardNo.Length; point += 4)
            {
                var finalLength = processedCardNo.Length - point;

                if (finalLength < 4)
                {
                    splitCahtCount = finalLength;
                }

                numbers.Add(processedCardNo.Substring(point, splitCahtCount));
            }

            return string.Join(" ", numbers);
        }

        /// <summary>
        /// 卡號統一補足至 16 位
        /// </summary>
        /// <param name="cardNumber"></param>
        private static string CardNoMaskProcess(string cardNumber)
        {
            var targetLength = 16;
            var startIndex = 0;

            if (cardNumber.Length > targetLength)
            {
                startIndex = cardNumber.Length - targetLength;
            }

            if (cardNumber.Length < targetLength)
            {
                cardNumber = cardNumber.PadLeft(targetLength, '*');
            }

            return cardNumber.Substring(startIndex, targetLength);
        }

        /// <summary>
        /// 銀行卡名字遮罩，小於兩碼會給兩個*
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToMaskBankCardUserName(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            HashSet<int> visibleIndexList = new HashSet<int>();

            if (input.Length >= 2)
            {
                new List<int> {
                    input.Length - 1 }.ForEach(f => visibleIndexList.Add(f));
            }
            else
            {
                var targetLength = input.Length;

                if (targetLength < 2) targetLength = 2;

                int visiblePartLength = visiblePartLength = (int)(Math.Floor(Convert.ToDouble(targetLength) / 3));

                for (int i = 0; i < visiblePartLength; i++)
                {
                    visibleIndexList.Add(i);
                }

                for (int i = targetLength - visiblePartLength; i < targetLength; i++)
                {
                    visibleIndexList.Add(i);
                }
            }

            return BaseMaskString(input, false, visibleIndexList);
        }

        private static string BaseMaskString(string input, bool isMaskByIndex, HashSet<int> indexList)
        {
            StringBuilder maskBuilder = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                if (indexList.Contains(i))
                {
                    if (isMaskByIndex)
                    {
                        maskBuilder.Append(_maskToken);
                    }
                    else
                    {
                        maskBuilder.Append(input.Substring(i, 1));
                    }
                }
                else
                {
                    if (isMaskByIndex)
                    {
                        maskBuilder.Append(input.Substring(i, 1));
                    }
                    else
                    {
                        maskBuilder.Append(_maskToken);
                    }
                }
            }

            return maskBuilder.ToString();
        }

        public static string MaskOriginalDataContainBankCardContent(string content)
        {
            string maskContent = content;

            if (!string.IsNullOrWhiteSpace(maskContent))
            {
                string chkBankCardPattern = @"(卡号)(:|([\s]?=[\s]?))([0-9]+[\*]*[0-9]+)";

                MatchCollection matches = Regex.Matches(content, chkBankCardPattern);

                foreach (Match match in matches)
                {
                    string cardNo = match.Groups.Cast<Group>().Last().Value;
                    maskContent = maskContent.Replace(cardNo, cardNo.ToMaskBankCardNo());
                }
            }

            return maskContent;
        }
    }
}
