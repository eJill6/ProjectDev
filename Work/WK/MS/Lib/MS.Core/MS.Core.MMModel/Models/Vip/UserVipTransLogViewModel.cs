using System;

namespace MS.Core.MMModel.Models.Vip
{
    public class UserVipTransLogViewModel
    {
        public string Title { get; set; }
        public string TransactionTime { get; set; }
        /// <summary>
        /// 狀態(IncomeExpenseStatusEnum.cs)
        /// </summary>
        public byte Status { get; set; }
        /// <summary>
        /// 支付方式(IncomeExpensePayType.cs)
        /// </summary>
        public byte? PayType { get; set; }
        /// <summary>
        /// 訂單編號
        /// </summary>
        public string OrderID { get; set; }
        /// <summary>
        /// 金額
        /// </summary>
        public string Amount { get; set; }
    }
}
