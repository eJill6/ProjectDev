using MS.Core.MMModel.Models.IncomeExpense;

namespace MS.Core.MMModel.Models.AdminIncomeExpense
{
    /// <summary>
    /// 後台核可舉報後還款成功更新狀態使用
    /// </summary>
    public class AdminIncomeExpenseUpdateStatusParam
    {
        /// <summary>
        /// 單號
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public IncomeExpenseStatusEnum Status { get; set; }

        /// <summary>
        /// 審核原因
        /// </summary>
        public string Memo { get; set; }
    }
}