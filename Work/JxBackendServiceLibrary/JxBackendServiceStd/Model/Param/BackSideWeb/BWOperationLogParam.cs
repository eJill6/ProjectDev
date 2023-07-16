using System;
using System.ComponentModel.DataAnnotations;
using System.Security;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Param.BackSideWeb
{
    public class CreateBWOperationLogParam : IRequiredParam
    {
        [Required]
        public PermissionKeyDetail PermissionKey { get; set; }

        public int? UserID { get; set; }

        public string ReferenceKey { get; set; }

        [Required]
        public string Content { get; set; }
    }

    public class QueryBWOperationLogParam : BasePagingRequestParam
    {
        public string PermissionKey { get; set; }

        public string OperateUserName { get; set; }

        public int? UserID { get; set; }

        [CustomizedRequired(IsErrorMessageContainField = false)]
        public DateTime StartDate { get; set; }

        [CustomizedRequired(IsErrorMessageContainField = false)]
        public DateTime EndDate { get; set; }
    }
}