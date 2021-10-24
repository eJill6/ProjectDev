using System;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.VIP
{
    public class VIPUserBonus : BaseEntityModel
    {
        [ExplicitKey]
        public int UserID { get; set; }

        [ExplicitKey]
        public int ProcessToken { get; set; }

        [ExplicitKey]
        public int BonusType { get; set; }

        public string MemoJson { get; set; }

        public DateTime? ReceivedDate { get; set; }

        public int ReceivedStatus { get; set; }

        public decimal BonusMoney { get; set; }

        public int ReceivedVIPLevel { get; set; }
    }
}