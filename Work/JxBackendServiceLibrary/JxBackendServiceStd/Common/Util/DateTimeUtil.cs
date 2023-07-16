using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace JxBackendService.Common.Util
{
    public static class DateTimeUtil
    {
        public static int DaysInOneWeek = Enum.GetNames(typeof(DayOfWeek)).Length;

        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public static string ToFormatDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// yyyyMM
        /// </summary>
        public static string ToFormatYearMonthValue(this DateTime dateTime)
        {
            return dateTime.ToString("yyyyMM");
        }

        /// <summary>
        /// yyyy-MM
        /// </summary>
        public static string ToFormatYearMonthText(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM");
        }

        /// <summary>
        /// MMdd
        /// </summary>
        public static string ToFormatMonthDayValue(this DateTime dateTime)
        {
            return dateTime.ToString("MMdd");
        }

        /// <summary>
        /// yyMMdd
        /// </summary>
        public static string ToFormatLastYearMonthDateValue(this DateTime dateTime)
        {
            return dateTime.ToString("yyMMdd");
        }

        /// <summary>
        /// yyyyMMdd
        /// </summary>
        public static string ToFormatYearMonthDateValue(this DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMdd");
        }

        public static string ToFormatDateString(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToFormatDateString();
            }

            return string.Empty;
        }

        /// <summary> yyyy-MM-dd HH:mm:ss.fffffff </summary>
        public static string ToFormatDateTimeMillisecondsString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
        }

        /// <summary> yyyy-MM-dd HH:mm:ss </summary>
        public static string ToFormatDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary> MM-dd HH:mm </summary>
        public static string ToFormatMonthDateString(this DateTime dateTime)
        {
            return dateTime.ToString("MM-dd HH:mm");
        }

        public static string ToFormatDateTimeString(this DateTime? dateTime)
        {
            return dateTime.ToFormatDateTimeString("-");
        }

        public static string ToFormatDateTimeString(this DateTime? dateTime, string nullRepresentationString)
        {
            if (!dateTime.HasValue)
            {
                return nullRepresentationString; //對應原本前端的FallbackValue
            }
            else
            {
                return dateTime.Value.ToFormatDateTimeString();
            }
        }

        public static string ToLatestLoginDateTimeString(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToString("MM/dd HH:mm:ss");
            }
            return string.Empty;
        }

        public static string ToFormatDateTimeStringWithoutSecond(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy/MM/dd HH:mm");
        }

        /// <summary> yyyyMMddHHmmss </summary>
        public static string ToFormatDateTimeStringWithoutSymbol(this DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMddHHmmss");
        }

        public static string ToFormatDateTimeStringWithoutSecond(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToFormatDateTimeStringWithoutSecond();
            }

            return string.Empty;
        }

        public static string ToFormatTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("mm:ss");
        }

        public static DateTime ToQueryStartDate(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        //public static DateTime ToQueryEndDate(this DateTime dateTime)
        //{
        //    //SQL Server 中 datetime 精度只有 3.33 毫秒 故-3避免進位
        //    return dateTime.Date.AddDays(1).AddMilliseconds(-3);
        //}

        //public static DateTime? ToQueryEndDate(this DateTime? dateTime)
        //{
        //    if (dateTime.HasValue)
        //    {
        //        return dateTime.Value.ToQueryEndDate();
        //    }

        //    return null;
        //}

        private static DateTime ConvertToQueryEndTime(this DateTime dateTime, DatePeriods minDateUnit, bool isEqualThanOperator)
        {
            DateTime resultTime = dateTime;

            switch (minDateUnit)
            {
                case DatePeriods.Second:
                    resultTime = resultTime.AddMilliseconds(-dateTime.Millisecond);
                    resultTime = resultTime.AddSeconds(1);
                    break;

                case DatePeriods.Minute:
                    resultTime = DateTime.Parse($"{resultTime.ToString("yyyy-MM-dd")} {resultTime.Hour}:{resultTime.Minute}:00");
                    resultTime = resultTime.AddMinutes(1);
                    break;

                //case DatePeriods.Hour:
                //    resultTime = DateTime.Parse($"{resultTime.ToString("yyyy-MM-dd")} {resultTime.Hour}:00:00");
                //    resultTime = resultTime.AddHours(1);
                //    break;

                case DatePeriods.Day:
                    resultTime = resultTime.Date.AddDays(1);
                    break;

                case DatePeriods.Month:
                    resultTime = DateTime.Parse(resultTime.ToString("yyyy-MM-01"));
                    resultTime = resultTime.AddMonths(1);
                    break;

                case DatePeriods.Year:
                    resultTime = DateTime.Parse(resultTime.ToString("yyyy-01-01"));
                    resultTime = resultTime.AddYears(1);
                    break;
            }

            if (isEqualThanOperator)
            {
                resultTime = resultTime.AddMilliseconds(-3);
            }

            return resultTime;
        }

        /// <summary>
        /// 查詢小於結尾的時間
        /// </summary>
        public static DateTime ToQuerySmallThanTime(this DateTime dateTime, DatePeriods minDateUnit)
        {
            return ConvertToQueryEndTime(dateTime, minDateUnit, false);
        }

        /// <summary>
        /// 查詢小於結尾的時間
        /// </summary>
        public static DateTime? ToQuerySmallThanTime(this DateTime? dateTime, DatePeriods minDateUnit)
        {
            if (dateTime.HasValue)
            {
                return ToQuerySmallThanTime(dateTime.Value, minDateUnit);
            }

            return dateTime;
        }

        /// <summary>
        /// 查詢小於等於結尾的時間
        /// </summary>
        public static DateTime ToQuerySmallEqualThanTime(this DateTime dateTime, DatePeriods minDateUnit)
        {
            return ConvertToQueryEndTime(dateTime, minDateUnit, true);
            //return dateTime.ToQuerySmallThanTime().AddMilliseconds(-3);
        }

        /// <summary>
        /// 查詢小於等於結尾的時間
        /// </summary>
        public static DateTime? ToQuerySmallEqualThanTime(this DateTime? dateTime, DatePeriods minDateUnit)
        {
            if (dateTime.HasValue)
            {
                return ToQuerySmallEqualThanTime(dateTime.Value, minDateUnit);
            }

            return dateTime;
        }

        /// <summary>
        /// 開始時間往後取30天結束時間
        /// 查詢小於等於結尾的時間
        /// </summary>
        /// <param name="startDateTime"></param>
        /// <returns></returns>
        public static DateTime ToPeriod30DaysQuerySmallEqualThanTime(this DateTime startDateTime)
        {
            DateTime endDateTime = startDateTime.AddDays(30).ToQuerySmallEqualThanTime(DatePeriods.Day);

            if (endDateTime > DateTime.Now)
            {
                endDateTime = DateTime.Now.ToQuerySmallEqualThanTime(DatePeriods.Day);
            }

            return endDateTime;
        }

        public static bool IsDateTimeWithinPeriod(this DateTime startDate, DateTime? endDate, DatePeriods datePeriod, int amount)
        {
            return IsBaseDateTimeWithinPeriod(startDate, endDate, datePeriod, amount);
        }

        /// <summary>
        /// 檢查日期是否超過限制的日數 例如 起訖日期 日否超過 days
        /// </summary>
        /// <returns>true 在範圍內 false 超過日數</returns>
        public static bool IsDateTimeWithinPeriod(this DateTime? startDate, DateTime? endDate, DatePeriods datePeriod, int amount)
        {
            return IsBaseDateTimeWithinPeriod(startDate, endDate, datePeriod, amount);
        }

        public static bool IsDateTimeWithinPeriod(this DateTime startDate, DatePeriods datePeriod, int amount)
        {
            return IsBaseDateTimeWithinPeriod(startDate, DateTime.Now, datePeriod, amount);
        }

        /// <summary>
        /// 檢查日期是否超過限制的日數 例如 查詢起始日期是否超過 days
        /// </summary>
        public static bool IsDateTimeWithinPeriod(this DateTime? startDate, DatePeriods datePeriod, int amount)
        {
            return IsBaseDateTimeWithinPeriod(startDate, DateTime.Now, datePeriod, amount);
        }

        private static bool IsBaseDateTimeWithinPeriod(DateTime? startDate, DateTime? endDate, DatePeriods datePeriod, int amount)
        {
            if (startDate == null || endDate == null)
            {
                return false;
            }
            if (startDate > endDate)
            {
                return false;
            }

            DateTime minDate = endDate.Value;

            switch (datePeriod)
            {
                case DatePeriods.Year:
                    minDate = endDate.Value.Date.AddYears(-amount);
                    break;

                case DatePeriods.Month:
                    minDate = endDate.Value.Date.AddMonths(-amount);
                    break;

                case DatePeriods.Day:
                    minDate = endDate.Value.Date.AddDays(-amount);
                    break;

                //case DatePeriods.Hour:
                //    minDate = endDate.Value.Date.AddHours(-amount);
                //    break;

                case DatePeriods.Minute:
                    minDate = endDate.Value.Date.AddMinutes(-amount);
                    break;

                case DatePeriods.Second:
                    minDate = endDate.Value.Date.AddSeconds(-amount);
                    break;
            }

            if (startDate.Value.Date < minDate)
            {
                return false;
            }

            return true;
        }

        public static DateTime ToStartOfWeek(this DateTime dateTime, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            int daysFromStart = (DaysInOneWeek + (dateTime.DayOfWeek - startOfWeek)) % DaysInOneWeek;

            return dateTime.AddDays(-1 * daysFromStart).Date;
        }

        /// <summary>
        /// 取得當月第一天 不含時分秒
        /// </summary>
        /// <returns></returns>
        public static DateTime GetThisMonthFirstDay()
        {
            return DateTime.Now.AddDays(-DateTime.Now.Day + 1).Date;
        }

        /// <summary>
        /// 取得昨天 00:00
        /// </summary>
        /// <returns></returns>
        public static DateTime GetYesterday()
        {
            return DateTime.Now.AddDays(-1).Date;
        }

        /// <summary>
        /// 取得下一天 00:00
        /// </summary>
        /// <returns></returns>
        public static DateTime GetNextDay()
        {
            return DateTime.Now.AddDays(1).Date;
        }

        /// <summary>
        /// 取得七天前 日期 00:00
        /// </summary>
        public static DateTime GetOneWeekAgoDay()
        {
            return DateTime.Now.AddDays(-6).Date;
        }

        /// <summary>
        /// 取得今天 00:00
        /// </summary>
        /// <returns></returns>
        public static DateTime GetToday()
        {
            return DateTime.Now.Date;
        }

        //public static string ToMaximumTimeUnitText(this DateTime endTime, DateTime? startTime)
        //{
        //    return (endTime as DateTime?).ToMaximumTimeUnitText(startTime);
        //}

        //public static string ToMaximumTimeUnitText(this DateTime? endTime, DateTime? startTime)
        //{
        //    double? sourceSeconds = null;

        //    if (endTime.HasValue && startTime.HasValue)
        //    {
        //        sourceSeconds = endTime.Value.Subtract(startTime.Value).TotalSeconds;
        //    }

        //    if (!sourceSeconds.HasValue)
        //    {
        //        return string.Empty;
        //    }

        //    //天, 小時, 分鐘, 秒
        //    List<double> timeUnitLimitList = new List<double>() {
        //        //365d * 24d * 60d, //1年
        //        //30d * 24d * 60d, //1月
        //        //24d * 60d * 60d, //1天
        //        //60d * 60d, //1小時
        //        60d, //1分鐘
        //        1 //秒
        //    };

        //    //List<string> timeUnitTextList = new List<string>() { MsgElement.Day, MsgElement.Hour, MsgElement.Minute, MsgElement.Second };
        //    List<string> timeUnitTextList = new List<string>() { UnitElement.Minute, UnitElement.Second };
        //    string finallyResult = null;

        //    for (int i = 0; i < timeUnitLimitList.Count; i++)
        //    {
        //        int timeNum = (int)Math.Floor(sourceSeconds.Value / timeUnitLimitList[i]);

        //        if (timeNum > 0)
        //        {
        //            finallyResult += timeNum.ToString() + timeUnitTextList[i];
        //            sourceSeconds -= timeNum * timeUnitLimitList[i];
        //        }

        //        if (sourceSeconds == 0)
        //        {
        //            break;
        //        }
        //    }

        //    return finallyResult;
        //}

        /// <summary>
        /// 算時間差
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="subDateTime"></param>
        /// <returns></returns>
        public static DateTime DiffTimes(this DateTime dateTime, DateTime subDateTime)
        {
            return (dateTime as DateTime?).DiffTimes((subDateTime as DateTime?));
        }

        /// <summary>
        /// 算時間差
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="subDateTime"></param>
        /// <returns></returns>
        public static DateTime DiffTimes(this DateTime? dateTime, DateTime? subDateTime)
        {
            return new DateTime(dateTime.Value.Subtract(subDateTime.Value).Ticks);
        }

        public static DateTime AddTimes(this DateTime dateTime, DatePeriods datePeriod, int amount)
        {
            switch (datePeriod)
            {
                case DatePeriods.Year:
                    return dateTime.Date.AddYears(amount);

                case DatePeriods.Month:
                    return dateTime.Date.AddMonths(amount);

                case DatePeriods.Day:
                    return dateTime.Date.AddDays(amount);

                //case DatePeriods.Hour:
                //    return dateTime.Date.AddHours(amount);

                case DatePeriods.Minute:
                    return dateTime.Date.AddMinutes(amount);

                    //case DatePeriods.Second:
                    //    return dateTime.Date.AddSeconds(amount);
            }

            throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// 回傳Unix Time
        /// </summary>
        public static long ToUnixOfTime(this DateTime dateTime, UnixOfTimeTypes unixOfTimeType = UnixOfTimeTypes.TotalMilliseconds)
        {
            dateTime = dateTime.ToUniversalTime();
            DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);            
            TimeSpan timeSpan = dateTime.Subtract(startTime);

            switch (unixOfTimeType)
            {
                case UnixOfTimeTypes.TotalMilliseconds:
                    return (long)timeSpan.TotalMilliseconds;

                case UnixOfTimeTypes.TotalSeconds:
                    return (long)timeSpan.TotalSeconds;
            }

            throw new NotSupportedException();
        }

        public static DateTime ToDateTime(this long unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static DateTime ToDateTimeSeconds(this long unixTimeStamp)
        {
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return unixEpoch.AddSeconds(unixTimeStamp).ToLocalTime();
        }

        /// <summary>
        /// 傳入 字串時間格式 轉換成 DateTime
        /// </summary>
        public static DateTime ToDateTime(this string dateTimeString, string format = "yyyyMMdd")
        {
            if (dateTimeString.IsNullOrEmpty())
            {
                return DateTime.Now;
            }

            return DateTime.ParseExact(dateTimeString, format, provider: null, DateTimeStyles.AllowWhiteSpaces);
        }

        public static DateTime GetChinaDateTime()
        {
            return DateTime.UtcNow.AddHours(8);
        }

        public static DateTime ToChinaDateTime(this DateTime utcDateTime)
        {
            return utcDateTime.AddHours(8);
        }

        public static string ConvertToMaximumTimeUnitText(double? sourceSeconds)
        {
            //天, 小時, 分鐘, 秒
            List<double> timeUnitLimitList = new List<double>() {
                //365d * 24d * 60d, //1年
                //30d * 24d * 60d, //1月
                24d * 60d * 60d, //1天
                60d * 60d, //1小時
                60d, //1分鐘
                1 //秒
            };

            List<string> timeUnitTextList = new List<string>() { MessageElement.Day, MessageElement.Hour, MessageElement.Minute, MessageElement.Second };

            if (!sourceSeconds.HasValue)
            {
                return string.Empty;
            }

            string finallyResult = null;

            for (int i = 0; i < timeUnitLimitList.Count; i++)
            {
                int timeNum = (int)Math.Floor(sourceSeconds.Value / timeUnitLimitList[i]);

                if (timeNum > 0)
                {
                    finallyResult += timeNum + timeUnitTextList[i];
                    sourceSeconds -= timeNum * timeUnitLimitList[i];
                }

                if (sourceSeconds == 0)
                {
                    break;
                }
            }

            return finallyResult;
        }

        public static bool IsTimeExpired(this DateTime? dateTime)
        {
            //沒資料代表沒過期
            if (dateTime.HasValue && dateTime.Value < DateTime.Now)
            {
                return true;
            }

            return false;
        }

        public static string ConvertToJavaScriptTimeSpan(this DateTime? dateTime)
        {
            var utcDefaultTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            if (dateTime.HasValue)
            {
                var time = dateTime.Value.ToUniversalTime().Subtract(utcDefaultTime);

                return $"/Date({time.Ticks / 10000})/";
            }

            return null;
        }

        /// <summary>
        /// 比较两个时间相差几个小时
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static double DiffHours(DateTime startTime, DateTime endTime)
        {
            TimeSpan hoursSpan = new TimeSpan(endTime.Ticks - startTime.Ticks);
            return hoursSpan.TotalHours;
        }
    }

    public enum DatePeriods
    {
        Year,

        Month,

        Day,

        //Hour,
        Minute,

        Second,
    }

    public enum UnixOfTimeTypes
    {
        TotalMilliseconds,

        TotalSeconds
    }
}