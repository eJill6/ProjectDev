using System.ComponentModel;

namespace LCDataBase.Enums
{
    /// <summary>
    /// api 呼叫代號列表
    /// </summary>
    public enum ApiAction
    {
        [Description("登入")]
        Login = 0,

        [Description("查询玩家可下分余额")]
        QueryBalance = 1,

        [Description("充值")]
        Recharge = 2,

        [Description("提現")]
        Withdraw = 3,

        [Description("查询订单状态")]
        OrderStatus = 4,

        [Description("查询玩家是否在线")]
        IsOnline = 5,

        [Description("获取游戏结果数据")]
        PlayGameResult = 6,

        [Description("查询游戏总余额")]
        TotalBalance = 7,

        [Description("根据玩家账号踢玩家下线")]
        KickOut = 8
    }
}
