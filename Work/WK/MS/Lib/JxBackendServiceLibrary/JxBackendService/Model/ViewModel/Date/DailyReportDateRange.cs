using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.Date
{
    public class DailyReportDateRange
    {
        public DateTime? InlodbStartTime { get; set; }
        public DateTime? InlodbEndTime { get; set; }
        public DateTime? DailyStartDate { get; set; }
        public DateTime? SmallThanDailyEndDate { get; set; }
    }
}
