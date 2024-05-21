using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.Game.Lottery;
using JxBackendService.Model.Entity.BackSideUser;
using JxBackendService.Model.Entity.Game.Lottery;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.Game.Lottery
{
    public class PalyInfoRep : BaseDbRepository<PalyInfo>, IPalyInfoRep
    {
        public PalyInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }
        public PagedResultModel<Model.BackSideWeb.PalyInfo> GetPalyInfoReport(PalyInfoParam param)
        {
            StringBuilder whereString = new StringBuilder(" WHERE 1 = 1 AND IsFactionAward IN (0,1,2,6,7,8,9) ");
            if (param.LotteryID.HasValue)
            {
                whereString.AppendLine($" AND LotteryID = @LotteryID");
            }
            if (!param.PalyCurrentNum.IsNullOrEmpty())
            {
                whereString.AppendLine($" AND PalyCurrentNum = @PalyCurrentNum");
            }
            if (param.UserId.HasValue)
            {
                whereString.AppendLine($" AND UserId = @UserId");
            }
            if (param.IsFactionAward.HasValue)
            {
                whereString.AppendLine($" AND IsFactionAward = @IsFactionAward");
            }
            if (param.IsWin.HasValue)
            {
                whereString.AppendLine($" AND IsWin = @IsWin");
            }
            if (!param.RoomId.IsNullOrEmpty())
            {
                whereString.AppendLine($" AND RoomId = @RoomId");
            }
            whereString.AppendLine($" AND NoteTime >= @StartDate AND NoteTime <= @EndDate ");

            PagedSqlQueryParamsModel result = CreateAllColumnsPagedSqlQueryParams(new BuildPagedSqlQueryParam
            {
                InlodbType = InlodbType.InlodbBak,
                TableName= "VW_PalyInfo",
                WhereString = whereString.ToString(),                
                Parameters = new
                {
                    LotteryID= param.LotteryID,
                    PalyCurrentNum= param.PalyCurrentNum,
                    UserId= param.UserId,
                    IsFactionAward= param.IsFactionAward,
                    IsWin= param.IsWin,
                    StartDate= param.StartDate,
                    EndDate= param.EndDate,
                    RoomId=param.RoomId,
                },
                RequestParam=new BasePagingRequestParam
                {
                    PageNo= param.PageNo,
                    PageSize= param.PageSize,
                    SortModels=new List<SortModel>
                    {
                        new SortModel { ColumnName = nameof(Model.BackSideWeb.PalyInfo.NoteTime),Sort=SortOrder.Descending}
                    }
                }
            });
            return DbHelper.PagedSqlQuery<Model.BackSideWeb.PalyInfo>(result);
        }
        public Model.BackSideWeb.PalyInfo GetPalyInfoDetail(string PalyID)
        {
            List<string> selectColumns = ModelUtil.GetAllColumnInfos<Model.BackSideWeb.PalyInfo>(). Select(s => s.ColumnName).ToList();
            string allSelectColumns = string.Join(",\n", selectColumns);
            string sql = $@"
             SELECT {allSelectColumns} 
             FROM {InlodbType.InlodbBak}.[dbo].[VW_PalyInfo] 
             WHERE PalyID=@PalyID ";
            return DbHelper.QuerySingle<Model.BackSideWeb.PalyInfo>(sql,new { PalyID = PalyID });
        }
        public List<PalyInfo> GetPalyInfos(List<int> palyIDs)
        {
            List<string> selectColumns = ModelUtil.GetAllColumnInfos<PalyInfo>().
                Select(s => s.ColumnName).
                ToList();

            string filter = "WHERE PalyID IN @palyIDs ";
            string allSelectColumns = string.Join(",\n", selectColumns);
            string createTempSelectColumns = string.Join(",\n", selectColumns.Where(w => w != nameof(PalyInfo.PalyID)));

            string sql = $@"
                SELECT
                    1 AS PalyID,
                    {createTempSelectColumns}
                INTO #PALYINFO
                FROM {InlodbType.Inlodb}.dbo.{ModelUtil.GetTableName(typeof(PalyInfo))}
                WHERE 1 = 2

                INSERT INTO #PalyInfo(
                   {allSelectColumns})
                {GetAllQuerySQL(InlodbType.Inlodb, selectColumns)}
                {filter}

                IF (SELECT COUNT(*) FROM #PalyInfo) < @palyIDCount
                BEGIN
                    INSERT INTO #PalyInfo(
                       {allSelectColumns})
                    {GetAllQuerySQL(InlodbType.InlodbBak, selectColumns)}
                    {filter}
                END

                SELECT DISTINCT
                   {allSelectColumns}
                FROM #PalyInfo ";

            return DbHelper.QueryList<PalyInfo>(sql, new { palyIDs, palyIDCount = palyIDs.Count });
        }
    }
}