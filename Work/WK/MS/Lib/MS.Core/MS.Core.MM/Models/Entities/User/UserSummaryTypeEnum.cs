namespace MS.Core.MM.Models.Entities.User
{
    public enum UserSummaryTypeEnum
    {
        /// <summary>
        /// 1.發贴次數
        /// </summary>
        Send = 1,

        /// <summary>
        /// 2.解鎖贴次數
        /// </summary>
        UnLock = 2,

        /// <summary>
        /// 3.免費解鎖次數
        /// </summary>
        FreeUnlock = 3,

        /// <summary>
        /// 4.被解鎖次數
        /// </summary>
        Unlocked = 4,

        /// <summary>
        /// 5.評論次數
        /// </summary>
        Comment = 5,

        /// <summary>
        /// 6.累積收益(已領取)
        /// </summary>
        Income = 6,

        /// <summary>
        /// 7.預約次數
        /// </summary>
        BookingCount = 7,

        /// <summary>
        /// 8.被預約次數
        /// </summary>
        BookedCount = 8,
        
        /// <summary>
        /// 9.收藏数
        /// </summary>
        FavoriteCount=9,
    }
}