using System;

namespace SLPolyGame.Web.Model
{
    public class GiftBagLog
    {
        public int ID { get; set; }
        public decimal? PrizeMoney { get; set; }
        public int? UserID { get; set; }
        public string UserName { get; set; }
        public string Msg { get; set; }
        public DateTime? RiseTime { get; set; }
        public DateTime? Processtime { get; set; }
        public int? ActType { get; set; }
        public int? Status { get; set; }
        public string Memo { get; set; }
        public int? SubUserID { get; set; }
    }
}
