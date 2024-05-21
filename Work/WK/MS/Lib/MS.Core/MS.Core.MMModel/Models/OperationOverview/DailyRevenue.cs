using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.OperationOverview
{
    public class DailyRevenue
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// DAU (日活躍用戶數)
        /// </summary>
        public int DAU { get; set; }

        /// <summary>
        /// PU (付費用戶數)
        /// </summary>
        public int PU { get; set; }

        /// <summary>
        /// 付費率
        /// </summary>
        public double PayingRate { get; set; }

        /// <summary>
        /// 收益
        /// </summary>
        public double Revenue { get; set; }

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
