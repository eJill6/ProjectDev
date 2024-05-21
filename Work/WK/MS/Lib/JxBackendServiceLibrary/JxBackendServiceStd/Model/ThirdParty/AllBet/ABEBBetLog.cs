using JxBackendService.Model.ThirdParty.Base;

namespace JxBackendService.Model.ThirdParty.AllBet
{
    public class ABEBBetLog : BaseRemoteBetLog
    {
        public long BetNum { get; set; }

        public int GameRoundId { get; set; }

        public int Status { get; set; }

        public string Player { get; set; }

        public string Currency { get; set; }

        public decimal BetAmount { get; set; }

        public decimal Deposit { get; set; }

        public int GameType { get; set; }

        public int BetType { get; set; }

        public int Commission { get; set; }

        public decimal ExchangeRate { get; set; }

        public string GameResult { get; set; }

        public string GameResult2 { get; set; }

        public decimal WinOrLossAmount { get; set; }

        public decimal ValidAmount { get; set; }

        public string BetTime { get; set; }

        public string TableName { get; set; }

        public int BetMethod { get; set; }

        public int AppType { get; set; }

        public string GameRoundStartTime { get; set; }

        public string GameRoundEndTime { get; set; }

        public string Ip { get; set; }

        public override string KeyId => $"{BetNum}";

        public override string TPGameAccount => Player;
    }
}