using System;

namespace SLPolyGame.Web.Model
{
    public class CurrentLotteryQueryInfo
    {
        public int LotteryId { get; set; }

        public int Count { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public string SortBy { get; set; }

        public string StartIssueNo { get; set; }

        public string EndIssueNo { get; set; }

        public int UserID { get; set; }
    }
}