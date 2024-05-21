using System.ComponentModel;

namespace SLPolyGame.Web.Enums
{
    /// <summary>
    /// AwardStatus
    /// </summary>
    public enum AwardStatus
    {
        /// <summary>
        /// 待确认
        /// </summary>
        [Description("待确认")]
        Confirming = -1,

        /// <summary>
        /// 待开奖
        /// </summary>
        [Description("待开奖")]
        Unawarded = 0,

        /// <summary>
        /// 已開奖(where db使用)
        /// </summary>
        [Description("已开奖")]
        IsDone = 1,

        /// <summary>
        /// 开奖失败
        /// </summary>
        [Description("开奖失败")]
        OpenFail = 2,

        /// <summary>
        /// 撤单
        /// </summary>
        [Description("撤单")]
        Canceled = 3,

        /// <summary>
        /// 正在开奖
        /// </summary>
        [Description("正在开奖")]
        Opening = 4,

        /// <summary>
        /// 废单
        /// </summary>
        [Description("废单")]
        Abandoned = 5,

        /// <summary>
        /// 和局  = 开奖程式系统撤单
        /// </summary>
        [Description("和局")]
        SystemCancel = 6,

        /// <summary>
        /// 系统撤单
        /// </summary>
        [Description("系统撤单")]
        SystemRefund = 7,

        /// <summary>
        /// 已中奖
        /// </summary>
        [Description("已中奖")]
        Won = 8,

        /// <summary>
        /// 未中獎
        /// </summary>
        [Description("未中奖")]
        Lost = 9,
    }
}