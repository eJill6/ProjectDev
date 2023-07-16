using JxBackendService.Interface.Model.AWCSP;
using Newtonsoft.Json;

namespace JxBackendService.Model.ThirdParty.AWCSP
{
    public class AWCSPGetBalanceRequestModel : AWCSPBaseRequestModel
    {
        public string PlayerId { get; set; }
    }

    /// <summary>
    /// 创建玩家
    /// </summary>
    public class AWCSPRegisterRequestModel : AWCSPBaseRequestModel
    {
        /// <summary>
        /// 独一使用者 ID，不可重复，仅许可 0-9, a-z
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 玩家货币代码
        /// </summary>
        public string Currency => "CNY";

        /// <summary>
        ///
        /// </summary>
        public string BetLimit { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Language => "cn";
    }

    public class HORSEBOOKLIVE
    {
        [JsonProperty(PropertyName = "maxbet")]
        public int Maxbet { get; set; }

        [JsonProperty(PropertyName = "minbet")]
        public int Minbet { get; set; }

        [JsonProperty(PropertyName = "maxBetSumPerHorse")]
        public int MaxBetSumPerHorse { get; set; }

        [JsonProperty(PropertyName = "minorMaxbet")]
        public int MinorMaxbet { get; set; }

        [JsonProperty(PropertyName = "minorMinbet")]
        public int MinorMinbet { get; set; }

        [JsonProperty(PropertyName = "minorMaxBetSumPerHorse")]
        public int MinorMaxBetSumPerHorse { get; set; }
    }

    public class HORSEBOOKRegisterBetLimit
    {
        public HORSEBOOKLIVE LIVE { get; set; }
    }

    public class SV388LIVE
    {
        [JsonProperty(PropertyName = "maxbet")]
        public int Maxbet { get; set; }

        [JsonProperty(PropertyName = "minbet")]
        public int Minbet { get; set; }

        [JsonProperty(PropertyName = "mindraw")]
        public int Mindraw { get; set; }

        [JsonProperty(PropertyName = "matchlimit")]
        public int Matchlimit { get; set; }

        [JsonProperty(PropertyName = "maxdraw")]
        public int Maxdraw { get; set; }
    }

    public class SV388RegisterBetLimit
    {
        public SV388LIVE LIVE { get; set; }
    }

    /// <summary>
    /// 查询玩家数据
    /// </summary>
    public class AWCSPUserInfoRequestModel : AWCSPBaseRequestModel
    {
        /// <summary>
        /// 玩家账号，只限a-z与0-9。
        /// </summary>
        public string UserIds { get; set; }
    }

    public class AWCSPCheckTransferRequestModel : AWCSPBaseRequestModel
    {
        /// <summary>
        /// 由您设置的唯一交易码避免重复操作
        /// </summary>
        public string TxCode { get; set; }
    }

    public class AWCSPTransferRequestModel : AWCSPBaseRequestModel
    {
        /// <summary>
        /// 玩家账号
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 唯一的交易码
        /// </summary>
        public string TxCode { get; set; }

        /// <summary>
        /// 转帐金额
        /// </summary>
        public string TransferAmount { get; set; }
    }

    public class AWCSPWithdrawRequestModel : AWCSPTransferRequestModel
    {
        /// <summary>
        /// 1: All 全部 (default预设值)
        /// 0: Partial部份
        /// </summary>
        public int WithdrawType { get; set; }
    }

    public class AWCSPGetBetLogRequestModel : AWCSPBaseRequestModel
    {
        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string Platform { get; set; }
    }

    /// <summary>
    /// 获取游戏链接（Token）请求参数
    /// </summary>
    public class AWCSPLaunchGameRequestModel : AWCSPBaseRequestModel, IAWCLunchGameCode
    {
        /// <summary>
        /// 独一使用者 ID，不可重复，仅许可 0-9, a-z
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 是否為手機裝置登入
        /// </summary>
        public bool IsMobileLogin { get; set; }

        /// <summary>
        /// 用于导回您指定的网站，需要设置 http:// 或 https://
        /// </summary>
        public string ExternalURL { get; set; }

        /// <summary>
        /// 可选功能。指定进入游戏大厅时的分页
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// 可选功能。指定进入游戏大厅时的分页
        /// </summary>
        public string GameType => "LIVE";

        /// <summary>
        /// 可选功能。指定进入游戏大厅时的分页
        /// </summary>
        public string GameCode { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        public string Language => "cn";
    }
}