using System.ComponentModel;

namespace ProductTransferService.LCDataBase.Enums
{
    /// <summary>
    /// 棋牌 api 返回錯誤碼
    /// </summary>
    public enum APIErrorCode
    {
        [Description("成功")]
        Success = 0,

        [Description("TOKEN 丢失")]
        TokenLoss = 1,

        [Description("渠道不存在")]
        ChannelNotExist = 2,

        [Description("验证时间超时")]
        VerifyTimeout = 3,

        [Description("验证错误")]
        VerifyFail = 4,

        [Description("渠道白名单错误")]
        WhiteListFail = 5,

        [Description("验证字段丢失")]
        VerifyParamStrLoss = 6,

        [Description("不存在的请求")]
        RequestNotExist = 8,

        [Description("渠道验证错误")]
        ChannelVerifyFail = 15,

        [Description("数据不存在")]
        DataNotExist = 16,

        [Description("账号禁用")]
        AccountBanned = 20,

        [Description("AES 解密失败")]
        DecryptAESFail = 22,

        [Description("渠道拉取数据超过时间范围")]
        ChannelGetDataTimeout = 24,

        [Description("订单号不存在")]
        OrderIdNotExist = 26,

        [Description("数据库异常")]
        DatabaseAbnormal = 27,

        [Description("ip 禁用")]
        IpBanned = 28,

        [Description("订单号与订单规则不符")]
        OrderNumberAndOrderRuleNotMatch = 29,

        [Description("获取玩家在线状态失败")]
        GetPlayerOnlineStatusFail = 30,

        [Description("更新的分数小于或者等于 0")]
        UpdateAmountLessOrEqualsZero = 31,

        [Description("更新玩家信息失败")]
        UpdatePlayerInfoFail = 32,

        [Description("更新玩家金币失败")]
        UpdatePlayerGoldFail = 33,

        [Description("订单重复")]
        OrderRepeat = 34,

        [Description("获取玩家信息失败")]
        GetPlayerInfoFail = 35,

        [Description("KindID 不存在")]
        KindIdNotExist = 36,

        [Description("登录瞬间禁止下分，导致下分失败")]
        LoginMomentBannedRecharge = 37,

        [Description("余额不足导致下分失败")]
        InsufficientBalanceWithRechargeFail = 38,

        [Description("禁止同一账号登录带分、上分、下分并发请求，后一个请求被拒")]
        SameAccountLoginBannedLastWithdrawOrRechargeRequest = 39,

        [Description("单次上下分数量不能超过一千万")]
        OnceTransferCannotExceedTenMillion = 40,

        [Description("拉取对局汇总统计时间范围有误")]
        StatisticsTimeRangeFail = 41,

        [Description("代理被禁用")]
        AgentBanned = 42,

        [Description("拉单过于频繁")]
        GetOrderInfoTooFrequent = 43,

        [Description("请求失败")]
        RequestFail = 999,

        [Description("注册会员账号系统异常")]
        RegisterMemberSystemAbnormal = 1001,

        [Description("代理商金额不足")]
        AgentAmountInsufficient = 1002,

        [Description("玩家大厅上分/下分异常")]
        PlayerWithdrawOrRechargeAbnormal = 1003,

        [Description("代理商渠道不存在")]
        AgentChannelNotExist = 1004,

        [Description("会员金额不足")]
        MemberBalanceInsufficient = 1005,

        [Description("会员游戏上分/下分异常")]
        MemberGameWithdrawOrRechargeAbnormal = 1006,

        [Description("玩家登录游戏上分异常")]
        PlayerLoginRechargeAbnormal = 1007,

        [Description("玩家登出游戏下分异常")]
        PlayerLogoutWithdrawAbnormal = 1008,

        [Description("上下分出现负数(非法值)")]
        AppearNegativeOfTransfer = 1009,

        [Description("会员退出大厅异常")]
        MemberExitHallAbnormal = 1010,

        [Description("订单已存在")]
        OrderExist = 1011,

        [Description("订单号不符合规则")]
        OrderNumberRuleNotMatch = 1012,

        [Description("后台上分/下分异常")]
        BackstageTransferAbnormal = 1013,

        [Description("上级代理金额不足")]
        InsufficientAmountOfSuperiorAgent = 1014,

        [Description("会员代理充值异常")]
        MemberAgentRechargeAbnormal = 1015,

        [Description("玩家账户被锁")]
        BannedOfPlayerAccount = 1016,

        [Description("更新会员、浏览器信息异常")]
        UpdateMemberOrBrowserAbnormal = 1017,

        [Description("存储过程参数格式不正确或参数为空")]
        StoredProcedureParameterError = 1018,

        [Description("更新会员玩过的游戏信息异常")]
        UpdatePlayedGameInfoAbnormal = 1019,

        [Description("玩家大厅进(出)游戏上分(下分)生成订单异常")]
        PlayerEnterHallAbnormal = 1020,

        [Description("会员订单失效或不存在")]
        InvalidOrNotExistMemberOrder = 1021,

        [Description("会员订单已经处理过，不能重复处理")]
        MemberOrderIsProcessed = 1022,

        [Description("玩家正在游戏中，不能上下分")]
        PlayerGamingCannotTransfer = 1023,

        [Description("大厅会员账号不存在")]
        MemberAccountNotExistOfHall = 1024,

        [Description("参数错误，子渠道标识不能为空")]
        ParameterErrorSubChannelTagNotNull = 1025,

        [Description("系统预设-资料解析失败")]
        SysDefault = -9999
    }
}