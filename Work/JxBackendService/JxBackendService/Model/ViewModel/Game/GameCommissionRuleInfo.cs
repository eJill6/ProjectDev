using System;
using JxBackendService.Model.Attributes;

namespace JxBackendService.Model.ViewModel.Game
{
    public class SaveCommissionRuleInfo
    {
        [IgnoreRead]
        public int Type { get; set; }
        public int UserID { get; set; }

        public string UserName { get; set; }

        public decimal MinProfitLossRange { get; set; }

        public decimal MaxProfitLossRange { get; set; }

        /// <summary>
        /// 原始分紅比例
        /// </summary>
        public double CommissionPercent { get; set; }

        public bool Visible { get; set; }

        /// <summary>
        /// CommissionPercent * 100, 用於顯示於畫面上
        /// </summary>
        [IgnoreRead]        
        public decimal CommissionValue { get { return (Convert.ToDecimal(CommissionPercent) * 100m); } set { } }
    }

    public class GameCommissionRuleInfo : SaveCommissionRuleInfo
    {
        public int GUID { get; set; }

        [IgnoreRead]
        public string CommissionGroupType { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
