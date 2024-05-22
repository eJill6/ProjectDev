using System.ComponentModel;

namespace IMSportsbookDataBase.Enums
{
    /// <summary>
    /// IMSportsbook api 返回錯誤碼
    /// </summary>
    public enum APIErrorCode
    {
        [Description("成功")]
        Success = 0,

        [Description("营运商或代理商代码无效")]
        InvalidMerchant = 500,

        [Description("非法访问")]
        UnauthorizedAccess = 501,

        [Description("玩家不存在")]
        PlayerDoesNotExist = 504,

        [Description("必须的参数不能为空")]
        RequiredFieldNull = 505,

        [Description("玩家账号无效")]
        InvalidPlayerID = 506,

        [Description("货币代码无效")]
        InvalidCurrency = 507,

        [Description("产品钱包无效")]
        InvalidProductWallet = 508,

        [Description("交易码无效")]
        InvalidTransactionId = 509,

        [Description("余额不足")]
        InsufficientAmount = 510,

        [Description("交易码在IMSportsbookOne系统里是重复的")]
        TransactionIdIsDuplicatedIMSportsbookOne = 514,

        [Description("交易码不存在于产品供应商")]
        TransactionIdIsNotFound = 516,

        [Description("产品供应商正在处理该交易")]
        TransactionIsBeingProcessed = 517,

        [Description("语言无效")]
        InvalidLanguage = 518,

        [Description("金额格式无效")]
        InvalidAmountFormat = 519,

        [Description("正在处理该交易")]
        TransactionIsBeingProcessedIMSportsbookOne = 520,

        [Description("交易码在产品端里是重复的")]
        TransactionIdIsDuplicated = 523,

        [Description("搜寻时间区间无效")]
        InvalidTimerange = 525,

        [Description("搜寻起始时间大于搜寻结束时间")]
        StartDateLaterThanEndDate = 526,

        [Description("日志正在处理中，请稍后重试")]
        BetDetailsInProcess = 527,

        [Description("时间格式无效")]
        InvalidDatetimeFormat = 528,

        [Description("设置正在进行中，请联络技术支援")]
        SetupInProgress = 538,

        [Description("玩家未在产品供应商端创建成功或在停用状态或在停用状态")]
        PlayerWasNotCreatedSuccessfully = 540,

        [Description("玩家未在产品供应商端创建成功或在停用状态或在停用状态")]
        TransactionHasBeenProcessed = 541,

        [Description("玩家在停用状态")]
        PlayerIsInactive = 542,

        [Description("金额无效。金额必须是货币的倍数")]
        AmountMustBeMultiple = 543,

        [Description("玩家在进行游戏时，交易无法被处理")]
        TransactionCannotBeProcessed = 544,

        [Description("最后跟新时间值必须在搜寻日期区间以内")]
        LastUpdatedDateInStartAndEndDate = 547,

        [Description("玩家已被注销")]
        PlayerIsSuspended = 548,

        [Description("超出API的访问频率限制")]
        APIIsCalledIntervalAllowed = 557,

        [Description("无数据")]
        NoDataFound = 558,

        [Description("玩家有其他交易未完成处理")]
        OtherTransactionIsYetToBeProcessed = 560,

        [Description("系统未能发送响应，请联系支持")]
        SystemHasFailed = 566,

        [Description("先前的请求尚未完成，请再试一次")]
        RequestIsYetBeCompleted = 567,

        [Description("产品供应商内部错误")]
        ProviderInternalError = 600,

        [Description("非法产品访问")]
        UnauthorizedProductAccess = 601,

        [Description("无效的参数值")]
        InvalidArgument = 612,

        [Description("系统目前无法处理您的请求。请重试")]
        SystemIsCurrently = 998,

        [Description("系统处理您的请求失败")]
        SystemHasFailedYourRequest = 999,

        [Description("系统预设-资料解析失败")]
        SysDefault = -9999
    }
}
