using Serenity.ComponentModel;
using Serenity.Web;

namespace Management.SystemSettings.Forms
{
    [FormScript("SystemSettings.LotteryInfo")]
    [BasedOnRow(typeof(LotteryInfoRow), CheckNames = false)]
    public class LotteryInfoForm
    {
        public string LotteryType { get; set; }
        public string TypeUrl { get; set; }
        public int GameTypeId { get; set; }
        public int Priority { get; set; }
        public string OfficialLotteryUrl { get; set; }
        public string NumberTrendUrl { get; set; }
        public int Status { get; set; }
        public int DefaultSec { get; set; }
        public int AppPriority { get; set; }
        public int HotNew { get; set; }
        public int MaxBonusMoney { get; set; }
        public string Notice { get; set; }
        public int RecommendSort { get; set; }
    }
}