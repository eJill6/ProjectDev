using JxBackendService.Model.Attributes;
using JxBackendService.Resource.Element;

namespace BackSideWeb.Models.ViewModel.OperatingData
{
    public class PostDailyRevenueViewModel
    {
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

        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataTimeText), ResourceType = typeof(DisplayElement), Sort = 1)]
        public string DateText => Date.ToString("yyyy-MM-dd");

        /// <summary>
        /// 广场解锁收益
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataSquareUnlockEarnings), ResourceType = typeof(DisplayElement), Sort = 2)]
        public string SquareUnlockRevenueText => SquareUnlockRevenue.ToString("N2");

        /// <summary>
        /// 寻芳阁解锁收益
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataGuaranteeUnlockEarnings), ResourceType = typeof(DisplayElement), Sort = 3)]
        public string AgencyUnlockRevenueText => AgencyUnlockRevenue.ToString("N2");

        /// <summary>
        /// 官方平台收益
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataOfficialPlatformEarnings), ResourceType = typeof(DisplayElement), Sort = 4)]
        public string OfficialPlatformRevenueText => OfficialPlatformRevenue.ToString("N2");

        /// <summary>
        /// 总收益
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataTotalEarnings), ResourceType = typeof(DisplayElement), Sort = 5)]
        public string TotalRevenueText => TotalRevenue.ToString("N2");

        /// <summary>
        /// 广场退款金额
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataSquareRefund), ResourceType = typeof(DisplayElement), Sort = 6)]
        public string SquareRefundAmountText => SquareRefundAmount.ToString("N2");

        /// <summary>
        /// 寻芳阁退款金额
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataGuaranteeRefund), ResourceType = typeof(DisplayElement), Sort = 7)]
        public string AgencyRefundAmountText => AgencyRefundAmount.ToString("N2");

        /// <summary>
        /// 官方退款金额
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataOfficialRefund), ResourceType = typeof(DisplayElement), Sort = 8)]
        public string OfficialRefundAmountText => OfficialRefundAmount.ToString("N2");

        /// <summary>
        /// 总退款
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.OperatingDataTotalRefund), ResourceType = typeof(DisplayElement), Sort = 9)]
        public string TotalRefundText => TotalRefund.ToString("N2");
    }
}