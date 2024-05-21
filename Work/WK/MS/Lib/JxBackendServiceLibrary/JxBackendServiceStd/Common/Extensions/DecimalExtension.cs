using JxBackendService.Common.Util;

namespace JxBackendService.Common.Extensions
{
    public static class DecimalExtension
    {
        private static readonly string _thousandCommaFormat = "{0:N2}";     // EX: 1,230.3600

        /// <summary>
        /// 轉為貨幣格式 EX: 10,000.0000
        /// </summary>
        /// <param name="number">數值資料</param>
        /// <param name="hasThousandComma">是否加上千分位</param>
        public static string ToCurrency(this decimal number, bool hasThousandComma = false)
        {
            if (!hasThousandComma)
            {
                return number.ToString("0.00");
            }
            else
            {
                return ToBaseFormatString(number, _thousandCommaFormat);
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
    }
}