using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Param.Audit
{
    public class AuditGivePrizeParam
    {
        public string ProfitLossType { get; set; }

        public string BankName { get; set; }

        public int? BankTypeId { get; set; }

        public decimal Amount { get; set; }

        public int WalletType { get; set; }

        public decimal? FlowMultiple { get; set; }

        public int RefundType { get; set; }
        
        public string RefundTypeName { get; set; }
        
        public string Memo { get; set; }
        
        public string MemoJson { get; set; }
    }
}
