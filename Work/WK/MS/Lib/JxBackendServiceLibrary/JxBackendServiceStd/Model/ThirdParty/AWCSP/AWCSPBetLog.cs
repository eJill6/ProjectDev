using JxBackendService.Model.ThirdParty.Base;

namespace JxBackendService.Model.ThirdParty.AWCSP
{
    public class AWCSPBetLog : BaseRemoteBetLog
    {
        /// <summary>
        /// 平台游戏类型
        /// </summary>
        public string GameType { get; set; }

        /// <summary>
        /// 返还金额 (包含下注金额)
        /// </summary>
        public decimal WinAmount { get; set; }

        /// <summary>
        /// 交易时间
        /// </summary>
        public string TxTime { get; set; }

        /// <summary>
        /// 用于区分注单结果是否有更改
        /// </summary>
        public int SettleStatus { get; set; }

        /// <summary>
        /// 游戏讯息会由游戏商以 JSON 格式提供
        /// </summary>
        public string GameInfo { get; set; }

        /// <summary>
        /// 真实返还金额
        /// </summary>
        public decimal RealWinAmount { get; set; }

        /// <summary>
        /// 更新时间 (遵循 ISO8601 格式)
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 真实下注金额
        /// </summary>
        public decimal RealBetAmount { get; set; }

        /// <summary>
        /// 玩家 ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 游戏平台的下注项目
        /// </summary>
        public string BetType { get; set; }

        /// <summary>
        /// 游戏平台名称
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// 该交易当前状况
        /// </summary>
        public int TxStatus { get; set; }

        /// <summary>
        /// 下注金额
        /// </summary>
        public decimal BetAmount { get; set; }

        /// <summary>
        /// 游戏名称
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// 游戏商注单号
        /// </summary>
        public string PlatformTxId { get; set; }

        /// <summary>
        /// 玩家下注时间
        /// </summary>
        public string BetTime { get; set; }

        /// <summary>
        /// 平台游戏代码
        /// </summary>
        public string GameCode { get; set; }

        /// <summary>
        /// 玩家货币代码
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 累积奖金的获胜金额
        /// </summary>
        public decimal JackpotWinAmount { get; set; }

        /// <summary>
        /// 累积奖金的下注金额
        /// </summary>
        public decimal JackpotBetAmount { get; set; }

        /// <summary>
        /// 游戏平台有效投注
        /// </summary>
        public decimal Turnover { get; set; }

        /// <summary>
        /// 游戏商的回合识别码
        /// </summary>
        public string RoundId { get; set; }

        public override string KeyId => PlatformTxId;

        public override string TPGameAccount => UserId;
    }
}