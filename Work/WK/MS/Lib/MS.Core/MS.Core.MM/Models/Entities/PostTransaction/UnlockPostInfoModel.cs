using MS.Core.MM.Models.IncomeExpense;
using MS.Core.MM.Services;

namespace MS.Core.MM.Models.Entities.PostTransaction
{
    public class UnlockPostInfoModel
    {
        public MMPostTransactionModel PostTransaction { get; set; }
        public MMIncomeExpenseModel Income { get; set; }
        public MMIncomeExpenseModel Expense { get; set; }
        public ExpenseData ExpenseData { get; set; }
    }

}
