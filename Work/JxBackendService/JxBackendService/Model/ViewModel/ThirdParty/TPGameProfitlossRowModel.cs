using JxBackendService.Common.Util;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public class TPGameProfitLossRowModel : BasicUserInfo
    {
        public string ProfitLossID { get; set; }

        public string ProfitLossType { get; set; }

        public DateTime BetTime { get; set; }
                
        public DateTime ProfitLossTime { get; set; }

        [IgnoreRead]
        public string CompareTimeText { get; set; }

        public decimal ProfitLossMoney { get; set; }

        public decimal WinMoney { get; set; }

        public decimal PrizeMoney { get; set; }

        public string Memo { get; set; }

        /// <summary>
        /// 用於呈現投注金額
        /// </summary>
        [IgnoreRead]
        public string DisplayBetAmountText
        {
            get
            {
                if (ProfitLossType == ProfitLossTypeName.KY)
                {
                    return ProfitLossMoney.ToCurrency();
                }
                else
                {
                    return 0m.ToCurrency();
                }
            }
            set { }
        }

        /// <summary>
        /// 用於呈現盈虧金額
        /// </summary>
        [IgnoreRead]
        public string DisplayProfitlossAmountText
        {
            get
            {
                if (ProfitLossType == ProfitLossTypeName.KY)
                {
                    return WinMoney.ToCurrency();
                }
                else if (ProfitLossType == ProfitLossTypeName.FD)
                {
                    return (ProfitLossMoney + WinMoney).ToCurrency();
                }
                else
                {
                    return ProfitLossMoney.ToCurrency();
                }
            }
            set { }
        }        
    }

    public class TPGameProfitLossRowModelCompareTime : BaseStringValueModel<TPGameProfitLossRowModelCompareTime>
    {
        private TPGameProfitLossRowModelCompareTime() { }

        public static TPGameProfitLossRowModelCompareTime BetTime = new TPGameProfitLossRowModelCompareTime() { Value = nameof(TPGameProfitLossRowModel.BetTime) };
        public static TPGameProfitLossRowModelCompareTime ProfitLossTime = new TPGameProfitLossRowModelCompareTime() { Value = nameof(TPGameProfitLossRowModel.ProfitLossTime) };
    }
}
