using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.Models.Models;

namespace MS.Core.MMModel.Models.AdminIncomeExpense
{
    /// <summary>
    /// 列表搜尋參數
    /// </summary>
    public class AdminIncomeExpenseListByTypeParam : PageParam
    {
        /// <summary>
        /// 要搜尋的編號
        /// </summary>
        public string[] Ids { get; set; }

        /// <summary>
        /// 搜尋的方向
        /// </summary>
        public IncomeExpenseTransactionTypeEnum Type { get; set; }

        /// <summary>
        /// 編號的類別 0: 解鎖單編號, 1: 收益支出單編號
        /// </summary>
        public int IdType { get; set; }
    }
}
