using JxBackendService.Interface.Repository.StoredProcedureErrorLog;
using JxBackendService.Model.Entity.StoredProcedureErrorLog;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;

namespace JxBackendService.Repository.StoredProcedureErrorLog
{
    public class JobRunLogRep : BaseDbRepository<JobRunLog>, IJobRunLogRep
    {
        private readonly int _intervalMinutes = 3;
        
        public JobRunLogRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public List<JobRunLog> GetLastSingleJobRunLog(DateTime? lastStartTime)
        {
            string sql = $@" 
                SELECT TOP 50
                    Status,
                    StartTime
                FROM {InlodbType.InlodbBak}.dbo.JobRunLog WITH(NOLOCK)
                WHERE StartTime > @lastStartTime
	                AND NowTime < DATEADD(MINUTE, -@intervalMinute, GETDATE())
                ORDER BY StartTime DESC  ";

            return DbHelper.QueryList<JobRunLog>(sql, new { lastStartTime, intervalMinute = _intervalMinutes });
        }
    }
}
