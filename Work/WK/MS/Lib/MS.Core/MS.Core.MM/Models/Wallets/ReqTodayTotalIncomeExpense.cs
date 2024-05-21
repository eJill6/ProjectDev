using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MM.Models.Wallets
{
    public class ReqTodayTotalIncomeExpense
    {
        public PostType? PostType { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }

        public IEnumerable<IncomeExpenseTransactionTypeEnum> TransactionTypes { get; set; }
        public IncomeExpensePayType? PayType { get; set; }
    }
}
