using JxBackendService.Interface.Repository.StoredProcedureErrorLog;
using JxBackendService.Model.Entity.StoredProcedureErrorLog;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.StoredProcedureErrorLog
{
    public class Pro_ErrorLogsRep : BaseDbRepository<Pro_ErrorLogs>, IProErrorLogsRep
    {
        public Pro_ErrorLogsRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public Pro_ErrorLogs GetLastSingleProErrorLogs(InlodbType inlodbType)
        {
            return BaseGetProErrorLogs(inlodbType,
                topRow: 1,
                startErrorTime: DateTime.Now.AddDays(-1),
                endErrorTime: DateTime.Now,
                errorLogId: null).SingleOrDefault();
        }

        public List<Pro_ErrorLogs> GetListByErrorLogId(InlodbType inlodbType, int errorLogId)
        {
            return BaseGetProErrorLogs(inlodbType,
                topRow: null,
                startErrorTime: null,
                endErrorTime: null,
                errorLogId: errorLogId).ToList();
        }
        private List<Pro_ErrorLogs> BaseGetProErrorLogs(InlodbType inlodbType, int? topRow, DateTime? startErrorTime,
            DateTime? endErrorTime, int? errorLogId)
        {
            StringBuilder sql = new StringBuilder(GetAllQuerySQL(
              inlodbType,
              topRow,
              new List<string>() { "ErrorLogId", "ErrorProcedure", "ErrorMessage" }) +
              @"WHERE 1 = 1 ");

            if (startErrorTime.HasValue)
            {
                sql.AppendLine("AND ErrorTime > @startErrorTime ");
            }

            if (endErrorTime.HasValue)
            {
                sql.AppendLine("AND ErrorTime < @endErrorTime ");
            }

            if (errorLogId.HasValue)
            {
                sql.AppendLine("AND ErrorLogId > @errorLogId ");
            }

            sql.AppendLine("ORDER BY ErrorLogId DESC ");

            return DbHelper.QueryList<Pro_ErrorLogs>(sql.ToString(), new { startErrorTime, endErrorTime, errorLogId });
        }
    }
}
