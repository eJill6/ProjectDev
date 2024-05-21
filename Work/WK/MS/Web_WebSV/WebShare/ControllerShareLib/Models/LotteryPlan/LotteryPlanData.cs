namespace ControllerShareLib.Models.LotteryPlan
{
    /// <summary>
    /// 計畫數據
    /// </summary>
    public class LotteryPlanData
    {        /// <summary>
             /// 當期期號
             /// </summary>
        public string CurrentIssueNo { get; set; }

        /// <summary>
        /// 彩种ID
        /// </summary>
        public int LotteryID { get; set; }

        /// <summary>
        /// 當前跟單計畫
        /// </summary>
        public LotteryPlanInfo CurrentLotteryPlan { get; set; }

        /// <summary>
        /// 跟單數據
        /// </summary>
        public LotteryPlanInfo[] LotteryPlayInfos { get; set; } = new LotteryPlanInfo[] { };

        /// <summary>
        /// 開獎時間 (拿來比對資料新舊用)
        /// </summary>
        public DateTime LotteryTime { get; set; }
    }
}
