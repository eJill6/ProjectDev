using SLPolyGame.Web.MSSeal.Models.Interface;

namespace SLPolyGame.Web.MSSeal.Models
{
    public class GameBalance : IGameBalance
    {
        public string Type { get; set; }

        public string SubType { get; set; }

        public string GameId { get; set; }

        public string TPGameAccount { get; set; }

        public decimal Balance { get; set; }

        public decimal FreezeBalance { get; set; }

        public string GameName { get; set; }
    }
}