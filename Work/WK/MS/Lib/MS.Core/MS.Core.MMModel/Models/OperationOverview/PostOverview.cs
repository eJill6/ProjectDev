using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.OperationOverview
{
    public class PostOverview
    {
        /// <summary>
        /// 展示中帖子总数
        /// </summary>
        public int TotalPost { get; set; }

        /// <summary>
        /// 广场展示中
        /// </summary>
        public int SquareDisplayed { get; set; }

        /// <summary>
        /// 广场审核中
        /// </summary>
        public int SquareUnderReview { get; set; }

        /// <summary>
        /// 广场未通过
        /// </summary>
        public int SquareNotApproved { get; set; }

        /// <summary>
        /// 广场被解锁次数
        /// </summary>
        public int SquareUnlockedCount { get; set; }

        /// <summary>
        /// 寻芳阁展示中
        /// </summary>
        public int AgencyDisplayed { get; set; }

        /// <summary>
        /// 寻芳阁审核中
        /// </summary>
        public int AgencyUnderReview { get; set; }

        /// <summary>
        /// 寻芳阁未通过
        /// </summary>
        public int AgencyNotApproved { get; set; }

        /// <summary>
        /// 寻芳阁被解锁次数
        /// </summary>
        public int AgencyUnlockedCount { get; set; }

        /// <summary>
        /// 官方展示中
        /// </summary>
        public int OfficialDisplayed { get; set; }

        /// <summary>
        /// 官方审核中
        /// </summary>
        public int OfficialUnderReview { get; set; }

        /// <summary>
        /// 官方未通过
        /// </summary>
        public int OfficialNotApproved { get; set; }

        /// <summary>
        /// 官方被预约次数
        /// </summary>
        public int OfficialReservedCount { get; set; }

        /// <summary>
        /// 官方被预约取消次数
        /// </summary>
        public int OfficialReserveCancelCount { get; set; }
    }
}