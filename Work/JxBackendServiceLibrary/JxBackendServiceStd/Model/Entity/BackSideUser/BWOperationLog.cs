using JxBackendService.Model.Attributes;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.BackSideUser
{
    public class BWOperationLog : BaseEntityModel
    {
        [IdentityKey]
        public int OperationID { get; set; }

        [VarcharColumnInfo(50)]
        public string PermissionKey { get; set; }

        [NVarcharColumnInfo(50)]
        public string OperateUserName { get; set; }

        [VarcharColumnInfo(50)]
        public string ReferenceKey { get; set; }

        [NVarcharColumnInfo(2000)]
        public string Content { get; set; }

        public int? UserID { get; set; }
    }
}