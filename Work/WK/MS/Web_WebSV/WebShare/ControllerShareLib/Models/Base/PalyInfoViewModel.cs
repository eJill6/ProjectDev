namespace ControllerShareLib.Models.Base
{
    public class PalyInfoViewModel
    {
        /// <summary>
        /// 彩种类型
        /// </summary>
        public int? LotteryId { get; set; }

        /// <summary>
        /// 彩种
        /// </summary>
        public string LotteryType { get; set; }

        /// <summary>
        /// 期號
        /// </summary>
        public string IssueNo { get; set; }

        /// <summary>
        /// 玩法（四星，五星）
        /// </summary>
        public string PlayTypeName { get; set; }

        /// <summary>
        /// 单选玩法
        /// </summary>
        public string PlayTypeRadioName { get; set; }
        /// <summary>
        /// 賠率
        /// </summary>
        public string Odds { get; set; }

        /// <summary>
        /// 投注时间
        /// </summary>
        public string NoteTime { get; set; }

        /// <summary>
        /// 投注总额
        /// </summary>
        public string NoteMoneyText { get; set; }

        /// <summary>
        /// 亏赢
        /// </summary>
        public decimal PrizeMoney { get; set; }

        public string PrizeMoneyText { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 狀態顯示文字
        /// </summary>
        public string StatusText { get; set; }
        /// <summary>
        /// 购买号码
        /// </summary>
        public string PalyNum { get; set; }

        /// <summary>
        /// 投注注数
        /// </summary>
        public int? NoteNum { get; set; }
        /// <summary>
        /// 中奖额
        /// </summary>
        public decimal? WinMoney { get; set; }

        public string WinMoneyText { get; set; }

        /// <summary>
        /// 一般/專家
        /// </summary>
        public string PlayModeId { get; set; }

        public int PlayTypeId { get; set; }

        public int PlayTypeRadioId { get; set; }
    }
}