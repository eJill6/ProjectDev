using MS.Core.MMModel.Attributes;
using System.ComponentModel;

namespace MS.Core.MMModel.Models.IncomeExpense
{
    /// <summary>
    /// 收入費用的來源
    /// </summary>
    public enum IncomeExpenseCategoryEnum : byte
    {
        None = 0,

        /// <summary>
        /// 廣場帖
        /// </summary>
        [ExpenseDescription("广场帖解锁")]
        [Description("广场帖")]
        Square = 1,

        /// <summary>
        /// 寻芳阁(原為中介)
        /// </summary>
        [ExpenseDescription("寻芳阁解锁")]
        [Description("寻芳阁")]
        Agency = 2,

        /// <summary>
        /// 帖子官方
        /// </summary>
        [ExpenseDescription("官方帖支付")]
        [Description("官方帖")]
        Official = 3,


        /// <summary>
        /// 帖子體驗
        /// </summary>
        [ExpenseDescription("体验帖解锁")]
        [Description("体验帖")]
        Experience = 4,

        ///// <summary>
        ///// 超觅老板
        ///// </summary>
        //[ExpenseDescription("超觅老板支付")]
        //[Description("超觅老板")]
        //SuperBoss = 5,

        /// <summary>
        /// 會員
        /// </summary>
        Vip = 51,
    }
}