using System;

namespace MS.Core.MMModel.Models.My.Enums
{
    public enum MyPostSortType
    {
        /// <summary>
        /// 時間升冪
        /// </summary>
        CreateDateAsc = 0,

        /// <summary>
        /// 時間降冪
        /// </summary>
        CreateDateDesc = 1,

        /// <summary>
        /// 解鎖次數/預約次數 升冪
        /// </summary>
        UolockCountAsc = 2,

        /// <summary>
        /// 解鎖次數/預約次數 降冪
        /// </summary>
        UolockCountDesc = 3,
    }
}