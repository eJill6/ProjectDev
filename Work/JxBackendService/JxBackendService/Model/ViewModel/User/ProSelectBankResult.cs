using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.User
{
    public class ProSelectBankResult
    {
        public int RowNum { get; set; }

        public int BankID { get; set; }

        public int UserID { get; set; }

        public string CardUser { get; set; }

        public int BankTypeID { get; set; }

        public string BankCard { get; set; }

        public DateTime ApplyTime { get; set; }

        public string BankName { get; set; }

        public bool IsActive { get; set; }

        public string LastAuditInfoId { get; set; }

        public bool IsABC { get; set; }

        public int AuditStatus { get; set; }

        public string AuditContent { get; set; }
    }
}
