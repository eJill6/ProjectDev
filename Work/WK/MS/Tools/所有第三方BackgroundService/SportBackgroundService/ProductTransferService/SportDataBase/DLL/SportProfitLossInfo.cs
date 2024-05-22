using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using Maticsoft.DBUtility;
using ProductTransferService.SportDataBase.Model;
using System.Data;
using System.Data.SQLite;

namespace ProductTransferService.SportDataBase.DLL
{
    public class SportProfitLossInfo : OldProfitLossInfo, ISportOldSaveProfitLossInfo
    {
        public static readonly bool dataBaseOnline = true;

        public static readonly string dbFullName = string.Empty;

        protected override object GetSqliteLock() => SQLiteDBHelper.WriteLockobj;

        protected override IDbConnection GetSqliteConnection() => SQLiteDBHelper.GetSQLiteConnection(dbFullName);

        static SportProfitLossInfo()
        {
            try
            {
                dbFullName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "data.db");

                if (!File.Exists(dbFullName))
                {
                    SQLiteDBHelper.CreateDataBase(dbFullName);
                }

                if (!SQLiteDBHelper.TableIsExist(dbFullName, "VersionKeyTD"))
                {
                    SQLiteDBHelper.CreateVersionKeyTD(dbFullName);
                }

                if (!SQLiteDBHelper.TableIsExist(dbFullName, "DailySequence"))
                {
                    DailySequence_DLL dailySequence_DLL = new DailySequence_DLL(dbFullName);
                    dailySequence_DLL.CreateTableDailySequence();
                    dailySequence_DLL.InitializeADailySequence(DateTime.Now.ToString("yyyyMMdd"));
                }

                dataBaseOnline = true;
            }
            catch (Exception ex)
            {
                DependencyUtil.ResolveService<ILogUtilService>().Value.Error("初始化sqllite数据库失败，详细信息：" + ex.Message + ",堆栈：" + ex.StackTrace);

                dataBaseOnline = false;
            }
        }

        public static void InIt()
        { }

        /// <summary>
        /// 保存在本地数据库
        /// </summary>

        public void SaveDataToTarget(List<SabaBetDetailViewModel> sabaBetDetailViewModels)
        {
            throw new NotSupportedException();
        }

        public void UpdateSqlLiteProfitLossInfo(SqlliteUpdateType type, string id, string transId)
        {
            string updateSqlForSuccess = @"
                            UPDATE [SportProfitLossInfo] SET remoteSaved = 1,remoteSavedTime=datetime('now', 'localtime') WHERE Id = @Id
                           ";

            string updateSqlForFailure = @"
                            UPDATE [SportProfitLossInfo] SET remoteSaveTryCount = remoteSaveTryCount+1,remoteSaveLastTryTime=datetime('now', 'localtime') WHERE Id = @Id
                           ";

            string updateSqlForD3D9Failure = @"
                            UPDATE [SportProfitLossInfo] SET remoteSaveTryCount = remoteSaveTryCount+10,remoteSaveLastTryTime=datetime('now', 'localtime') WHERE Id = @Id
                           ";

            string sql = "";
            string memo = "";

            switch (type)
            {
                case SqlliteUpdateType.UpdateSuccess:
                    sql = updateSqlForSuccess;
                    memo = "远程保存成功";
                    break;

                case SqlliteUpdateType.UpdateFailure:
                    sql = updateSqlForFailure;
                    memo = "远程保存失败";
                    break;

                case SqlliteUpdateType.UpdateD3Failure:
                    sql = updateSqlForD3D9Failure;
                    memo = "远程保存失败";
                    break;
            }

            UpdateSqlLiteProfitLossInfo(sql, id, transId, memo);
        }

        private void UpdateSqlLiteProfitLossInfo(string updateSql, string id, string transId, string memo)
        {
            try
            {
                SQLiteParameter[] parameterForUpdate = new SQLiteParameter[]{
                                               new SQLiteParameter { ParameterName = "@Id" }
                                           };
                parameterForUpdate[0].Value = id;

                SQLiteDBHelper.ExecuteNonQuery(dbFullName, updateSql, parameterForUpdate);
            }
            catch (Exception ex)
            {
                LogUtilService.Error("标识SqlLite体育亏赢数据 " + transId + $" 状态为“{memo}”时失败，详细信息：" + ex.Message);
            }
        }

        /// <summary>
        /// 保存last_version_key
        /// </summary>
        public void UpdateVersion_key(string version_key)
        {
            string Sql = @"UPDATE [VersionKeyTD] SET version_key = @version_key";
            SQLiteParameter[] parameter = {
                                               new SQLiteParameter { ParameterName = "@version_key" }
                                           };

            parameter[0].Value = version_key;
            try
            {
                SQLiteDBHelper.ExecuteNonQuery(dbFullName, Sql, parameter);
            }
            catch (Exception ex)
            {
                LogUtilService.Error("更新last_version_key时失败，Key：" + version_key + "详细信息：" + ex.Message + ",堆栈：" + ex.Message);
            }
        }

        /// <summary>
        /// 查询last_version_key
        /// </summary>
        public string SelectVersion_key()
        {
            try
            {
                string sql = @"
                                    SELECT
                                        version_key
                                    FROM [VersionKeyTD]
                                    LIMIT 0,1
                                   ";

                DataTable dt = SQLiteDBHelper.ExecuteDataTable(dbFullName, sql, null);
                return dt.Rows[0]["version_key"].ToString();
            }
            catch (Exception ex)
            {
                LogUtilService.Error("查询version_key时错误：错误信息:" + ex.Message + ",错误堆栈：" + ex.StackTrace);

                return "";
            }
        }
    }
}