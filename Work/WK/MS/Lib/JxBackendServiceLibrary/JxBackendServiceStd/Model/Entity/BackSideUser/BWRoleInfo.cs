using JxBackendService.Model.Attributes;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.BackSideUser
{
    public class BWRoleInfo : BaseEntityModel
    {
        [IdentityKey]
        public int RoleID { get; set; }

        [NVarcharColumnInfo(50)]
        public string RoleName { get; set; }
    }
}