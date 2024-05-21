using JxBackendService.Model.Attributes;
using JxBackendService.Resource.Element;

namespace JxBackendService.Model.ViewModel.OperatingData
{
    public class OperationOverviewViewModel
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

        [Export(ResourcePropertyName = nameof(DisplayElement.TotalUsers), ResourceType = typeof(DisplayElement), Sort = 1)]
        public string TotalUsersText => TotalUsers.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.PaidUsersCount), ResourceType = typeof(DisplayElement), Sort = 2)]
        public string PaidUsersCountText => PaidUsersCount.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.PaymentRate), ResourceType = typeof(DisplayElement), Sort = 3)]
        public string PaymentRateText => PaymentRate.ToString("0.00") + "%";

        [Export(ResourcePropertyName = nameof(DisplayElement.TotalRevenue), ResourceType = typeof(DisplayElement), Sort = 4)]
        public string TotalRevenueText => TotalRevenue.ToString("N2");

        [Export(ResourcePropertyName = nameof(DisplayElement.ARPU), ResourceType = typeof(DisplayElement), Sort = 5)]
        public string ARPUText => ARPU.ToString("N2");

        [Export(ResourcePropertyName = nameof(DisplayElement.ARPPU), ResourceType = typeof(DisplayElement), Sort = 6)]
        public string ARPPUText => ARPPU.ToString("N2");

        [Export(ResourcePropertyName = nameof(DisplayElement.DepositsReceivedAmount), ResourceType = typeof(DisplayElement), Sort = 7)]
        public string DepositsReceivedAmountText => DepositsReceivedAmount.ToString("N2");

        [Export(ResourcePropertyName = nameof(DisplayElement.DepositsReceivedCount), ResourceType = typeof(DisplayElement), Sort = 8)]
        public string DepositsReceivedCountText => DepositsReceivedCount.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.MerchantCount), ResourceType = typeof(DisplayElement), Sort = 9)]
        public string MerchantCountText => MerchantCount.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.SquareUnlockEarnings), ResourceType = typeof(DisplayElement), Sort = 10)]
        public string SquareUnlockEarningsText => SquareUnlockEarnings.ToString("N2");

        [Export(ResourcePropertyName = nameof(DisplayElement.SquareRefundAmount), ResourceType = typeof(DisplayElement), Sort = 11)]
        public string SquareRefundAmountText => SquareRefundAmount.ToString("N2");

        [Export(ResourcePropertyName = nameof(DisplayElement.SquareRefundCount), ResourceType = typeof(DisplayElement), Sort = 12)]
        public string SquareRefundCountText => SquareRefundCount.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.AgencyUnlockEarnings), ResourceType = typeof(DisplayElement), Sort = 13)]
        public string AgencyUnlockEarningsText => AgencyUnlockEarnings.ToString("N2");

        [Export(ResourcePropertyName = nameof(DisplayElement.AgencyRefundAmount), ResourceType = typeof(DisplayElement), Sort = 14)]
        public string AgencyRefundAmountText => AgencyRefundAmount.ToString("N2");

        [Export(ResourcePropertyName = nameof(DisplayElement.AgencyRefundCount), ResourceType = typeof(DisplayElement), Sort = 15)]
        public string AgencyRefundCountText => AgencyRefundCount.ToString("N0");

        [Export(ResourcePropertyName = nameof(DisplayElement.OfficialPlatformEarnings), ResourceType = typeof(DisplayElement), Sort = 16)]
        public string OfficialPlatformEarningsText => OfficialPlatformEarnings.ToString("N2");

        [Export(ResourcePropertyName = nameof(DisplayElement.OfficialRefundAmount), ResourceType = typeof(DisplayElement), Sort = 17)]
        public string OfficialRefundAmountText => OfficialRefundAmount.ToString("N2");

        [Export(ResourcePropertyName = nameof(DisplayElement.OfficialRefundCount), ResourceType = typeof(DisplayElement), Sort = 18)]
        public string OfficialRefundCountText => OfficialRefundCount.ToString("N0");
    }
}