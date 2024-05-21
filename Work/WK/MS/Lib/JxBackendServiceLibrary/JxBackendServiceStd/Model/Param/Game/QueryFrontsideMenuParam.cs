using JxBackendService.Model.Attributes;
using JxBackendService.Model.Paging;
using System;

namespace JxBackendService.Model.Param.Game
{
    public class QueryFrontsideMenuParam : BasePagingRequestParam
    {
        public string MenuName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [CustomizedRequired(IsErrorMessageContainField = false)]
        public int? MinSort { get; set; }

        [CustomizedRequired(IsErrorMessageContainField = false)]
        public int? MaxSort { get; set; }

        public string TypeValue { get; set; }
    }
}