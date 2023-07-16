using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Service.ThirdPartyTransfer.Old
{
    public interface IOldProfitLossInfo
    {
        DataTable GetBatchDataFromLocalDB();

        DataTable GetBatchDataFromLocalDB(string tableName);

        bool UpdateSQLiteToSavedStatus(string keyId, SaveBetLogFlags saveBetLogFlag);

        bool UpdateSQLiteToSavedStatus(string tableName, string keyId, SaveBetLogFlags saveBetLogFlag);

        void ClearExpiredData();
    }

    public abstract class OldProfitLossInfo : IOldProfitLossInfo
    {
        protected ILogUtilService LogUtilService { get; private set; }

        protected OldProfitLossInfo()
        {
            LogUtilService = DependencyUtil.ResolveService<ILogUtilService>();
        }

        protected abstract string SqliteProfitLossInfoTableName { get; }

        protected abstract int ExecuteNonQuery(string sql, SQLiteParameter[] sqlParams);

        protected abstract DataTable ExecuteDataTable(string sql, SQLiteParameter[] sqlParams);

        public bool UpdateSQLiteToSavedStatus(string keyId, SaveBetLogFlags saveBetLogFlag)
        {
            return UpdateSQLiteToSavedStatus(SqliteProfitLossInfoTableName, keyId, saveBetLogFlag);
        }

        public bool UpdateSQLiteToSavedStatus(string tableName, string keyId, SaveBetLogFlags saveBetLogFlag)
        {
            string setColumnValueSql = null;

            switch (saveBetLogFlag)
            {
                case SaveBetLogFlags.Success:
                    setColumnValueSql = @"
                    SET
                        remoteSaved = 1,
                        remoteSavedTime = datetime('now', 'localtime') ";
                    break;

                case SaveBetLogFlags.Fail:
                    setColumnValueSql = @"
                    SET
                        remoteSaveTryCount = remoteSaveTryCount + 1,
                        remoteSaveLastTryTime = datetime('now', 'localtime') ";
                    break;

                case SaveBetLogFlags.Ignore:
                    setColumnValueSql = @"
                    SET
                        remoteSaveTryCount = 999,
                        remoteSaveLastTryTime = datetime('now', 'localtime') ";
                    break;
            }

            string sql = $@"
                UPDATE [{tableName}]
                {setColumnValueSql}
                WHERE Id = @Id";

            SQLiteParameter[] parameterForUpdate = { new SQLiteParameter { ParameterName = "@Id", Value = keyId } };

            return ExecuteNonQuery(sql, parameterForUpdate) > 0;
        }

        public DataTable GetBatchDataFromLocalDB() => GetBatchDataFromLocalDB(SqliteProfitLossInfoTableName);

        public DataTable GetBatchDataFromLocalDB(string tableName)
        {
            string selectSql = $@"
                SELECT *
                FROM [{tableName}]
                WHERE remoteSaved = 0 AND (remoteSaveTryCount >= 0 AND remoteSaveTryCount < 10)
                ORDER BY Id
                LIMIT 0,100 ";

            return ExecuteDataTable(selectSql, null);
        }

        public virtual void ClearExpiredData()
        {
            DoClearExpiredData(SqliteProfitLossInfoTableName);
        }

        public void DoClearExpiredData(string tableName)
        {
            string deleteSql = $@"
                DELETE FROM [{tableName}]
                WHERE Id IN (
                    SELECT Id
                    FROM [{tableName}]
                    WHERE
                        localSavedTime < @localSavedTime
                        AND (RemoteSaved = 1 OR RemoteSaveTryCount >= 10)
                    ORDER BY Id LIMIT 300)";

            var localSavedTime = DateTime.Now.AddMonths(-1);

            SQLiteParameter[] parameters = { new SQLiteParameter
            {
                ParameterName = "@localSavedTime",
                Value = localSavedTime
            }};

            while (true)
            {
                int rowCount = ExecuteNonQuery(deleteSql, parameters);
                LogUtilService.ForcedDebug($"从 SqlLite 数据库中删除{tableName} {rowCount} 条过期亏赢数据");

                if (rowCount == 0)
                {
                    break;
                }

                Task.Delay(2000).Wait();
            }
        }
    }
}