using System.ComponentModel;

namespace IMBGDataBase.Enums
{
    /// <summary>
    /// IMBG api 返回錯誤碼
    /// </summary>
    public enum APIErrorCode
    {
        [Description("成功")]
        Success = 0,

        [Description("重复订单，订单已成功处理")]
        DuplicateProcessOrder = 1000,

        [Description("token过期（重新调用登录接口获取）")]
        TokenExpired = 1001,

        [Description("代理商标示不存在（请检查agentId是否正确）")]
        InvalidMerchant = 1002,

        [Description("验证时间超时（请检查 timestamp 是否正确）")]
        VerificationExpired = 1003,

        [Description("参数加密字符串解析错误")]
        EncryptedParameterParsingFailed = 1004,

        [Description("MD5校验字符串验证错误")]
        MD5ParameterParsingFailed = 1005,

        [Description("访问IP不在白名单内（请联系客服添加服务器白名单）")]
        AccessIpIsNotInTheWhitelist = 1006,

        [Description("没有该子操作类型")]
        AcDoesNotExist = 1007,

        [Description("数据操作失败")]
        DataOperatingFailed = 1008,

        [Description("获取token失败")]
        GetTokenFailed = 1009,

        [Description("用户有其他订单未操作，上下分失败")]
        OtherTransactionIsYetToBeProcessedAndToScoreFailed = 1010,

        [Description("订单号格式有误")]
        IncorrectOrderNumberFormat = 1011,

        [Description("userCode 有误，在平台服务器找不到对应的用户")]
        UserNotExist = 1012,

        [Description("用户还在游戏中，下分失败")]
        TheUserIsStillInTheGameToScoreFailed = 1013,

        [Description("用户的分数不足")]
        InsufficientUserScore = 1014,

        [Description("用户的可下分数不足")]
        InsufficientUserFreeScore = 1015,

        [Description("订单号不存在")]
        OrderNotExist = 1016,

        [Description("踢用户离线失败")]
        FailedToKickUserOffline = 1017,

        [Description("用户列表格式有误")]
        IncorrectUserListFormat = 1018,

        [Description("用户被锁定，不能进行上下分操作")]
        TheUserIsLockedCannotToScore = 1019,

        [Description("gameId 有误，或者代理还未开通该游戏")]
        TheGameIdIsWrongOrTheAgentHasNotActivatedTheGame = 1020,

        [Description("查询注单的时间有误，或已超出范围")]
        TheTimeForQueryingOrderIsWrongOrOutOfRange = 1021,

        [Description("上下分分数有误。（上下分分数不能为负数且上分分数不能大于 100 万）")]
        ToScoresWrong = 1022,

        [Description("homeUrl 有误或者不是合法的 URL 格式地址")]
        InvalidURLFormat = 1023,

        [Description("请求的代理标示非法（请求的代理商没有权限对该代理商进行信用系统的所有操作）")]
        InsufficientPermissions = 1024,

        [Description("该代理标示存在未处理完成的封盘操作")]
        UnfinishedClosingOperation = 1025,

        [Description("操作失败，该期数对应的上一步操作未完成或封盘期数不存在")]
        OperationFailed = 1026,

        [Description("重复请求，该 parentCode 还在处理中")]
        RepeatRequest = 1027,

        [Description("代理商处于封盘中")]
        TheAgentIsInClosing = 1028,

        [Description("该代理商的可用分数不足，上分失败")]
        TheAgentAvailablePointsNotEnoughToScore = 1029,

        [Description("该代理商还未设置售分额度")]
        TheAgentHasNotSetSalesQuota = 1030,

        [Description("系统目前无法处理您的请求。请重试")]
        SystemIsCurrently = 998,

        [Description("系统处理您的请求失败")]
        SystemHasFailedYourRequest = 999,

        [Description("系统预设-资料解析失败")]
        SysDefault = -9999
    }
}
