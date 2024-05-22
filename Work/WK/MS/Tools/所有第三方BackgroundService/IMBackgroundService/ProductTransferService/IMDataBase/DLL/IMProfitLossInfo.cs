using IMDataBase.DBUtility;
using IMDataBase.Model;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using System.Data;
using System.Data.SQLite;

namespace IMDataBase.DLL
{
    public class IMProfitLossInfo : OldProfitLossInfo, IIMOldSaveProfitLossInfo
    {
        public static readonly int GameNameMaxLength = 197;

        public static readonly bool databaseOnline = true;

        public static readonly string dbFullName = string.Empty;

        protected override object GetSqliteLock() => SQLiteDBHelper.WriteLockobj;

        protected override IDbConnection GetSqliteConnection() => SQLiteDBHelper.GetSQLiteConnection(dbFullName);

        static IMProfitLossInfo()
        {
            try
            {
                dbFullName = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar + "data.db";

                if (!File.Exists(dbFullName))
                {
                    SQLiteDBHelper.CreateDataBase(dbFullName);
                }

                if (!SQLiteDBHelper.TableIsExist(dbFullName, "LastTimeTable"))
                {
                    SQLiteDBHelper.CreateSearchLastTimeTable(dbFullName);
                }

                if (!SQLiteDBHelper.TableIsExist(dbFullName, "DailySequence"))
                {
                    DailySequence_DLL dailySequence_DLL = new DailySequence_DLL(dbFullName);
                    dailySequence_DLL.CreateTableDailySequence();
                    dailySequence_DLL.InitializeADailySequence(DateTime.Now.ToString("yyyyMMdd"));
                }

                databaseOnline = true;
            }
            catch (Exception ex)
            {
                DependencyUtil.ResolveService<ILogUtilService>().Value.Error("初始化 sqllite 数据库失败，详细信息：" + ex.Message + ",堆栈：" + ex.StackTrace);
                databaseOnline = false;
            }
        }

        public static void InIt()
        { }

        /// <summary>
        /// 保存在本地数据库
        /// </summary>
        /// <returns></returns>
        public void SaveDataToTarget(List<SingleBetInfoViewModel> betLogs)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 保存最后查询时间
        /// </summary>
        public static void UpdateSearchTimestamp(string searchTimestamp)
        {
            string Sql = @"UPDATE [LastTimeTable] SET LastSearchTime = @LastSearchTime";

            SQLiteParameter[] parameter = {
                new SQLiteParameter { ParameterName = "@LastSearchTime" }
            };

            parameter[0].Value = searchTimestamp;

            SQLiteDBHelper.ExecuteNonQuery(dbFullName, Sql, parameter);
        }

        /// <summary>
        /// 查询 last_version_key
        /// </summary>
        /// <param name="trans_id"></param>
        /// <returns></returns>
        public string SelectLastSearchTime()
        {
            string result = string.Empty;

            try
            {
                string sql = @"
                    SELECT LastSearchTime
                    FROM [LastTimeTable]
                    LIMIT 0,1";

                DataTable dt = SQLiteDBHelper.ExecuteDataTable(dbFullName, sql, null);
                result = dt.Rows[0]["LastSearchTime"].ToString();
            }
            catch (Exception ex)
            {
                LogUtilService.Error("查询 LastSearchTime 时错误：错误信息:" + ex.Message + ",错误堆栈：" + ex.StackTrace);
            }

            return result;
        }
    }
}