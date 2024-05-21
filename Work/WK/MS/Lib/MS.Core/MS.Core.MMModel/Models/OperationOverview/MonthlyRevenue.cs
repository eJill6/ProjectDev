using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.OperationOverview
{
    public class MonthlyRevenue
    {
        /// <summary>
        /// 月份
        /// </summary>
        public DateTime Month { get; set; }

        /// <summary>
        /// MAU (月活躍用戶數)
        /// </summary>
        public int MAU { get; set; }

        /// <summary>
        /// 付費用戶數
        /// </summary>
        public int PU { get; set; }

        /// <summary>
        /// 付費率
        /// </summary>
        public double PayingRate { get; set; }

        /// <summary>
        /// 總收益
        /// </summary>
        public double TotalRevenue { get; set; }

        /// <summary>
        /// ARPU (平均每用戶收益)
        /// </summary>
        public double ARPU { get; set; }

        /// <summary>
        /// ARPPU (平均每付費用戶收益)
        /// </summary>
        public double ARPPU { get; set; }

        /// <summary>
        /// 保證金金額
        /// </summary>
        public double DepositAmount { get; set; }

        /// <summary>
        /// 保證金筆數
        /// </summary>
        public int DepositCount { get; set; }
    }
}
