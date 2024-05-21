using JxBackendService.Interface.Repository.StoredProcedureErrorLog;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity.StoredProcedureErrorLog;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;

namespace JxBackendService.Service
{
    public class StoredProcedureErrorLogService : BaseService, IStoredProcedureErrorLogService
    {
        private readonly Lazy<IProErrorLogsRep> _proErrorLogsRep;

        private readonly Lazy<IJobRunLogRep> _jobRunLogRep;

        public StoredProcedureErrorLogService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _proErrorLogsRep = ResolveJxBackendService<IProErrorLogsRep>();
            _jobRunLogRep = ResolveJxBackendService<IJobRunLogRep>();
        }

        public Pro_ErrorLogs GetProErrorLogs(InlodbType inlodbType)
        {
            return _proErrorLogsRep.Value.GetLastSingleProErrorLogs(inlodbType);
        }

        public List<Pro_ErrorLogs> GetProErrorLogsList(InlodbType inlodbType, int errorLogId)
        {
            return _proErrorLogsRep.Value.GetListByErrorLogId(inlodbType, errorLogId);
        }

        public List<JobRunLog> GetJobRunLogStatus(DateTime? lastStartTime)
        {
            return _jobRunLogRep.Value.GetLastSingleJobRunLog(lastStartTime);
        }
    }
}