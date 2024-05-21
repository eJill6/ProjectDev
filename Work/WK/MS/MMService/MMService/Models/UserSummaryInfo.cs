using MS.Core.MMModel.Models.Post.Enums;

namespace MMService.Models
{
    public class UserSummaryInfo
    {
        /// <summary>
        /// 總發送數量
        /// </summary>
        public int TotalSend { get; set; }
        /// <summary>
        /// 显示给前台的发帖次数
        /// </summary>
        public int ShowRemainingSend { get; set; }

        /// <summary>
        /// 剩餘發送次數
        /// </summary>
        public int RemainingSend { get; set; }

        /// <summary>
        /// 免費剩餘解次數
        /// </summary>
        public int RemainingFreeUnlock { get; set; }

        /// <summary>
        /// 總收益
        /// </summary>
        public decimal TotalIncome { get; set; }

	}
}