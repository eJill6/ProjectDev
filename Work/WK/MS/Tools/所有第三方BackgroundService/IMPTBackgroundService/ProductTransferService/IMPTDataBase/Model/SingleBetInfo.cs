using JxBackendService.Model.ThirdParty.Base;

namespace IMPTDataBase.Model
{
    public interface ISingleBetInfo
    {
        decimal Balance { get; set; }

        decimal Bet { get; set; }

        decimal CurrentBet { get; set; }

        string ExitGame { get; set; }

        string GameCode { get; set; }

        string GameDate { get; set; }

        string GameId { get; set; }

        string GameName { get; set; }

        string GameType { get; set; }

        string LiveNetwork { get; set; }

        string PlayerName { get; set; }

        decimal ProgressiveBet { get; set; }

        decimal ProgressiveWin { get; set; }

        string ProviderPlayerId { get; set; }

        string RNum { get; set; }

        string SessionId { get; set; }

        decimal Win { get; set; }

        string WindowCode { get; set; }

        string BonusType { get; set; }
    }

    public class SingleBetInfo : ISingleBetInfo
    {
        public string PlayerName { get; set; }

        public string ProviderPlayerId { get; set; }

        public string WindowCode { get; set; }

        public string GameId { get; set; }

        public string GameCode { get; set; }

        public string GameType { get; set; }

        public string GameName { get; set; }

        public string SessionId { get; set; }

        public decimal Bet { get; set; }

        public decimal Win { get; set; }

        public decimal ProgressiveBet { get; set; }

        public decimal ProgressiveWin { get; set; }

        public decimal Balance { get; set; }

        public decimal CurrentBet { get; set; }

        public string GameDate { get; set; }

        public string LiveNetwork { get; set; }

        public string ExitGame { get; set; }

        public string BonusType { get; set; }

        public string RNum { get; set; }
    }

    public class SingleBetInfoViewModel : BaseRemoteBetLog
    {
        public ISingleBetInfo SingleBetInfo { get; set; }

        public override string KeyId => SingleBetInfo.GameCode;

        public override string TPGameAccount => SingleBetInfo.PlayerName;
    }
}