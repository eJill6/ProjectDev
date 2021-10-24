using System.Data;
using System.Linq;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.VIP;
using JxBackendService.Model.Entity.VIP;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.VIP;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.VIP
{
    public class VIPUserEventDetailRep : BaseDbRepository<VIPUserEventDetail>, IVIPUserEventDetailRep
    {
        public VIPUserEventDetailRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser,
            dbConnectionType)
        {

        }

        private string QueryString(VIPUserEventQueryParam queryParam)
        {
            string sql = string.Empty;

            if (queryParam.UserID > 0)
            {
                sql += $" AND {nameof(queryParam.UserID)} = @{nameof(queryParam.UserID)}";
            }

            if (queryParam.EventTypeID > 0)
            {
                sql += $" AND {nameof(queryParam.EventTypeID)} = @{nameof(queryParam.EventTypeID)}";
            }

            if (queryParam.AuditStatus.HasValue)
            {
                sql += $" AND {nameof(queryParam.AuditStatus)} = @{nameof(queryParam.AuditStatus)}";
            }

            if (queryParam.ActivityStartDate.HasValue)
            {
                sql += $" AND CreateDate >= @{nameof(queryParam.ActivityStartDate)}";
            }

            if (queryParam.ActivityEndDate.HasValue)
            {
                sql += $" AND CreateDate < @{nameof(queryParam.ActivityEndDate)}";
            }

            return sql;
        }

        /// <summary>
        /// 有無未審核活動單
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="eventTypeID"></param>
        /// <returns></returns>
        public bool HasAuditUnprocessedActivity(int userId, int eventTypeID)
        {
            var queryParam = new VIPUserEventQueryParam 
            {
                UserID = userId,
                EventTypeID = eventTypeID,
                AuditStatus = AuditStatusType.Unprocessed.Value,
            };

            string whereSql = " WHERE 1 = 1 " + QueryString(queryParam);
            string sql = $@"
SELECT TOP 1 1
{GetFromTableSQL(InlodbType.Inlodb)} 
{whereSql} ";

            return DbHelper.ExecuteScalar<int?>(sql, queryParam).HasValue;
        }

        public VIPUserEventAuditStat GetVIPUserEventAuditStat(VIPUserEventQueryParam queryParam)
        {
            string whereSql = " WHERE 1 = 1 " + QueryString(queryParam);
            string sql = $@"
SELECT COUNT(0) AuditTotalCount, SUM(BonusAmount) BonusTotalAmount
{GetFromTableSQL(InlodbType.Inlodb)} 
{whereSql} ";

            return DbHelper.QuerySingleOrDefault<VIPUserEventAuditStat>(sql, queryParam);
        }

        public bool HasAuditRefIDActivity(CheckUserEventRefIDParam refIDParam)
        {
            var queryParam = refIDParam.CastByJson<VIPUserEventQueryParam>();

            string whereSql = " WHERE 1 = 1 " + QueryString(queryParam);            
            string sql = $@"
SELECT TOP 1 1
{GetFromTableSQL(InlodbType.Inlodb)} 
{whereSql} AND RefID = @{nameof(refIDParam.RefID)} ";

            return DbHelper.ExecuteScalar<int?>(sql, refIDParam).HasValue;
        }

        public BaseReturnModel ProcessVIPUserEventAudit(ProcessEventAuditParam auditParam)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_VIP_ProcessUserEventAudit";

            var returnCode = DbHelper.ExecuteScalar<string>(sql, auditParam, CommandType.StoredProcedure);

            return new BaseReturnModel(ReturnCode.GetSingle(returnCode));
        }
    }
}