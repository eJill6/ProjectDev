using System.ComponentModel;

namespace MS.Core.MMModel.Models.IncomeExpense
{
    public enum IncomeExpenseStatusEnum : byte
    {
        /// <summary>
        /// 審核通过/支出
        /// </summary>
        [Description("审核通过")]
        Approved = 1,

        /// <summary>
        /// 未派發
        /// </summary>
        [Description("未派发")]
        UnDispatched = 2,

		/// <summary>
		/// 投诉不入账
		/// </summary>
		[Description("投诉不入账")]
		ReportUnDispatched = 5,

		/// <summary>
		/// 已派發
		/// </summary>
		[Description("已派发")]
        Dispatched = 10,

        /// <summary>
        /// 審核不通过
        /// </summary>
        [Description("审核不通过")]
        Reject = 11,

        /// <summary>
        /// 未退款
        /// </summary>
        [Description("未退款")]
        UnRefund = 12,

        /// <summary>
        /// 已退款
        /// </summary>
        [Description("已退款")]
        Refund = 13,

        /// <summary>
        /// 異常(有評論且不是審核未通过、無會員卡)
        /// </summary>
        [Description("异常")]
        Unusual = 99
    }
}