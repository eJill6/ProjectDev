using System;

namespace JxBackendService.Model.Entity
{
    public partial class UserBankInfo
    {
        public int BankID { get; set; }
        public int UserID { get; set; }
        public string CardUser { get; set; }
        public int BankTypeID { get; set; }
        public string BankCard { get; set; }
        public DateTime? ApplyTime { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string MobileNo { get; set; }
        public string SiteName { get; set; }
        public bool IsActive { get; set; }
        public string BankTypeName { get; set; }
    }
}
