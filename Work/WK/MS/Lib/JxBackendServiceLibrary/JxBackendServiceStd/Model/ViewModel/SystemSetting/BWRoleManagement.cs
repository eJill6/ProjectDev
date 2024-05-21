using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.Web.BackSideWeb;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Resource.Element;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.ViewModel
{
    public class BaseBWRoleInfo
    {
        public int RoleID { get; set; }

        [Display(Name = nameof(DisplayElement.RoleName), ResourceType = typeof(DisplayElement))]
        [CustomizedRequired, CustomizedMaxLength(50)]
        public string RoleName { get; set; }
    }

    public class RoleManagementViewModel : BaseBWRoleInfo, IDataKey
    {
        public string KeyContent => RoleID.ToNonNullString();
    }

    public class EditPermissionKey
    {
        public string PermissionKey { get; set; }

        public int AuthorityType { get; set; }
    }

    public class EditRolePermission : BaseBWRoleInfo
    {
        public List<PermissionInfo> UserRolePermissonInfos { get; set; }
    }

    public class AuthorityInfo
    {
        public int AuthorityType { get; set; }

        public bool IsChecked { get; set; }

        public bool IsDisplay { get; set; }
    }

    public class PermissionInfo
    {
        public MenuType MenuType { get; set; }

        public PermissionKeyDetail PermissionKey { get; set; }

        public List<AuthorityInfo> AuthorityInfo { get; set; }
    }
}