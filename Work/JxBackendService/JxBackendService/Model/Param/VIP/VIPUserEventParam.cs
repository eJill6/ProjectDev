using System;
using JxBackendService.Model.Enums;

namespace JxBackendService.Model.Param.VIP
{
    public class BaseActivityParam
    {
        public int UserID { get; set; }
        public int EventTypeID { get; set; }
        public DateTime? ActivityStartDate { get; set; }
        public DateTime? ActivityEndDate { get; set; }
    }

    public class VIPUserEventQueryParam : BaseActivityParam
    {
        public int? AuditStatus { get; set; }
    }

    public class VIPUserEventAuditStat
    {
        /// <summary> 通過審核總筆數 </summary>
        public int AuditTotalCount { get; set; }
        /// <summary> 通過審核紅利總金額 </summary>
        public decimal BonusTotalAmount { get; set; }
    }

    public class CheckMonthlyDepositParam : BaseActivityParam
    {
        public DateTime OrderStartDate { get; set; }
        public DateTime OrderEndDate { get; set; }
    }

    public class CheckUserEventRefIDParam : BaseActivityParam
    {
        public string RefID { get; set; }
    }

    public class CreateVIPUserEventAuditParam
    {
        public int EventTypeID { get; set; }
        public int AuditStatus { get; set; }
        /// <summary> 用戶當下VIP等級 </summary>
        public int CurrentLevel { get; set; }
        /// <summary> 申請金額 </summary>
        public decimal ApplyAmount { get; set; }
        /// <summary> 紅利金額 </summary>
        public decimal BonusAmount { get; set; }
        /// <summary> 關連代碼 </summary>
        public string RefID { get; set; }
        /// <summary> 活動參與金額 </summary>
        public decimal EventAmount { get; set; }
        /// <summary> 流水倍數 </summary>
        public decimal FlowMultiple { get; set; }
        /// <summary> 流水 </summary>
        public decimal FinancialFlowAmount { get; set; }
        /// <summary> 多語系結構備註 </summary>
        public string MemoJson { get; set; }
    }

    public class BacksideEventAuditParam
    {
        public string SEQID { get; set; }
        public int AuditStatus { get; set; }
        public string AuditMemo { get; set; }
    }

    public class ProcessEventAuditParam : BacksideEventAuditParam
    {
        public int AuditorUserID { get; set; }
        public string Auditor { get; set; }
        public string OperationLogContent { get; set; }
        public string RC_Success => ReturnCode.Success.Value;
        public string RC_UpdateFailed => ReturnCode.UpdateFailed.Value;
        public string RC_SystemError => ReturnCode.SystemError.Value;
    }
}