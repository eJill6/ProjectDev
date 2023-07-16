using JxBackendService.Model.Attributes;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.BackSideUser
{
    public class BWRolePermission : BaseEntityModel
    {
        [ExplicitKey]
        public int RoleID { get; set; }

        [ExplicitKey, VarcharColumnInfo(50)]
        public string PermissionKey { get; set; }

        [ExplicitKey]
        public int AuthorityType { get; set; }
    }
}