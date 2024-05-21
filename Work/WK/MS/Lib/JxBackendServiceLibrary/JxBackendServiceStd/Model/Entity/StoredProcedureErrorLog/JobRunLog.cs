using System;

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
