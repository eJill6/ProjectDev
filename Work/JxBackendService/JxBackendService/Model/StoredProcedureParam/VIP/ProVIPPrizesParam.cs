using System;
using JxBackendService.Model.Enums;

namespace JxBackendService.Model.StoredProcedureParam.VIP
{
    public class ProVIPPrizesParam
    {
        public int UserID { get; set; }

        public string Handle { get; set; }

        public decimal Money { get; set; }

        public int BonusType { get; set; }

        public int ProcessToken { get; set; }

        public string MemoJson { get; set; }

        public string CreateUser { get; set; }

        public int ReceivedStatus { get; set; }

        public DateTime ReceivedDate { get; set; }

        public int UserBonusActType { get; set; }

        public string RC_Success => ReturnCode.Success.Value;

        public string RC_SystemError => ReturnCode.SystemError.Value;
    }
}