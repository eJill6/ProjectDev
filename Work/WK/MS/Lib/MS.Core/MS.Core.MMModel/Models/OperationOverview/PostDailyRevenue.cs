using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.OperationOverview
{
    public class PostDailyRevenue
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 广场解锁收益
        /// </summary>
        public decimal SquareUnlockRevenue { get; set; }

        /// <summary>
        /// 寻芳阁解锁收益
        /// </summary>
        public decimal AgencyUnlockRevenue { get; set; }

        /// <summary>
        /// 官方平台收益
        /// </summary>
        public decimal OfficialPlatformRevenue { get; set; }

        /// <summary>
        /// 总收益
        /// </summary>
        public decimal TotalRevenue { get; set; }

        /// <summary>
        /// 广场退款金额
        /// </summary>
        public decimal SquareRefundAmount { get; set; }

        /// <summary>
        /// 寻芳阁退款金额
        /// </summary>
        public decimal AgencyRefundAmount { get; set; }

        /// <summary>
        /// 官方退款金额
        /// </summary>
        public decimal OfficialRefundAmount { get; set; }

        /// <summary>
        /// 总退款
        /// </summary>
        public decimal TotalRefund { get; set; }
    }
}