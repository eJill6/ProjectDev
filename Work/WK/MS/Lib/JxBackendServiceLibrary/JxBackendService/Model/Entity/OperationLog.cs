using System;
using System.ComponentModel.DataAnnotations;
using JxBackendService.Model.Attributes;

namespace JxBackendService.Model.Entity
{
    public class OperationLog
    {
        [Key]
        public int SysNo { get; set; }
        public int Category { get; set; }
        [VarcharColumnInfo(100)]
        public string ReferenceKey { get; set; }
        [NVarcharColumnInfo(2000)]
        public string Content { get; set; }
        public int OperateUserSysNo { get; set; }
        [NVarcharColumnInfo(100)]
        public string OperateUserName { get; set; }
        public DateTime? OperateDate { get; set; }
        public int UserId { get; set; }
        [NVarcharColumnInfo(50)]
        public string UserName { get; set; }
    }
}
