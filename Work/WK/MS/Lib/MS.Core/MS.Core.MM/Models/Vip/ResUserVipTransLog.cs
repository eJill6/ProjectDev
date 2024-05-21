using MS.Core.MMModel.Models.IncomeExpense;

namespace MS.Core.MM.Models.Vip
{
    public class ResUserVipTransLog
    {
        public string Title { get; set; } = null!;
        public string TransactionTime { get; set; }
        /// <summary>
        /// 狀態
        /// </summary>
        public IncomeExpenseStatusEnum Status { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public IncomeExpensePayType? PayType { get; set; }
        /// <summary>
        /// 訂單編號
        /// </summary>
        public string OrderID { get; set; } = null!;
        /// <summary>
        /// 金額
        /// </summary>
        public string Amount { get; set; } = null!;
    }
}