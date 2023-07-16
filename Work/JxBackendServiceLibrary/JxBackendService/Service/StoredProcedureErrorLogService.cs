using JxBackendService.Interface.Repository.StoredProcedureErrorLog;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity.StoredProcedureErrorLog;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Service
{
    public class StoredProcedureErrorLogService : BaseService, IStoredProcedureErrorLogService
    {
        private readonly IProErrorLogsRep _proErrorLogsRep;
        private readonly IJobRunLogRep _jobRunLogRep;

        public StoredProcedureErrorLogService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _proErrorLogsRep = ResolveJxBackendService<IProErrorLogsRep>();
            _jobRunLogRep = ResolveJxBackendService<IJobRunLogRep>();
        }

        public Pro_ErrorLogs GetProErrorLogs(InlodbType inlodbType)
        {
            return _proErrorLogsRep.GetLastSingleProErrorLogs(inlodbType);
        }

        public List<Pro_ErrorLogs> GetProErrorLogsList(InlodbType inlodbType, int errorLogId)
        {
            return _proErrorLogsRep.GetListByErrorLogId(inlodbType, errorLogId);
        }

        public List<JobRunLog> GetJobRunLogStatus(DateTime? lastStartTime)
        {
            return _jobRunLogRep.GetLastSingleJobRunLog(lastStartTime);
        }
    }
}
