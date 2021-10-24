using JxBackendService.Interface.Repository.VIP;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.VIP;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.VIP;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.VIP;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.User;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Repository.VIP
{
    public class VIPUserBonusRep : BaseDbRepository<VIPUserBonus>, IVIPUserBonusRep
    {
        public VIPUserBonusRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public bool IsReceived(int userID, int processToken, int bonusType)
        {
            string sql = $@"SELECT TOP 1 1 
                           {GetFromTableSQL(InlodbType.Inlodb)}
                            WHERE UserID = @userID 
                              AND ProcessToken = @processToken 
                              AND BonusType = @bonusType AND ReceivedStatus = {ReceivedStatus.Received.Value}";

            return DbHelper.QueryFirstOrDefault<int?>(sql, new { userID, processToken, bonusType }).HasValue;
        }

        public PagedResultWithAdditionalData<VIPUserBonus, decimal> GetEntityList(VIPUserBonusQueryParam param, BasePagingRequestParam pageParam)
        {
            string whereSql = QueryString(param);

            pageParam.SortModels = new List<SortModel>() { new SortModel() { ColumnName = nameof(VIPUserBonus.UpdateDate), Sort = System.Data.SqlClient.SortOrder.Descending } };

            BuildPagedSqlQueryParam pagedParamBuilder = new BuildPagedSqlQueryParam()
                                                {
                                                    InlodbType = InlodbType.Inlodb,
                                                    WhereString = whereSql,
                                                    Parameters = param,
                                                    RequestParam = pageParam
                                                };

            PagedSqlQueryParamsModel pagedParam = CreateAllColumnsPagedSqlQueryParams(pagedParamBuilder);

            if (pageParam.PageNo == 0)
            {
                pageParam.PageNo = 1;
            }

            pagedParam.SetPager(pageParam);

            // 計算總額
            string sqlForTotalBonusMoney = $@"SELECT ISNULL(SUM(BonusMoney),0) 
                           {GetFromTableSQL(InlodbType.Inlodb)}
                            {whereSql}";

            return DbHelper.PagedSqlQuery<VIPUserBonus, decimal>(pagedParam, new Model.db.BaseSearchParam { Sql = sqlForTotalBonusMoney , Parameters = param});
        }

        private string QueryString(VIPUserBonusQueryParam param)
        {
            string sql = $" WHERE {nameof(VIPUserBonus.ReceivedDate)} BETWEEN @{nameof(param.StartTime)} AND @{nameof(param.EndTime)}";

            if (param.UserID.HasValue)
            {
                sql += $" AND {nameof(VIPUserBonus.UserID)} = @{nameof(param.UserID)}";
            }
            if (param.BonusType.HasValue && param.BonusType != -1) 
            {
                sql += $" AND {nameof(VIPUserBonus.BonusType)} = @{nameof(param.BonusType)}";
            }
            if (param.VIPLevel.HasValue && param.VIPLevel != -1)
            {
                sql += $" AND {nameof(VIPUserBonus.ReceivedVIPLevel)} = @{nameof(param.VIPLevel)}";
            }
            if (param.ReceivedStatus.HasValue && param.ReceivedStatus != -1)
            {
                sql += $" AND {nameof(VIPUserBonus.ReceivedStatus)} = @{nameof(param.ReceivedStatus)}";
            }

            return sql;
        }

    }
}