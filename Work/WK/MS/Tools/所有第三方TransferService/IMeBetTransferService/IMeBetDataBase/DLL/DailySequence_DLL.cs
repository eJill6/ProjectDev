using IMeBetDataBase.Common;
using IMeBetDataBase.DBUtility;
using System;
using System.Data.SQLite;

namespace IMeBetDataBase.DLL
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
                LogsManager.Error(ex);
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
            DateTime currentTime = DateTime.Now;
            object result = currentTime.Hour.ToString().PadLeft(2, '0') + currentTime.Minute.ToString().PadLeft(2, '0') + "x";
            try
            {
                string sqlUpdate = @"UPDATE DailySequence SET SeqNumber = SeqNumber + 1 WHERE DailyDate = @DailyDate;";
                SQLiteDBHelper.ExecuteNonQuery(dbPath, sqlUpdate, parameters);

                string sql = @"SELECT SeqNumber FROM DailySequence WHERE DailyDate = @DailyDate";
                result = SQLiteDBHelper.ExecuteScalar(dbPath, sql, parameters);
            }
            catch (Exception ex)
            {
                LogsManager.Error(ex);
                throw;
            }
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
