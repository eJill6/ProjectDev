using JxBackendService.Model.Attributes;
using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Entity
{
    public class AuditInfo : BaseEntityModel
    {
        public AuditInfo() { }

        public AuditInfo(string id)
        {
            ID = id;
        }

        [ExplicitKey]
        public string ID { get; set; }
        public int AuditType { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string AuditContent { get; set; }
        public string AuditValue { get; set; }
        public string Memo { get; set; }
        public DateTime? AuditTime { get; set; }
        public string Auditor { get; set; }
        public int? AuditStatus { get; set; }
        public string RefTable { get; set; }
        public string RefID { get; set; }
        public string AddtionalAuditValue { get; set; }

        [IgnoreRead]
        [Write(false)]
        public string AuditTypeText { get { return AuditTypeValue.GetSingle(AuditType).Name; } set { AuditTypeText = value; } }
        [IgnoreRead]
        [Write(false)]
        public string AuditStatusText { get { return AuditStatusType.GetSingle(AuditStatus.GetValueOrDefault()).Name; } set { AuditStatusText = value; } }
    }

}
