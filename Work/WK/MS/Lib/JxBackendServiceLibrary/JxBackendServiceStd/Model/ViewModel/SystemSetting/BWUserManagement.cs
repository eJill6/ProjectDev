using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.Web.BackSideWeb;
using JxBackendService.Model.Attributes;
using JxBackendService.Resource.Element;
using System;
using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.ViewModel
{
    public class BaseBWUserInfo
    {
        public int UserID { get; set; }

        [Display(Name = nameof(DisplayElement.UserName), ResourceType = typeof(DisplayElement))]
        [CustomizedRegularExpression(RegularExpressionEnumTypes.BWUserName)]
        [CustomizedRequired]
        public string UserName { get; set; }

        public int RoleID { get; set; }
    }

    public class EditUserInfo : BaseBWUserInfo
    {
        [Display(Name = nameof(DisplayElement.Password), ResourceType = typeof(DisplayElement))]
        [CustomizedRegularExpression(RegularExpressionEnumTypes.Password)]
        [CustomizedRequired]
        public string Password { get; set; }
    }

    public class UserManagementViewModel : BaseBWUserInfo, IDataKey
    {
        public string RoleName { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateDateText => CreateDate.ToFormatDateTimeString();

        public string KeyContent => UserID.ToNonNullString();
    }
}