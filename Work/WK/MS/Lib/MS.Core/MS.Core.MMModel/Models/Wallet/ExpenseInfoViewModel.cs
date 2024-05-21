using System;

namespace MS.Core.MMModel.Models.Wallet
{
    public class ExpenseInfoViewModel
    {
        public string Id { get; set; }
        /// <summary>
        /// 支付金額
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 交易時間
        /// </summary>
        public string TransactionTime { get; set; } = string.Empty;

        /// <summary>
        /// 類型
        /// </summary>
        public byte Category { get; set; }

        public byte TransactionType { get; set; }
    }
}