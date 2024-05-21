using JxBackendService.Interface.Model.Common;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums.BackSideWeb;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using System;
using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.Param.BackSideWeb
{
    public class BaseBWOperationLogParam : IRequiredParam
    {
        public int? UserID { get; set; }

        public string ReferenceKey { get; set; }

        [Required]
        public string Content { get; set; }
    }

    public class CreateBWOperationLogParam : BaseBWOperationLogParam
    {
        [Required]
        public PermissionKeyDetail PermissionKey { get; set; }
    }

    public class CreateBWOperationLogByTypeParam : BaseBWOperationLogParam
    {
        [Required]
        public OperationType OperationType { get; set; }
    }

    public class QueryBWOperationLogParam : BasePagingRequestParam
    {
        public string OperationType { get; set; }

        public string OperateUserName { get; set; }

        public int? UserID { get; set; }

        [CustomizedRequired(IsErrorMessageContainField = false)]
        public DateTime StartDate { get; set; }

        [CustomizedRequired(IsErrorMessageContainField = false)]
        public DateTime EndDate { get; set; }
    }
}