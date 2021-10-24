using JxBackendService.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Param.Commission
{
    public class CommissionSearchParam
    {
        public int UserId { get; set; }

        public int ReportType { get; set; }

        public int CommissionType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }

    public class CommissionSearchFrontSideParam : BasePagingRequestParam
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int ReportType { get; set; }

        public int CommissionType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
