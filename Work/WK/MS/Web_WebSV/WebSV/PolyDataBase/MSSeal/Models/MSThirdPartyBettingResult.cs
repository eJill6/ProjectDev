using SLPolyGame.Web.MSSeal.Models.Interface;

namespace SLPolyGame.Web.MSSeal.Models
{
    public class MSThirdPartyBettingResult : IMSThirdPartyBettingResult
    {
        public string Amount { get; set; }

        public string CreateTime { get; set; }

        public string GameDetail { get; set; }

        public string GameName { get; set; }

        public string GameResult { get; set; }

        public string Nickname { get; set; }

        public string PeriodNumber { get; set; }

        public string PlayId { get; set; }

        public string ProfitLoss { get; set; }

        public string RoomNumber { get; set; }

        public string SerialNumber { get; set; }

        public string SettleTime { get; set; }

        public string Status { get; set; }

        public string Turnover { get; set; }

        public string UserId { get; set; }

        public string Type { get; set; }

        public string SubType { get; set; }

        public string GameId { get; set; }

        public bool IsCashOut { get; set; }
    }
}