using JxBackendService.Model.Entity.StoredProcedureErrorLog;
using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service
{
    public interface IStoredProcedureErrorLogService
    {
        Pro_ErrorLogs GetProErrorLogs(InlodbType inlodbType);

        List<Pro_ErrorLogs> GetProErrorLogsList(InlodbType inlodbType, int errorLogId);

        List<JobRunLog> GetJobRunLogStatus(DateTime? lastStartTime);
    }
}