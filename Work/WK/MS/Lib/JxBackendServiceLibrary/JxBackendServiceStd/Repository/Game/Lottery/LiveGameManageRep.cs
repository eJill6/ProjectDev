using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using JxBackendService.Model.Entity;
using JxBackendService.Repository.Extensions;

namespace JxBackendService.Repository.Game.Lottery
{
    public class LiveGameManageRep : BaseDbRepository<LiveGameManage>, ILiveGameManageRep
    {
        public LiveGameManageRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public IEnumerable<LiveGameManage> GetAll()
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb);

            return DbHelper.QueryList<LiveGameManage>(sql, new { });
        }

        public LiveGameManage GetDetail(int no)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + " WHERE No = @No ";

            return DbHelper.QuerySingleOrDefault<LiveGameManage>(sql, new { No = no });
        }

        public PagedResultModel<LiveGameManage> GetPagedAll(LiveGameManageQueryParam param)
        {
            StringBuilder whereString = new StringBuilder(" WHERE 1 = 1 ");

            if (!param.TabType.IsNullOrEmpty())
            {
                whereString.AppendLine($" AND TabType = @TabType");
            }

            if (!param.LotteryType.IsNullOrEmpty())
            {
                whereString.AppendLine($" AND LotteryType LIKE '%' + @LotteryType + '%'");
            }

            if (param.IsActive.HasValue)
            {
                whereString.AppendLine($" AND IsActive = @IsActive");
            }

            PagedSqlQueryParamsModel result = CreateAllColumnsPagedSqlQueryParams(new BuildPagedSqlQueryParam
            {
                InlodbType = InlodbType.Inlodb,
                TableName = "LiveGameManage",
                WhereString = whereString.ToString(),
                Parameters = new
                {
                    TabType = param.TabType.ToInt32Nullable(),
                    LotteryType = param.LotteryType.ToNVarchar(50),
                    param.IsActive,
                },
                RequestParam = new BasePagingRequestParam
                {
                    PageNo = param.PageNo,
                    PageSize = param.PageSize,
                    SortModels = new List<SortModel>
                    {
                        new SortModel { ColumnName = nameof(LiveGameManage.Sort), Sort = SortOrder.Ascending },
                        new SortModel { ColumnName = nameof(LiveGameManage.No), Sort = SortOrder.Ascending }
                    }
                }
            });

            return DbHelper.PagedSqlQuery<LiveGameManage>(result);
        }
    }
}