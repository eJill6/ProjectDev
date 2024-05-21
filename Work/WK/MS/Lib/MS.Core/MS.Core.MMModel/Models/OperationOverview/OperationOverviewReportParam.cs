using MS.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.OperationOverview
{
    public class OperationOverviewReportParam : PageParam
    {
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
