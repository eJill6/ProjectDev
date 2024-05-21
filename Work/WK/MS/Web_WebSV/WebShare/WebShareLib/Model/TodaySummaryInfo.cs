using System.Collections.Generic;

namespace SLPolyGame.Web.Model
{
    public class TodaySummaryInfo
    {
        public List<SLPolyGame.Web.Model.PalyInfo> Orders { get; set; }

        public List<SLPolyGame.Web.Model.CurrentLotteryInfo> LotteryResults { get; set; }

        public List<SLPolyGame.Web.Model.PlaySummaryModel> PlaySummaryInfoes { get; set; }
    }
}
