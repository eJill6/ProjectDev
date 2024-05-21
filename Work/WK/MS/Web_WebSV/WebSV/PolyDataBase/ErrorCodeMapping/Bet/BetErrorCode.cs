using System.ComponentModel;

namespace SLPolyGame.Web.ErrorCodeMapping.Bet
{
    /// <summary>
    /// Define for internal use to increase the readability of error description and unify error codes.
    /// Todo : 1. Consider how to migrate it to web project.
    /// </summary>
    public enum BetErrorCode
    {
        [Description("Success")]
        Success = 0,

        #region bet error code start from 100
        //Parameters is negative
        [Description("0L")]
        ParameterNagative = 100,

        //中獎金額>系統最高獎金
        [Description("0M")]
        OverMaxBonus = 101,

        //總金額不相等 :
        [Description("0S")]
        TotalAmountNotEqual = 102,

        //總注數不相等 :
        [Description("0V")]
        BetCntNotEqual = 103,

        //最小單價非法 :
        [Description("0PRE")]
        MinAmountIllegal = 104,

        //注單 已超過封單時間 :
        [Description("03")]
        BetOverTime = 105,

        //BetInfo 找不到 :
        [Description("0G")]
        BetInfoNotFound = 106,

        //BetInfo 找不到 :
        [Description("0K")]
        OverMaxBetCnt = 107,

        //參數錯誤
        [Description("0Y")]
        SpParameterError = 108,
        //SpParameterError = 107,
        #endregion
    }
}