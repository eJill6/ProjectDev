using JxBackendService.Model.Enums;
using System;

namespace JxBackendService.Common.Util
{
    public static class DecimalUtil
    {
        /// <summary>
        /// 無條件進位
        /// </summary>
        public static decimal Ceiling(this decimal value)
        {
            return Math.Ceiling(value);
        }

        /// <summary>
        /// 無條件捨去
        /// </summary>
        public static decimal Floor(this decimal value)
        {
            return Math.Floor(value);
        }

        public static decimal Floor(this decimal value, int digits)
        {
            decimal multiplier = 1;

            for (int i = 0; i < digits; i++)
            {
                multiplier *= 10;
            }

            return Math.Truncate(value * multiplier) / multiplier;
        }

        public static string ToNullableDecimalString(this decimal? nullableDecimal)
        {
            return nullableDecimal.ToNullableDecimalString("-");
        }

        /// <summary>
        /// 將有可能為Null的Decimal轉換為值或代表Null的字串
        /// </summary>
        /// <param name="nullableDecimal"></param>
        /// <param name="nullRepresentationString">代表Null的字串</param>
        /// <returns></returns>
        public static string ToNullableDecimalString(this decimal? nullableDecimal, string nullRepresentationString)
        {
            return nullableDecimal.HasValue ? nullableDecimal.Value.ToString() : nullRepresentationString;
        }

        public static BetResultType ToBetResultType(this decimal winMoney)
        {
            if (winMoney > 0)
            {
                return BetResultType.Win;
            }
            else if (winMoney < 0)
            {
                return BetResultType.Lose;
            }
            else
            {
                return BetResultType.Draw;
            }
        }
    }
}
