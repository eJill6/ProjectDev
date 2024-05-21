using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.Game.Lottery;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Repository.Game.Lottery
{
    public class CurrentLotteryInfoRep : BaseDbRepository<CurrentLotteryInfo>, ICurrentLotteryInfoRep
    {
        public CurrentLotteryInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }
        public PagedResultModel<CurrentLotteryInfo> GetCurrentLotteryInfoReport(CurrentLotteryParam param)
        {
            StringBuilder whereString = new StringBuilder(" WHERE 1 = 1 ");
            if (param.LotteryID.HasValue)
            {
                whereString.AppendLine($" AND LotteryID = @LotteryID");
            }
            if (!param.IssueNo.IsNullOrEmpty())
            {
                whereString.AppendLine($" AND IssueNo = @IssueNo");
            }
            if (param.IsLottery.HasValue)
            {
                whereString.AppendLine($" AND IsLottery = @IsLottery");
            }
            whereString.AppendLine($" AND CurrentLotteryTime >= @StartDate AND CurrentLotteryTime <= @EndDate ");
            PagedSqlQueryParamsModel result = CreateAllColumnsPagedSqlQueryParams(new BuildPagedSqlQueryParam
            {
                InlodbType = InlodbType.InlodbBak,
                TableName = "VW_CurrentLotteryInfo",
                WhereString = whereString.ToString(),
                Parameters = new
                {
                    LotteryID = param.LotteryID,
                    IssueNo = param.IssueNo,
                    StartDate = param.StartDate,
                    EndDate = param.EndDate,
                    IsLottery= param.IsLottery
                },
                RequestParam = new BasePagingRequestParam
                {
                    PageNo = param.PageNo,
                    PageSize = param.PageSize,
                    SortModels = new List<SortModel>
                    {
                        new SortModel { ColumnName = nameof(CurrentLotteryInfo.LotteryID)},
                        new SortModel { ColumnName = nameof(CurrentLotteryInfo.IssueNo),Sort=SortOrder.Descending},
                        new SortModel { ColumnName = nameof(CurrentLotteryInfo.CurrentLotteryTime),Sort=SortOrder.Descending}
                    }
                }
            });
            return DbHelper.PagedSqlQuery<CurrentLotteryInfo>(result);
        }
    }
}
