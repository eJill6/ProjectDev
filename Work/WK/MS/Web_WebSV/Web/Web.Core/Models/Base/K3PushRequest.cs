namespace Web.Models.Base
{
    public class K3PushRequest
    {
        public string Account { get; set; }
        public string LotteryType { get; set; }
        public decimal? BetAmount { get; set; }
        public long PlayId { get; set; }
        public string RoomId { get; set; }
        public int? UserID { get; set; }
    }
}