namespace MS.Core.Extensions
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 回傳Unix Time
        /// </summary>
        public static long ToUnixOfTime(this DateTime dateTime, UnixOfTimeTypes unixOfTimeType = UnixOfTimeTypes.TotalMilliseconds)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan timeSpan = dateTime - startTime;

            switch (unixOfTimeType)
            {
                case UnixOfTimeTypes.TotalMilliseconds:
                    return (long)timeSpan.TotalMilliseconds;

                case UnixOfTimeTypes.TotalSeconds:
                    return (long)timeSpan.TotalSeconds;
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// 將timestamp轉成日期
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="unixOfTimeType"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static DateTime ConvertToDateTime(this double ts, UnixOfTimeTypes unixOfTimeType = UnixOfTimeTypes.TotalMilliseconds)
        {
            switch (unixOfTimeType)
            {
                case UnixOfTimeTypes.TotalMilliseconds:
                    return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddMilliseconds(ts);

                case UnixOfTimeTypes.TotalSeconds:
                    return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(ts);
            }

            throw new NotSupportedException();
        }

        public static DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
    }

    public enum UnixOfTimeTypes
    {
        TotalMilliseconds,

        TotalSeconds
    }
}