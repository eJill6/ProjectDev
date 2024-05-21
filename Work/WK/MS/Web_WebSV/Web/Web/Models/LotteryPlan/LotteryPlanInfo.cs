namespace Web.Models.LotteryPlan
{
    /// <summary>
    /// 計畫數據
    /// </summary>
    public class LotteryPlanInfo
    {
        /// <summary>
        /// 玩法
        /// </summary>
        public string TypePlan { get; set; }

        /// <summary>
        /// 投注項
        /// </summary>
        public string LotteryBet { get; set; }

        /// <summary>
        /// 目前期號
        /// </summary>
        public string IssueNo { get; set; }

        /// <summary>
        /// 計畫期號(規格書設定兩期)
        /// </summary>
        public string[] IssueNos { get; set; }

        /// <summary>
        /// 結果
        /// </summary>
        public string BetResult { get; set; }

        /// <summary>
        /// 計畫的倍數
        /// </summary>
        public int Multi { get; set; }
    }
}
