using JxBackendService.Common.Util;
using JxBackendService.Model.Attributes;

namespace JxBackendService.Model.Entity.Game.Lottery
{
    public class LotteryInfo
    {
        [IdentityKey]
        public int LotteryID { get; set; }

        public string LotteryType { get; set; }

        public string TypeURL { get; set; }

        public int GameTypeID { get; set; }

        public int Priority { get; set; }

        public string OfficialLotteryUrl { get; set; }

        public string NumberTrendUrl { get; set; }

        public int Status { get; set; }

        public int Default_Sec { get; set; }

        public int APPPriority { get; set; }

        public int HotNew { get; set; }

        public int MaxBonusMoney { get; set; }

        public string Notice { get; set; }

        public int RecommendSort { get; set; }
    }
}