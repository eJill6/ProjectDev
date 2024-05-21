using System;

namespace MS.Core.MMModel.Models.Wallet
{
    public class IncomeInfoViewModel
    {
        public string Id { get; set; }
        /// <summary>
        /// 解鎖價格
        /// </summary>
        public string UnlockAmount { get; set; }

        /// <summary>
        /// 收益
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 解鎖使用者ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 貼子 Title
        /// </summary>
        public string PostTitle { get; set; }

        /// <summary>
        /// 交易時間
        /// </summary>
        public string TransactionTime { get; set; } = string.Empty;

        /// <summary>
        /// IncomeExpenseCategoryEnum.cs
        /// </summary>
        public byte Category { get; set; }
    }
}