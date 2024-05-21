using MS.Core.MMModel.Models.Post.Enums;
using System.Collections.Generic;

namespace MS.Core.MMModel.Models.My
{
    public class UserSummaryViewModel
    {
        /// <summary>
        /// 總發送數量
        /// </summary>
        public int TotalSend { get; set; }
        /// <summary>
        /// 前台显示的发帖次数
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

        /// <summary>
        /// 剩餘發送廣場次數
        /// </summary>
        public int RemainingSendSquare => RemainingSend;
	}
}
