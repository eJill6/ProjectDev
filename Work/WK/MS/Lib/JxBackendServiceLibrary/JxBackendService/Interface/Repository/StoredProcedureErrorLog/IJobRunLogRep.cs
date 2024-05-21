using JxBackendService.Model.Entity.StoredProcedureErrorLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Interface.Repository.StoredProcedureErrorLog
{
    public interface IJobRunLogRep
    {
        List<JobRunLog> GetLastSingleJobRunLog(DateTime? lastStartTime);
    }
}
