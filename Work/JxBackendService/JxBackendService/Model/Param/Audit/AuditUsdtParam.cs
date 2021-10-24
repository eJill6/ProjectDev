using JxBackendService.Resource.Element;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Param.Audit
{
    public class AuditBaseParam
    {
        public string WalletAddr { get; set; }
        public bool IsActive { get; set; }
    }


    public class AuditUsdtParam : AuditBaseParam
    {
        public int BlockChainID { get; set; }
        [JsonIgnore]
        public string IsActiveText => IsActive ? AuditElement.AuditBankIsActive : AuditElement.AuditBankUnActive;

    }

    public class AuditUsdtCheckParam : AuditBaseParam
    {

    }
}
