using MS.Core.MMModel.Models.IncomeExpense;

namespace MS.Core.MMModel.Models.AdminIncomeExpense
{
    public class AdminIncomeData
    {
        /// <summary>
        /// 評價編號
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 審核狀態 1. 入账, 2, 審核中, 4. 不入账, 
        /// </summary>
        public IncomeExpenseStatusEnum Status { get; set; }

        /// <summary>
        /// 未通過原因
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 審核人員暱稱
        /// </summary>
        public string ExamineMan { get; set; }
    }
}
