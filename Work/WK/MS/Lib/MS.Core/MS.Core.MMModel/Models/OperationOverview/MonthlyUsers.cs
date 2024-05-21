using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.OperationOverview
{
    public class MonthlyUsers
    {
        /// <summary>
        /// 月份
        /// </summary>
        public DateTime Month { get; set; }

        /// <summary>
        /// PV (頁面訪問量)
        /// </summary>
        public int PV { get; set; }

        /// <summary>
        /// MAU (月活躍用戶數)
        /// </summary>
        public int MAU { get; set; }
    }
}
