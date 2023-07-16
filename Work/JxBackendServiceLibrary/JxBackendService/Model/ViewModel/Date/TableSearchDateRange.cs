using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.Date
{
    public class TableSearchDateRange
    {
        public DateTime? InlodbStartDate { get; set; }
        public DateTime? SmallThanInlodbEndDate { get; set; }
        public DateTime? InlodbBakStartDate { get; set; }
        public DateTime? SmallThanInlodbBakEndDate { get; set; }
    }
}
