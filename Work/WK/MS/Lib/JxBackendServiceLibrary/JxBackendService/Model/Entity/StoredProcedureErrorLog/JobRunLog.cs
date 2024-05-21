using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Model.Entity.StoredProcedureErrorLog
{
    public class JobRunLog
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime NowTime { get; set; }
        public string Status { get; set; }
    }
}
