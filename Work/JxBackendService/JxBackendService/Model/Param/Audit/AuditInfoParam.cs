using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Param.Audit
{
    public class AuditInfoParam
    {
        public AuditTypeValue AuditType { get; set; }
        public int UserID { get; set; }
        public string BeforeValue { get; set; }
        public string AuditValue { get; set; }
        public string Memo { get; set; } = "";
        public string RefTable { get; set; }
        public string RefID { get; set; }
        public string AddtionalAuditValue { get; set; }
    }

    public class AuditInfoQueryParam
    {
        public string UserName { get; set; }

        public int? AuditType { get; set; }

        public int? AuditStatus { get; set; }

        public string CreateUser { get; set; }

    }

    public class AuditInfoDealParam
    {
        public string ID { get; set; }
        public int AuditorUserId { get; set; }
        public string AuditorUserName { get; set; }
        public int AuditStatus { get; set; }
        public string Memo { get; set; }

        public string AuditStatus_PassText => AuditElement.AuditPass;

        public string AuditStatus_RejectText => AuditElement.AuditReject;

        public string ReturnCodes_Success => ReturnCode.Success.Value;

        public string ReturnCodes_SystemError => ReturnCode.SystemError.Value;

        public string ReturnCodes_CustomizedMessage => ReturnCode.CustomizedMessage.Value;

        public string ReturnCodes_DataIsExist => ReturnCode.DataIsExist.Value;

        public string ReturnCodes_DataIsNotCompleted => ReturnCode.DataIsNotCompleted.Value;

        public string ReturnCodes_UpdateFailed => ReturnCode.UpdateFailed.Value;

        public string ReturnCodes_UserInitializeIncomplete => ReturnCode.UserInitializeIncomplete.Value;

        public string ReturnCodes_UserAuthenticatorUnbinded => ReturnCode.UserAuthenticatorUnbinded.Value;

        public string ReturnCodes_UserAuthenticatorChanged => ReturnCode.UserAuthenticatorChanged.Value;
    }
}