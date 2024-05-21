using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.Media.Enums
{
    public enum PostIncomeAggregateType
    {
        /// <summary>
        /// 廣場解鎖收益。
        /// </summary>
        SquareUnlockEarnings = 1,

        /// <summary>
        /// 擔保解鎖收益。
        /// </summary>
        AgencyUnlockEarnings = 2,

        /// <summary>
        /// 官方平台收益。
        /// </summary>
        OfficialPlatformEarnings = 3,

        /// <summary>
        /// 總收益。
        /// </summary>
        TotalRevenue = 4,

        /// <summary>
        /// 廣場退款金額。
        /// </summary>
        SquareRefundAmount = 5,

        /// <summary>
        /// 擔保退款金額。
        /// </summary>
        AgencyRefundAmount = 6,

        /// <summary>
        /// 官方退款金額。
        /// </summary>
        OfficialRefundAmount = 7,

        /// <summary>
        /// 總退款。
        /// </summary>
        TotalRefund = 9
    }
}
