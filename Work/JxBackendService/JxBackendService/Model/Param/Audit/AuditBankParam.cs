using JxBackendService.Resource.Element;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Param.Audit
{
    public class AuditBankBaseParam
    {
        public int BankTypeId { get; set; }
        public string BankCard { get; set; }
        public bool IsActive { get; set; }
    }
    public class AuditBankParam : AuditBankBaseParam
    {
        public string CardUser { get; set; }
        public int? SourceBankTypeId { get; set; }
        public string BankTypeName { get; set; }
        [JsonIgnore]
        public string IsActiveText => IsActive ? AuditElement.AuditBankIsActive : AuditElement.AuditBankUnActive;
    }

    public class AuditBankCheckParam : AuditBankBaseParam
    {
    }
}
