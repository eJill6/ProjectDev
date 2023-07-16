using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.Game.Lottery;
using JxBackendService.Model.Entity.Game.Lottery;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Repository.Game.Lottery
{
    public class PalyInfoRep : BaseDbRepository<PalyInfo>, IPalyInfoRep
    {
        public PalyInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
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