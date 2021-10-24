using JxBackendService.Model.Enums;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.Game
{
    public class UserSelfCommissionApiResult
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string CommissionType { get; set; }

        public string ProcessMonth { get; set; }

        public double CommissionPercent { get; set; }

        /// <summary> 總分紅 </summary>
        public decimal CommissionAmount { get; set; }

        /// <summary> 1=可領取, 2=欠款, 3=已結清, 9=可發放 </summary>
        public int AuditStatus { get; set; }

        /// <summary> 投注額 </summary>
        public decimal ProfitLossMoney { get; set; }

        /// <summary> 盈虧 </summary>
        public decimal DownlineWinMoney { get; set; }

        /// <summary> 下級分紅 </summary>
        public decimal DownlineCommissionAmount { get; set; }

        /// <summary> 淨分紅 </summary>
        public decimal SelfCommissionAmount { get; set; }

        /* <summary> 有效投注人數 (目前還沒用到先拿掉) </summary>*/
        //public int ActiveCount { get; set; }

        /// <summary> 欠款 </summary>
        public decimal DebtAmount { get; set; }

        /// <summary> 貢獻 </summary>
        public decimal Contribute { get; set; }

        /// <summary> 累計貢獻 </summary>
        public decimal TotalContribute { get; set; }

        public decimal DepositFee { get; set; }

    }

    public class UserTeamCommissionApiResult
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string CommissionType { get; set; }

        public string ProcessMonth { get; set; }

        public double CommissionPercent { get; set; }

        /// <summary> 總分紅 </summary>
        public decimal CommissionAmount { get; set; }

        public int AuditStatus { get; set; }

        /// <summary> 投注額 </summary>
        public decimal ProfitLossMoney { get; set; }

        /// <summary> 盈虧 </summary>
        public decimal DownlineWinMoney { get; set; }

        // <summary> 有效投注人數 (目前還沒用到先拿掉) </summary>
        //public int ActiveCount { get; set; }

        /// <summary> 貢獻 </summary>
        public decimal Contribute { get; set; }
    }

    public class SpUserCommissionSelFrontSideResult
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public int ParentID { get; set; }

        public string ParentName { get; set; }

        public string CommissionType { get; set; }

        public decimal ProfitLossMoney { get; set; }

        public decimal PrizeMoney { get; set; }

        public decimal DownlineWinMoney { get; set; }

        public decimal Contribute { get; set; }

        public decimal TotalContribute { get; set; }

        public double CommissionPercent { get; set; }

        public bool Visible { get; set; }

        public decimal CommissionAmount { get; set; }

        public decimal DownlineCommissionAmount { get; set; }

        public decimal SelfCommissionAmount { get; set; }

        public byte AuditStatus { get; set; }

        public string AuditStatusText
        {
            get
            {
                return CommissionAuditStatus.GetName(AuditStatus);
            }
            set { }
        }

        public int ProcessMonth { get; set; }

        public DateTime ProcessTime { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastLoginTime { get; set; }

        public decimal DepositFee { get; set; }
    }

    public class ContributeDetailViewModel
    {        
        /// <summary>
        /// 投注
        /// </summary>
        public string BetAmountText { get; set; }
        
        /// <summary>
        /// 派彩
        /// </summary>
        public string PrizeAmountText { get; set; }
        
        /// <summary>
        /// 虧盈
        /// </summary>
        public string ProfitLossAmountText { get; set; }
        
        /// <summary>
        /// 貢獻
        /// </summary>
        public string ContributeText { get; set; }

        public int ProductType { get; set; }

        public string DisplayName { get; set; }

        public int GroupSort { get; set; }

        public int ProductSort { get; set; }

        public bool IsGroup { get; set; }

        /// <summary>
        /// 是否顯示摺疊開關
        /// </summary>
        public bool IsCollapseVisible { get; set; }
    }
}

