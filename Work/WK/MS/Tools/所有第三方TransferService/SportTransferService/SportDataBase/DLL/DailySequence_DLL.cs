using Maticsoft.DBUtility;
using SportDataBase.Common;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace SportDataBase.DLL
{
    public class DailySequence_DLL
    {
        private readonly string dbPath = string.Empty;

        public DailySequence_DLL(string dbPath)
        {
            this.dbPath = dbPath;
        }

        /// <summary>
        /// 创建DailySequence表
        /// </summary>
        /// <param name="dbPath">要创建的SQLite数据库文件路径</param>
        public void CreateTableDailySequence()
        {
            string sql = @"CREATE TABLE DailySequence (
    DailyDate TEXT (8) PRIMARY KEY,
    SeqNumber INTEGER  NOT NULL DEFAULT (0) 
);";
            try
            {
                SQLiteDBHelper.ExecuteNonQuery(dbPath, sql, null);
            }
            catch (Exception ex)
            {
                LogsManager.Error(ex);
                throw;
            }
        }

        public void InitializeADailySequence(string dailyDate)
        {
            string sql = @"INSERT INTO DailySequence(DailyDate)
VALUES(@DailyDate)";

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
                LogsManager.Error(ex);
                throw;
            }
        }

        public string UpdateAndGetSequenceNumber(string dailyDate)
        {
            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@DailyDate",dailyDate),
            };

            string sql = @"UPDATE DailySequence SET SeqNumber = SeqNumber + 1 WHERE DailyDate = @DailyDate;
SELECT SeqNumber FROM DailySequence WHERE DailyDate = @DailyDate";

            var result = SQLiteDBHelper.ExecuteScalar(dbPath, sql, parameters);

            return Convert.ToString(result);
        }

        public bool IsExistDailySequenceRecord(string dailyDate)
        {
            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@DailyDate",dailyDate),
            };

            string sql = @"SELECT DailyDate FROM DailySequence WHERE DailyDate = @DailyDate;";

            var result = SQLiteDBHelper.ExecuteScalar(dbPath, sql, parameters);

            return false == string.IsNullOrEmpty(Convert.ToString(result));
        }
    }
}
