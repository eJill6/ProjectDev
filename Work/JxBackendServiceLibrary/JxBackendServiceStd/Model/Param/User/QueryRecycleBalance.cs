using JxBackendService.Model.Attributes;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Game;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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