namespace SLPolyGame.Web.MSSeal.Models.Interface
{
    public interface IMSThirdPartyBettingResult : IGameIdInfo
    {
        string Amount { get; set; }

        string CreateTime { get; set; }

        string GameDetail { get; set; }

        string GameResult { get; set; }

        string Nickname { get; set; }

        string PeriodNumber { get; set; }

        string PlayId { get; set; }

        string ProfitLoss { get; set; }

        string RoomNumber { get; set; }

        string SerialNumber { get; set; }

        string SettleTime { get; set; }

        string Status { get; set; }

        string Turnover { get; set; }

        string UserId { get; set; }

        bool IsCashOut { get; set; }
    }
}