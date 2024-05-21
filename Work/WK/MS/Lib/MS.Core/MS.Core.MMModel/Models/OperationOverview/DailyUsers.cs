using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.OperationOverview
{
    public class DailyUsers
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// PV (頁面訪問量)
        /// </summary>
        public int PV { get; set; }

        /// <summary>
        /// DAU (日活躍用戶數)
        /// </summary>
        public int DAU { get; set; }

        /// <summary>
        /// PCU (同時在線用戶數)
        /// </summary>
        public int PCU { get; set; }

        /// <summary>
        /// ACU (平均同時在線用戶數)
        /// </summary>
        public double ACU { get; set; }

        /// <summary>
        /// DUN (每用戶平均訪問頁面數)
        /// </summary>
        public double DUN { get; set; }
    }
}
