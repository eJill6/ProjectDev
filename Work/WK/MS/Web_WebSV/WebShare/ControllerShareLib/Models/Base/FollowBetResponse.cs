namespace ControllerShareLib.Models.Base
{
    //K3回傳結果
    public class FollowBetResponse
    {
        /// <summary>
        /// 投注數量
        /// </summary>
        public int TotalBetCount { get; set; }

        /// <summary>
        /// 總盈虧
        /// </summary>
        public decimal TotalWinMoney { get; set; }

        /// <summary>
        /// 派彩總金額
        /// </summary>
        public decimal TotalPrizeMoney { get; set; }

        /// <summary>
        /// 下一頁指標
        /// </summary>
        public string NextCursor { get; set; }

        /// <summary>
        /// 注單詳細資料
        /// </summary>
        public List<PalyInfoViewModel> DataDetail { get; set; }

        public FollowBetResponse()
        {
            DataDetail = new List<PalyInfoViewModel>();
        }
    }
}