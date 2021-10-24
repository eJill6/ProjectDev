using JxBackendService.Common.Util;
using JxBackendService.Model.Paging;
using System;

namespace JxBackendService.Model.ViewModel.Game
{
    public class ProfitLossStatColumn
    {
        /// <summary> 充值 </summary>
        public decimal CZProfitLossMoney { get; set; }

        /// <summary> 提現 </summary>
        public decimal TXProfitLossMoney { get; set; }

        /// <summary> 本級抽水 </summary>
        public decimal FDProfitLossMoney { get; set; }

        /// <summary> 投注  </summary>
        public decimal TZProfitLossMoney { get; set; }

        /// <summary> 派獎 </summary>
        public decimal KYProfitLossMoney { get; set; }

        /// <summary> 總虧盈 </summary>
        public decimal ZKYProfitLossMoney { get; set; }

        /// <summary> 上級抽水 </summary>
        public decimal XJFDProfitLossMoney { get; set; }

        /// <summary> 佣金 </summary>
        public decimal YJProfitLossMoney { get; set; }

        /// <summary> 紅包 </summary>
        public decimal HBProfitLossMoney { get; set; }

        public string CZProfitLossMoneyText { get { return CZProfitLossMoney.ToCurrency(); } set { } }

        public string TXProfitLossMoneyText { get { return TXProfitLossMoney.ToCurrency(); } set { } }

        public string FDProfitLossMoneyText { get { return FDProfitLossMoney.ToCurrency(); } set { } }

        public string TZProfitLossMoneyText { get { return TZProfitLossMoney.ToCurrency(); } set { } }

        public string KYProfitLossMoneyText { get { return KYProfitLossMoney.ToCurrency(); } set { } }

        public string ZKYProfitLossMoneyText { get { return ZKYProfitLossMoney.ToCurrency(); } set { } }

        public string XJFDProfitLossMoneyText { get { return XJFDProfitLossMoney.ToCurrency(); } set { } }

        public string YJProfitLossMoneyText { get { return YJProfitLossMoney.ToCurrency(); } set { } }

        public string HBProfitLossMoneyText { get { return HBProfitLossMoney.ToCurrency(); } set { } }
    }

    public class TeamUserTotalProfitloss : ProfitLossStatColumn
    {
        public int DataType { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FullUserPaths { get; set; }
        public int ParentID { get; set; }
    }

    public class TeamUserTotalProfitlossStat
    {
        #region 當頁合計
        public decimal PageTotalCZProfitLossMoney { get; set; }
        public decimal PageTotalTXProfitLossMoney { get; set; }
        public decimal PageTotalFDProfitLossMoney { get; set; }
        public decimal PageTotalTZProfitLossMoney { get; set; }
        public decimal PageTotalKYProfitLossMoney { get; set; }
        public decimal PageTotalZKYProfitLossMoney { get; set; }
        public decimal PageTotalXJFDProfitLossMoney { get; set; }
        public decimal PageTotalYJProfitLossMoney { get; set; }
        public decimal PageTotalHBProfitLossMoney { get; set; }
        #endregion

        public decimal TotalCZProfitLossMoney { get; set; }
        public decimal TotalTXProfitLossMoney { get; set; }
        public decimal TotalFDProfitLossMoney { get; set; }
        public decimal TotalTZProfitLossMoney { get; set; }
        public decimal TotalKYProfitLossMoney { get; set; }
        public decimal TotalZKYProfitLossMoney { get; set; }
        public decimal TotalXJFDProfitLossMoney { get; set; }
        public decimal TotalYJProfitLossMoney { get; set; }
        public decimal TotalHBProfitLossMoney { get; set; }
    }

    public class TeamUserInfo : BasicUserInfo
    {
        public int ParentID { get; set; }
        public string FullUserPaths { get; set; }
    }


    public class ProfitlossUserInfo : TeamUserInfo
    {
        public int DataType { get; set; }
    }

    public class BasicUserProfitlossStat
    {
        public string ProfitLossType { get; set; }
        public decimal TotalProfitLossMoney { get; set; }
        public decimal TotalWinMoney { get; set; }
        public decimal TotalPrizeMoney { get; set; }
    }

    public class UserProfitlossStat : BasicUserProfitlossStat
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FullUserPaths { get; set; }
        public int ParentID { get; set; }
    }
}
