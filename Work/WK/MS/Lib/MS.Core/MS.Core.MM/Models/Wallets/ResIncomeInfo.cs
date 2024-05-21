using MS.Core.MMModel.Models.IncomeExpense;

namespace MS.Core.MM.Models.Wallets
{
    public class ResIncomeInfo
    {
        public string Id { get; set; }
        /// <summary>
        /// 解鎖價格
        /// </summary>
        public string UnlockAmount { get; set; }

        public string Amount { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string PostTitle { get; set; }
        public string TransactionTime { get; set; }
        public IncomeExpenseCategoryEnum Category { get; set; }
    }
}