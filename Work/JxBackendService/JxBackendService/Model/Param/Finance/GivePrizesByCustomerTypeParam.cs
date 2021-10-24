using JxBackendService.Model.Entity.Finance;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Param.Finance
{
    public class GivePrizesByCustomerTypeParam
    {
        /// <summary>用戶帳號</summary>
        public int UserID { get; set; }

        /// <summary cref="Enums.WalletType">錢包類型</summary>
        public WalletType WalletType { get; set; }          

        /// <summary>彩金金額</summary>
        public decimal PrizesMoney { get; set; }

        /// <summary>彩金的流水倍數</summary>
        public decimal? FlowMultiple { get; set; }

        /// <summary>寫入賬變的銀行資訊</summary>
        public BaseBankType BankType { get; set; }

        /// <summary>賬變種類</summary>
        public RefundType RefundTypeParam { get; set; }

        /// <summary>寫入盈虧報表的類型</summary>
        public ProfitLossTypeName ProfitLossType { get; set; }

        /// <summary>多語系結構備註</summary>
        public LocalizationParam MemoJsonParam { get; set; }
    }
}
