using JxBackendService.Model.Entity.StoredProcedureErrorLog;
using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository.StoredProcedureErrorLog
{
    public interface IProErrorLogsRep
    {
        Pro_ErrorLogs GetLastSingleProErrorLogs(InlodbType inlodbType);

        List<Pro_ErrorLogs> GetListByErrorLogId(InlodbType inlodbType, int errorLogId);
    }
}
