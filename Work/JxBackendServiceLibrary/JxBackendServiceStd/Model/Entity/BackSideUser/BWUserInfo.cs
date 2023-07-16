using JxBackendService.Model.Attributes;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.BackSideUser
{
    public class BWUserInfo : BaseEntityModel
    {
        [IdentityKey]
        public int UserID { get; set; }

        [NVarcharColumnInfo(50)]
        public string UserName { get; set; }

        [NVarcharColumnInfo(50)]
        public string Password { get; set; }

        public int RoleID { get; set; }
    }
}