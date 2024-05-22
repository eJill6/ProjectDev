using JxBackendService.Service.ThirdPartyTransfer.Old;
using ProductTransferService.LCDataBase.DBUtility;
using ProductTransferService.LCDataBase.Model;
using System.Data;
using System.Data.SQLite;

namespace ProductTransferService.LCDataBase.DLL
{
    public class LCProfitLossInfo : OldProfitLossInfo, ILCOldSaveProfitLossInfo
    {
        public static bool DatabaseOnline { get; private set; }

        private static string s_dbFullName = string.Empty;

        protected override object GetSqliteLock() => SQLiteDBHelper.WriteLockobj;

        protected override IDbConnection GetSqliteConnection() => SQLiteDBHelper.GetSQLiteConnection(s_dbFullName);

        public void Init()
        {
            try
            {
                s_dbFullName = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar + "data.db";

                if (!File.Exists(s_dbFullName))
                {
                    SQLiteDBHelper.CreateDataBase(s_dbFullName);
                }

                if (!SQLiteDBHelper.TableIsExist(s_dbFullName, "LastTimeTable"))
                {
                    SQLiteDBHelper.CreateSearchLastTimeTable(s_dbFullName);
                }

                if (!SQLiteDBHelper.TableIsExist(s_dbFullName, "DailySequence"))
                {
                    DailySequence_DLL dailySequence_DLL = new DailySequence_DLL(s_dbFullName);
                    dailySequence_DLL.CreateTableDailySequence();
                    dailySequence_DLL.InitializeADailySequence(DateTime.Now.ToString("yyyyMMdd"));
                }

                DatabaseOnline = true;
            }
            catch (Exception ex)
            {
                LogUtilService.Error("初始化 sqllite 数据库失败，详细信息：" + ex.Message + ",堆栈：" + ex.StackTrace);
                DatabaseOnline = false;
            }
        }

        /// <summary>
        /// 保存在本地数据库
        /// </summary>
        /// <returns></returns>
        public void SaveDataToTarget(List<SingleBetInfoViewModel> singleBetInfoViewModels)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 保存最后查询时间
        /// </summary>
        public static void UpdateSearchTimestamp(string searchTimestamp)
        {
            string sql = @"UPDATE [LastTimeTable] SET LastSearchTime = @LastSearchTime";

            SQLiteParameter[] parameter =
            {
                new SQLiteParameter { ParameterName = "@LastSearchTime" }
            };

            parameter[0].Value = searchTimestamp;

            SQLiteDBHelper.ExecuteNonQuery(s_dbFullName, sql, parameter);
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

                DataTable dt = SQLiteDBHelper.ExecuteDataTable(s_dbFullName, sql, null);
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