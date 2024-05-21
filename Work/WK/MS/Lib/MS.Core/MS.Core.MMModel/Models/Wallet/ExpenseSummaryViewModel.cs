using MS.Core.Models.Models;

namespace MS.Core.MMModel.Models.Wallet
{
    public class ExpenseSummaryViewModel : PageResultModel<ExpenseInfoViewModel>
    {
        /// <summary>
        /// 總收益
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}
