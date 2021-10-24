using JxBackendService.Resource.Element;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Param.Audit
{
    public class AuditUserDataBaseParam
    {
        public string Content { get; set; }
    }

    public class AuditUserDataParam : AuditUserDataBaseParam
    {
        public int ModifyUserDataType { get; set; }

        public string EncryptContent { get; set; }

        public string OldEncryptContent { get; set; }

        public string AuditPassReturnMessage { get; set; }
    }

    public class AuditUserDataCheckParam : AuditUserDataBaseParam
    {

    }
}
