using MS.Core.MMModel.Models.IncomeExpense;

namespace MS.Core.MM.Models.Wallets
{
    public class ResExpenseInfo
    {
        public string Amount { get; set; }
        public string Title { get; set; }
        public string TransactionTime { get; set; }
        public IncomeExpenseCategoryEnum Category { get; set; }
        public IncomeExpenseTransactionTypeEnum TransactionType { get; set; }
    }
}
