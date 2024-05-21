using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.OperationOverview
{
    public class OperationOverview
    {
        /// <summary>
        /// 累计用户数
        /// </summary>
        public int TotalUsers { get; set; }

        /// <summary>
        /// 付费用户数
        /// </summary>
        public int PaidUsersCount { get; set; }

        /// <summary>
        /// 付费率
        /// </summary>
        public decimal PaymentRate { get; set; }

        /// <summary>
        /// 总收益
        /// </summary>
        public decimal TotalRevenue { get; set; }

        /// <summary>
        /// ARPU
        /// </summary>
        /// 总收益 / 累计用户数
        public decimal ARPU { get; set; }

        /// <summary>
        /// ARPPU
        /// </summary>
        /// 总收益 / 付费用户数
        public decimal ARPPU { get; set; }

        /// <summary>
        /// 已收保证金金额
        /// </summary>
        public decimal DepositsReceivedAmount { get; set; }

        /// <summary>
        /// 已收保证金笔数
        /// </summary>
        public int DepositsReceivedCount { get; set; }

        /// <summary>
        /// 商户数
        /// </summary>
        public int MerchantCount { get; set; }

        /// <summary>
        /// 广场区解锁收益
        /// </summary>
        public decimal SquareUnlockEarnings { get; set; }

        /// <summary>
        /// 广场区退款金额
        /// </summary>
        public decimal SquareRefundAmount { get; set; }

        /// <summary>
        /// 广场区退款笔数
        /// </summary>
        public int SquareRefundCount { get; set; }

        /// <summary>
        /// 寻芳阁解锁收益
        /// </summary>
        public decimal AgencyUnlockEarnings { get; set; }

        /// <summary>
        /// 寻芳阁退款金额
        /// </summary>
        public decimal AgencyRefundAmount { get; set; }

        /// <summary>
        /// 寻芳阁退款笔数
        /// </summary>
        public int AgencyRefundCount { get; set; }

        /// <summary>
        /// 官方区平台收益
        /// </summary>
        public decimal OfficialPlatformEarnings { get; set; }

        /// <summary>
        /// 官方区退款金额
        /// </summary>
        public decimal OfficialRefundAmount { get; set; }

        /// <summary>
        /// 官方区退款笔数
        /// </summary>
        public int OfficialRefundCount { get; set; }
    }
}