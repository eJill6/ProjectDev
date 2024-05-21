namespace MS.Core.Models.Models
{
    public static class GlobalSettings
    {
        /// <summary>
        /// 最大分頁數 (防止大量抓取)
        /// </summary>
        public static readonly int MaxPageSize = 50;

        /// <summary>
        /// 分頁大小
        /// </summary>
        public static readonly int PageSize = 20;

        /// <summary>
        /// 日期的格式
        /// </summary>
        public static string DateTimeFormat => "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 餘額的格式(到小數第二位)
        /// </summary>
        public static string AmountFormat => "#0.00";

        /// <summary>
        /// 餘額的格式(到小數第一位)
        /// </summary>
        public static string AmountFormatWithOne => "#0.0";

        /// <summary>
        /// 餘額的格式(不保留小数)
        /// </summary>
        public static string AmountToIntString => "#0";

    }
}