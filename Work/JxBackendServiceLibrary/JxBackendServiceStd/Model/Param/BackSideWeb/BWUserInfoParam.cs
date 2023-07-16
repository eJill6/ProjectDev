using JxBackendService.Interface.Model.Common;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Paging;
using JxBackendService.Resource.Element;
using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.Param.BackSideWeb
{
    public class BaseBWUserInfoParam
    {
        [Display(Name = nameof(DisplayElement.Password), ResourceType = typeof(DisplayElement))]
        [CustomizedRegularExpression(RegularExpressionEnumTypes.Password)]
        [CustomizedRequired]
        public string Password { get; set; }

        public int RoleID { get; set; }
    }

    public class CreateBWUserInfoParam : BaseBWUserInfoParam, IRequiredParam
    {
        [Display(Name = nameof(DisplayElement.UserName), ResourceType = typeof(DisplayElement))]
        [CustomizedRegularExpression(RegularExpressionEnumTypes.BWUserName)]
        [CustomizedRequired]
        public string UserName { get; set; }
    }

    public class UpdateBWUserInfoParam : BaseBWUserInfoParam, IRequiredParam
    {
        public int UserID { get; set; }
    }

    public class QueryBWUserInfoParam : BasePagingRequestParam
    {
        public string UserName { get; set; }
    }
}