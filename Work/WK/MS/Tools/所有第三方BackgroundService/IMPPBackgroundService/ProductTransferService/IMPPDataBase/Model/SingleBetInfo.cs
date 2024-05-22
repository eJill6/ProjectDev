using JxBackendService.Model.ThirdParty.Base;

namespace IMPPDataBase.Model
{
    public class SingleBetInfo
    {
        /// <summary>
        /// 产品供应商代码，参考附录 B
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// IMOne 系统内的 GameID
        /// </summary>
        public string GameId { get; set; }

        /// <summary>
        /// 游戏英文名称
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// 遊戲中文名稱
        /// </summary>
        public string ChineseGameName { get; set; }

        /// <summary>
        /// 产品供应商所提供的游戏回合码
        /// </summary>
        public string RoundId { get; set; }

        /// <summary>
        /// 供应商提供的游戏回合码
        /// </summary>
        public string ExternalRoundId { get; set; }

        /// <summary>
        /// 玩家账号
        /// </summary>
        public string PlayerId { get; set; }

        /// <summary>
        /// 产品提供商玩家账号
        /// </summary>
        public string ProviderPlayerId { get; set; }

        /// <summary>
        /// 下注金额
        /// </summary>
        public decimal BetAmount { get; set; }

        /// <summary>
        /// 输赢
        /// </summary>
        public decimal WinLoss { get; set; }

        /// <summary>
        /// Jackpot/奖池贡献金
        /// </summary>
        public decimal ProgressiveBet { get; set; }

        /// <summary>
        /// Jackpot/Pro 奖池派彩金
        /// </summary>
        public decimal ProgressiveWin { get; set; }

        /// <summary>
        /// 红利
        /// </summary>
        public decimal Bonus { get; set; }

        /// <summary>
        /// 游戏供应商提供之红利
        /// </summary>
        public string ProviderBonus { get; set; }

        /// <summary>
        /// 注单状态: Open, Settled, Closed, Cancelled.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 下注平台: [Desktop\Mobile\Mini Games\Download\N/A]
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// IMOne 系统在收到下注单是的创建时间戳
        /// </summary>
        public DateTimeOffset DateCreated { get; set; }

        /// <summary>
        /// 产品供应商提供的时间戳
        /// </summary>
        public DateTimeOffset GameDate { get; set; }

        /// <summary>
        /// IMOne 系统对注单的最后跟新时间
        /// </summary>
        public DateTimeOffset LastUpdatedDate { get; set; }
    }

    public class SingleBetInfoViewModel : BaseRemoteBetLog
    {
        public SingleBetInfo SingleBetInfo { get; set; }

        public override string KeyId => SingleBetInfo.RoundId;

        public override string TPGameAccount => SingleBetInfo.PlayerId;
    }
}