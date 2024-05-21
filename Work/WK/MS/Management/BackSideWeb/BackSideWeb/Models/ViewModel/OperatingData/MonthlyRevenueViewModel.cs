using JxBackendService.Model.Attributes;
using JxBackendService.Resource.Element;

namespace BackSideWeb.Models.ViewModel.OperatingData
{
    /// <summary>
    /// 月营收
    /// </summary>
    public class MonthlyRevenueViewModel
    {
        /// <summary>
        /// 月份
        /// </summary>
        public DateTime Month { get; set; }
        /// <summary>
        /// MAU
        /// </summary>
        public decimal MAU { get; set; }
        /// <summary>
        /// 付费用户
        /// </summary>
        public decimal PU { get; set; }
        /// <summary>
        /// 付费率
        /// </summary>
        public decimal PayingRate { get; set; }
        /// <summary>
        /// 总收益
        /// </summary>
        public decimal TotalRevenue { get; set; }
        public decimal ARPU { get; set; }
        public decimal ARPPU { get; set; }
        /// <summary>
        /// 保证金
        /// </summary>
        public decimal DepositAmount { get; set; }
        /// <summary>
        /// 保证金笔数
        /// </summary>
        public decimal DepositCount { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataMonthTimeText), ResourceType = typeof(DisplayElement), Sort = 1)]
        public string MonthlyRevenueTimeText => Month.ToString("yyyy-MM");
        /// <summary>
        /// MAU
        /// </summary>

        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataMAU), ResourceType = typeof(DisplayElement), Sort = 2)]
        public string MAUText => MAU.ToString("N0");
        /// <summary>
        /// 付费用户
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataPU), ResourceType = typeof(DisplayElement), Sort = 3)]
        public string PUText => PU.ToString("N0");
        /// <summary>
        /// 付费率
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataPayoutRate), ResourceType = typeof(DisplayElement), Sort = 4)]
        public string PayingRateText => $"{PayingRate.ToString("N2")}%";
        /// <summary>
        /// 总收益
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataTotalEarnings), ResourceType = typeof(DisplayElement), Sort = 5)]
        public string TotalRevenueText => TotalRevenue.ToString("N2");

        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataARPU), ResourceType = typeof(DisplayElement), Sort = 6)]
        public string ARPUText => ARPU.ToString("N2");
        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataARPPU), ResourceType = typeof(DisplayElement), Sort = 7)]
        public string ARPPUText => ARPPU.ToString("N2");
        /// <summary>
        /// 保证金
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataEarnestMoney), ResourceType = typeof(DisplayElement), Sort = 8)]
        public string DepositAmountText => DepositAmount.ToString("N2");
        /// <summary>
        /// 保证金笔数
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataEarnestMoneyCount), ResourceType = typeof(DisplayElement), Sort = 9)]
        public string DepositCountText => DepositCount.ToString("N0");
    }
}
