using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.BackSideWeb
{
    public class GivePrizesProcessParam
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public decimal Money { get; set; }

        public string ProfitLossType { get; set; }

        public int? BankTypeId { get; set; }

        public string BankName { get; set; }

        public string Memo { get; set; }

        public int WalletTypeValue { get; set; }

        public decimal? FlowMultiple { get; set; }
    }
}
