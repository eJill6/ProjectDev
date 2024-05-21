using System.ComponentModel;

namespace MS.Core.MMModel.Models.IncomeExpense
{
    public enum IncomeExpensePayType : byte
    {
        /// <summary>
        /// 钻石
        /// </summary>
        [Description("钻石")]
        Point = 1,
        /// <summary>
        /// 觅钱包
        /// </summary>
        [Description("觅钱包")]
        Amount = 2,
    }
}
