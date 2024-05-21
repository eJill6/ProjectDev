using JxBackendService.Model.Attributes;
using JxBackendService.Resource.Element;

namespace BackSideWeb.Models.ViewModel.OperatingData
{
    public class DailyRevenueViewModel
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? Date { get; set; }
        public int DAU { get; set; }
        public int PU { get; set; }
        /// <summary>
        /// 付费率
        /// </summary>
        public double PayingRate { get; set; }
        /// <summary>
        /// /收益
        /// </summary>
        public double Revenue { get; set; }
        public double ARPU { get; set; }
        public double ARPPU { get; set; }
        /// <summary>
        /// 保证金金额
        /// </summary>
        public double DepositAmount { get; set; }
        /// <summary>
        /// 保证金笔数
        /// </summary>
        public int DepositCount { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataTimeText), ResourceType = typeof(DisplayElement), Sort = 1)]
        public string DailyRevenueTimeText  => Date?.ToString("yyyy-MM-dd");

        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataDAU), ResourceType = typeof(DisplayElement), Sort = 2)]
        public string DAUText => DAU.ToString("N0");
        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataPU), ResourceType = typeof(DisplayElement), Sort = 3)]
        public string PUText => PU.ToString("N0");
        /// <summary>
        /// 付费率
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataPayoutRate), ResourceType = typeof(DisplayElement), Sort = 4)]
        public string PayingRateText => $"{PayingRate.ToString("N2")}%";
        /// <summary>
        /// 收益
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataEarnings), ResourceType = typeof(DisplayElement), Sort = 5)]
        public string RevenueText => Revenue.ToString("N2");


        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataARPU), ResourceType = typeof(DisplayElement), Sort = 6)]
        public string ARPUText => ARPU.ToString("N2");
        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataARPPU), ResourceType = typeof(DisplayElement), Sort = 7)]
        public string ARPPUText => ARPPU.ToString("N2");
        /// <summary>
        /// 保证金金额
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
