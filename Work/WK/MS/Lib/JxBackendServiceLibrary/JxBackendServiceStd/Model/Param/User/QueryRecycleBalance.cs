using JxBackendService.Model.Attributes;
using JxBackendService.Model.Paging;
using JxBackendService.Resource.Element;
using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.Param.User
{
    public class QueryRecycleBalance : BasePagingRequestParam
    {
        [Display(Name = nameof(DisplayElement.AccountID), ResourceType = typeof(DisplayElement))]
        [CustomizedRequired]
        public string AccountID { get; set; }

        [CustomizedRequired]
        public string AccountType { get; set; }
    }
}