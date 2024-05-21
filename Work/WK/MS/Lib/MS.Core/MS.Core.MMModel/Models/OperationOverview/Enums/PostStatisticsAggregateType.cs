using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.Media.Enums
{
    public enum PostStatisticsAggregateType
    {
    /// <summary>
    /// 展示中帖子總數。
    /// </summary>
    ShowTotalPosts = 1,

    /// <summary>
    /// 廣場展示中。
    /// </summary>
    SquareShow = 2,

    /// <summary>
    /// 廣場審核中。
    /// </summary>
    SquareUnderReview = 3,

    /// <summary>
    /// 廣場未通過。
    /// </summary>
    SquareNotApproved = 4,

    /// <summary>
    /// 廣場被解鎖次數。
    /// </summary>
    SquareUnlocksCount = 5,

    /// <summary>
    /// 擔保展示中。
    /// </summary>
    AgencyShow = 6,

    /// <summary>
    /// 擔保審核中。
    /// </summary>
    AgencyUnderReview = 7,

    /// <summary>
    /// 擔保未通過。
    /// </summary>
    AgencyNotApproved = 8,

    /// <summary>
    /// 擔保被解鎖次數。
    /// </summary>
    AgencyUnlocksCount = 9,

    /// <summary>
    /// 官方展示中。
    /// </summary>
    OfficialShow = 10,

    /// <summary>
    /// 官方審核中。
    /// </summary>
    OfficialUnderReview = 11,

    /// <summary>
    /// 官方未通過。
    /// </summary>
    OfficialNotApproved = 12,

    /// <summary>
    /// 官方被預約次數。
    /// </summary>
    OfficialReservationsCount = 13
}
}