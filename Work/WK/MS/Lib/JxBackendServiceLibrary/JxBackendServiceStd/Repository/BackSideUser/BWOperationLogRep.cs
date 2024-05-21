using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.BackSideUser;
using JxBackendService.Model.db;
using JxBackendService.Model.Entity.BackSideUser;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Date;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.BackSideUser
{
    public class BWOperationLogRep : BaseDbRepository<BWOperationLog>, IBWOperationLogRep
    {
        public BWOperationLogRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public PagedResultModel<BWOperationLog> GetPagedOperationLog(QueryBWOperationLogParam queryParam)
        {
            TableSearchDateRange dateRange = GetTableSearchDateRange(queryParam.StartDate, queryParam.EndDate.ToQuerySmallEqualThanTime(DatePeriods.Day));

            StringBuilder commonFilter = new StringBuilder(" ");

            if (!queryParam.OperationType.IsNullOrEmpty())
            {
                commonFilter.AppendLine($" AND OperationType = @OperationType");
            }

            if (!queryParam.OperateUserName.IsNullOrEmpty())
            {
                commonFilter.AppendLine($" AND OperateUserName = @OperateUserName ");
            }

            if (queryParam.UserID.HasValue)
            {
                commonFilter.AppendLine($" AND UserID = @UserID ");
            }

            queryParam.SortModels = new List<SortModel>
            {
                new SortModel { ColumnName = nameof(BWOperationLog.CreateDate), Sort= SortOrder.Descending }
            };

            List<SqlSelectColumnInfo> selectColumnInfos = ModelUtil.GetAllColumnInfos<BWOperationLog>();
            //一律設定成identity,讓底層再建立temp table的時候做轉型
            string operationIDColumnName = "OperationID";
            selectColumnInfos.Where(w => w.AliasName == operationIDColumnName).Single().IsIdentity = true;

            return GetCrossDbQueryList<BWOperationLog>(new JxCrossDbQueryParam()
            {
                SelectColumnInfos = selectColumnInfos,
                InlodbFilters = $"CreateDate >= @{nameof(dateRange.InlodbStartDate)} AND CreateDate < @{nameof(dateRange.SmallThanInlodbEndDate)} " + commonFilter,
                InlodbBakFilters = $"CreateDate >= @{nameof(dateRange.InlodbBakStartDate)} AND CreateDate < @{nameof(dateRange.SmallThanInlodbBakEndDate)} " + commonFilter,
                OrderBy = queryParam.ToOrderByText(),
                PageNo = queryParam.PageNo,
                PageSize = queryParam.PageSize,
                Parameters = new
                {
                    OperateUserName = queryParam.OperateUserName.ToNVarchar(100),
                    queryParam.UserID,
                    OperationType = queryParam.OperationType.ToVarchar(50),
                    queryParam.PageNo,
                    queryParam.PageSize,
                    dateRange.InlodbStartDate,
                    dateRange.SmallThanInlodbEndDate,
                    dateRange.InlodbBakStartDate,
                    dateRange.SmallThanInlodbBakEndDate,
                }
            });
        }
    }
}