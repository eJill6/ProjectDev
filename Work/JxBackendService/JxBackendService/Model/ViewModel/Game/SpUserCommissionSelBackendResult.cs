using JxBackendService.Common.Util;
using JxBackendService.Model.Entity.Game;
using JxBackendService.Model.Enums;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.Game
{
    public class UserCommissionBackSideViewModel
    {
        public List<SpUserCommissionSelBackendResult> UserCommissions { get; set; }
        public double SumCommissionAmount { get; set; }
    }

    public class BaseSpUserCommissionResult
    {
        public string ProductCode { get; set; }

        public ProductTypes ProductType { get; set; }

        public string DisplayName { get; set; }

        public int GroupSort { get; set; }

        public int ProductSort { get; set; }

        public bool IsGroup { get; set; }
    }

    public class SpUserCommissionSelBackendResult : BaseSpUserCommissionResult
    {
        private decimal _contribute;
        private decimal _downlineWinMoney;
        
        public int UserID { get; set; }

        public string UserName { get; set; }

        public string CommissionType { get; set; }

        /// <summary>
        /// 接收sp參數用,避免欄位名稱混淆,請用來接收就好
        /// </summary>
        public decimal Contribute { get => int.MinValue; set => _contribute = value; } //沒有getter加wcf參考會異常

        public decimal TotalContribute { get; set; }

        /// <summary>
        /// 接收sp參數用,避免欄位名稱混淆,請用來接收就好
        /// </summary>
        public decimal DownlineWinMoney { get => int.MinValue; set => _downlineWinMoney = value; } //沒有getter加wcf參考會異常

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

        public int IsMinus { get; set; }

        public string IsMinusText
        {
            get
            {
                if (IsMinus == (int)IsMinusTypes.HasDebt)
                {
                    return SelectItemElement.IsMinus_HasDebt;
                }
                else if (IsMinus == (int)IsMinusTypes.NoDebt)
                {
                    return SelectItemElement.IsMinus_NoDebt;
                }
                else if (IsMinus == (int)IsMinusTypes.NoDebtByManual)
                {
                    return SelectItemElement.IsMinus_NoDebtByManual;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            set { }
        }

        /// <summary>
        /// 總虧盈,Contribute在SP內轉過負號
        /// </summary>
        public decimal TotalProfitloss { get { return _contribute; } set { } }

        /// <summary>
        /// 下線盈虧,DownlineWinMoney在SP內轉過負號
        /// </summary>
        public decimal DownlineProfitloss { get { return _downlineWinMoney; } set { } }

        public int CommissionReportDataType { get; set; }
        public decimal DepositFee { get; set; }

        /// <summary>
        /// 本級最後領取分紅金額
        /// </summary>
        public decimal FinalSelfCommissionAmount { get { return SelfCommissionAmount - DepositFee; } set { } }            
    }

    public class SpSelSummaryCommissionAmountResult
    {
        public double SumCommissionAmount { get; set; }
        public string ErrCode { get; set; }
        public string ErrMsg { get; set; }
    }

    public class UserCommissionExportViewModel : UserCommissionList
    {
        public string AuditStatusText
        {
            get
            {
                if (AuditStatus.HasValue)
                {
                    return CommissionAuditStatus.GetName(AuditStatus.Value);
                }

                return string.Empty;
            }
            set { }
        }

        public string DisplayName { get; set; }

        public int GroupSort { get; set; }

        public int ProductSort { get; set; }

        public int CommissionReportDataType { get; set; }

        public decimal DepositFee { get; set; }

        /// <summary>
        /// 本級最後領取分紅金額
        /// </summary>
        public decimal FinalSelfCommissionAmount { get { return SelfCommissionAmount.GetValueOrDefault() - DepositFee; } set { } }
    }

    public class CommissionTypeSortInfo
    {
        public string DisplayName { get; set; }

        public int GroupSort { get; set; }

        public int ProductSort { get; set; }

        public bool IsGroup { get; set; }
    }
}
