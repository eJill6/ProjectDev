using JxBackendService.Common.Util;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using System;
using System.Collections.Generic;

namespace JxBackendService.Model.Param.TransferRecord
{
    public class SearchTransferRecordParam : BasePagingRequestParam
    {
        public int? UserID { get; set; }

        public short? OrderStatus { get; set; }

        public int? TransferType { get; set; }

        public string ProductCode { get; set; }

        [CustomizedRequired(IsErrorMessageContainField = false)]
        public DateTime StartDate { get; set; }

        [CustomizedRequired(IsErrorMessageContainField = false)]
        public DateTime EndDate { get; set; }

        public DateTime QueryEndDate => EndDate.ToQuerySmallEqualThanTime(DatePeriods.Day);
    }

    public class QueryTPTransferRecordParam : SearchTransferRecordParam
    {
        public List<PlatformProduct> PlatformProducts { get; set; }
    }

    public class QueryPlatformTransferRecordParam : SearchTransferRecordParam
    {
        public int? DealType { get; set; }

        public PlatformProduct PlatformProduct { get; set; }
    }
}