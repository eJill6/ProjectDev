namespace ControllerShareLib.Models
{
    public class LotteryRebateInfo
    {
        /// <summary>
        /// 彩種大類編號
        /// </summary>
        public int GameTypeId { get; set; }

        /// <summary>
        /// 彩種編號
        /// </summary>
        public int LotteryId { get; set; }

        /// <summary>
        /// 玩法編號
        /// </summary>
        public int PlayTypeId { get; set; }

        /// <summary>
        /// 子玩法編號
        /// </summary>
        public int PlayTypeRadioId { get; set; }

        /// <summary>
        /// 投注項已及賠率
        /// </summary>
        public Dictionary<string, decimal> NumberOdds { get; set; }

    }
}
