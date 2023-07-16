using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using System;

namespace JxBackendService.Model.Param.Game
{
    public class QueryBetRecordParam : BasePagingRequestParam
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string QueryUserName { get; set; }

        public int? QueryUserId { get; set; }

        public string ProductTypeName { get; set; }

        public PlatformProduct ProductType => PlatformProduct.GetSingle(ProductTypeName);
    }
}