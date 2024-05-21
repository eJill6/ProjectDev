namespace MS.Core.MM.Infrastructures.Extensions
{
    /// <summary>
    /// 時間擴充類型
    /// </summary>
    public static class MMDateTimeExtension
    {
        /// <summary>
        /// 顯示更新時間的文字
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToMMUpdateString(this DateTime dateTime)
        {
            if (dateTime == default(DateTime))
            {
                return string.Empty;
            }

            var diffTimes = DateTime.Now.Subtract((DateTime)dateTime);

            if (diffTimes.TotalMinutes < 60)
            {
                return $"{Math.Floor(diffTimes.TotalMinutes)}分前更新";
            }
            else if (diffTimes.TotalHours < 24)
            {
                return $"{Math.Floor(diffTimes.TotalHours)}小时前更新";
            }

            return $"{diffTimes.Days}天前更新";
        }
    }
}