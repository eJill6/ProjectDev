using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;
using ProductTransferService.LCDataBase.DBUtility;
using System.Data.SQLite;

namespace ProductTransferService.LCDataBase.DLL
{
    public class DailySequence_DLL
    {
        private readonly string dbPath = string.Empty;

        private readonly Lazy<ILogUtilService> _logUtilService;

        public DailySequence_DLL(string dbPath)
        {
            this.dbPath = dbPath;
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
        }

        /// <summary>
        /// 创建DailySequence表
        /// </summary>
        /// <param name="dbPath">要创建的SQLite数据库文件路径</param>
        public void CreateTableDailySequence()
        {
            string sql = @"
                CREATE TABLE DailySequence (
                    DailyDate TEXT (8) PRIMARY KEY,
                    SeqNumber INTEGER  NOT NULL DEFAULT (0)
                );";
            try
            {
                SQLiteDBHelper.ExecuteNonQuery(dbPath, sql, null);
            }
            catch (Exception ex)
            {
                _logUtilService.Value.Error(ex);

                throw;
            }
        }

        public void InitializeADailySequence(string dailyDate)
        {
            string sql = @"INSERT INTO DailySequence(DailyDate) VALUES (@DailyDate)";

            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@DailyDate",dailyDate),
            };

            try
            {
                SQLiteDBHelper.ExecuteNonQuery(dbPath, sql, parameters);
            }
            catch (Exception ex)
            {
                _logUtilService.Value.Error(ex);

                throw;
            }
        }
    }
}