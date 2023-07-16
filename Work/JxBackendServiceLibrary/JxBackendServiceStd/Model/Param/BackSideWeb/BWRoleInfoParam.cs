using System.Collections.Generic;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;

namespace JxBackendService.Model.Param.BackSideWeb
{
    public class BaseBWRoleInfoParam
    {
        public string RoleName { get; set; }
    }

    public class CreateRoleInfoParam : BaseBWRoleInfoParam
    {
    }

    public class UpdateRolePermissionParam : BaseBWRoleInfoParam
    {
        public int RoleID { get; set; }

        public List<RolePermissionInfo> PermissionKeys { get; set; } = new List<RolePermissionInfo>();
    }

    public class RolePermissionInfo
    {
        public string PermissionKey { get; set; }

        public int AuthorityType { get; set; }
    }

    public class ProSaveBWRolePermission
    {
        public string CreateUser { get; set; }

        public int RoleID { get; set; }

        public string PermissionAuthTypeJson { get; set; }

        [VarcharColumnInfo(6)]
        public string RC_Success => ReturnCode.Success.Value;

        [VarcharColumnInfo(6)]
        public string RC_UpdateFailed => ReturnCode.UpdateFailed.Value;

        [VarcharColumnInfo(6)]
        public string RC_SystemError => ReturnCode.SystemError.Value;
    }

    public class QueryBWRoleInfoParam : BasePagingRequestParam
    {
    }
}