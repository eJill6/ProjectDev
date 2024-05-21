using JxBackendService.Model.Entity.StoredProcedureErrorLog;
using System;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository.StoredProcedureErrorLog
{
    public interface IJobRunLogRep
    {
        List<JobRunLog> GetLastSingleJobRunLog(DateTime? lastStartTime);
    }
}
