using IMPTDataBase.Common;
using IMPTDataBase.DBUtility;
using IMPTDataBase.Model;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace IMPTDataBase.DLL
{
    public class IMPTProfitLossInfo : OldProfitLossInfo
    {
        public static readonly bool databaseOnline = true;

        public static readonly string dbFullName = string.Empty;

        protected override string SqliteProfitLossInfoTableName => "IMPTProfitLossInfo";

        protected override int ExecuteNonQuery(string sql, SQLiteParameter[] sqlParams) => SQLiteDBHelper.ExecuteNonQuery(dbFullName, sql, sqlParams);

        protected override DataTable ExecuteDataTable(string sql, SQLiteParameter[] sqlParams) => SQLiteDBHelper.ExecuteDataTable(dbFullName, sql, null);

        static IMPTProfitLossInfo()
        {
            try
            {
                dbFullName = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\data.db";

                if (!File.Exists(dbFullName))
                {
                    SQLiteDBHelper.CreateDataBase(dbFullName);
                }

                if (!SQLiteDBHelper.TableIsExist(dbFullName, "IMPTProfitLossInfo"))
                {
                    SQLiteDBHelper.CreateIMProfitLossInfo(dbFullName);
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

                //動態擴增免費投注 Bonus Type欄位
                SQLiteDBHelper.AddColumnNX(dbFullName, "IMPTProfitLossInfo", "BonusType", "NVARCHAR(20)");

                databaseOnline = true;
            }
            catch (Exception ex)
            {
                LogsManager.InfoToTelegram("初始化 sqllite 数据库失败，详细信息：" + ex.Message + ",堆栈：" + ex.StackTrace);
                databaseOnline = false;
            }
        }

        public static void InIt()
        { }

        /// <summary>
        /// 单号是否存在
        /// </summary>
        /// <param name="trans_id"></param>
        /// <returns></returns>
        public static bool ExistsOrder(string trans_id)
        {
            bool isExist = true;
            try
            {
                string sql = @"
                    SELECT GameCode
                    FROM [IMPTProfitLossInfo]
                    WHERE GameCode=@GameCode
                    LIMIT 0,1";

                SQLiteParameter[] parameter = { new SQLiteParameter { ParameterName = "@GameCode" } };
                parameter[0].Value = trans_id;

                if (SQLiteDBHelper.ExecuteScalar(dbFullName, sql, parameter) == null)
                {
                    isExist = false;
                }
            }
            catch (Exception ex)
            {
                LogsManager.Info("查询本地单号时错误：错误信息:" + ex.Message + ",错误堆栈：" + ex.StackTrace);
            }
            return isExist;
        }

        public static Dictionary<string, int> GetUserIdWithUserName(List<string> usernames)
        {
            Dictionary<string, int> userMapping = new Dictionary<string, int>();

            List<SqlParameter> parameters = new List<SqlParameter>();

            StringBuilder usernameStr = new StringBuilder();
            for (int i = 0; i < usernames.Count; i++)
            {
                string nameField = "@UserName" + i;
                parameters.Add(new SqlParameter(nameField, usernames[i]));
                usernameStr.Append(nameField);
                if (i < (usernames.Count - 1))
                    usernameStr.Append(",");
            }

            string cmdTxt = @"
                SELECT UserName, UserID
                FROM UserInfo WITH (NOLOCK)
                WHERE UserName IN ({0})";

            cmdTxt = string.Format(cmdTxt, usernameStr.ToString());

            DataSet ds = DbHelperSQL.Query(cmdTxt, parameters.ToArray());

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var username = ds.Tables[0].Rows[i]["UserName"].ToString();
                    var userID = Convert.ToInt32(ds.Tables[0].Rows[i]["UserID"].ToString());

                    if (!userMapping.ContainsKey(username))
                    {
                        userMapping.Add(username, userID);
                    }
                }
            }

            return userMapping;
        }

        /// <summary>
        /// 保存在本地数据库
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// 保存在本地数据库
        /// </summary>
        /// <returns></returns>
        public static bool SaveDataToLocal(ISingleBetInfo bet, string memo)
        {
            try
            {
                #region sql

                string sql = @"
                    INSERT INTO [IMPTProfitLossInfo]
                    (
                        PlayerName, ProviderPlayerId, WindowCode, GameId, GameCode, GameType, GameName, SessionId,
                        Bet, Win, ProgressiveBet, ProgressiveWin, Balance, CurrentBet,
                        GameDate, LiveNetwork, ExitGame, BonusType, RNum,
                        Memo, localSavedTime, remoteSaved, remoteSavedTime, remoteSaveLastTryTime
                    )
                    VALUES
                    (
                        @PlayerName, @ProviderPlayerId, @WindowCode, @GameId, @GameCode, @GameType, @GameName, @SessionId,
                        @Bet, @Win, @ProgressiveBet, @ProgressiveWin, @Balance, @CurrentBet,
                        @GameDate, @LiveNetwork, @ExitGame, @BonusType, @RNum,
                        @Memo, @localSavedTime, @remoteSaved, @remoteSavedTime, @remoteSaveLastTryTime
                    )";

                #endregion sql

                #region par

                DateTime now = DateTime.Now;

                SQLiteParameter[] parameter = {
                    new SQLiteParameter { ParameterName = "@PlayerName", Value = bet.PlayerName },
                    new SQLiteParameter { ParameterName = "@ProviderPlayerId", Value = bet.ProviderPlayerId },
                    new SQLiteParameter { ParameterName = "@WindowCode", Value = bet.WindowCode },
                    new SQLiteParameter { ParameterName = "@GameId", Value = bet.GameId },
                    new SQLiteParameter { ParameterName = "@GameCode", Value = bet.GameCode },
                    new SQLiteParameter { ParameterName = "@GameType", Value = bet.GameType },
                    new SQLiteParameter { ParameterName = "@GameName", Value = bet.GameName },
                    new SQLiteParameter { ParameterName = "@SessionId", Value = bet.SessionId },

                    new SQLiteParameter { ParameterName = "@Bet", Value = bet.Bet },
                    new SQLiteParameter { ParameterName = "@Win", Value = bet.Win },
                    new SQLiteParameter { ParameterName = "@ProgressiveBet", Value = bet.ProgressiveBet },
                    new SQLiteParameter { ParameterName = "@ProgressiveWin", Value = bet.ProgressiveWin },
                    new SQLiteParameter { ParameterName = "@Balance", Value = bet.Balance },
                    new SQLiteParameter { ParameterName = "@CurrentBet", Value = bet.CurrentBet },

                    new SQLiteParameter { ParameterName = "@GameDate", Value = bet.GameDate },
                    new SQLiteParameter { ParameterName = "@LiveNetwork", Value = bet.LiveNetwork },
                    new SQLiteParameter { ParameterName = "@ExitGame", Value = bet.ExitGame },
                    new SQLiteParameter { ParameterName = "@BonusType", Value = bet.BonusType },
                    new SQLiteParameter { ParameterName = "@RNum", Value = bet.RNum },

                    new SQLiteParameter { ParameterName = "@Memo", Value = memo },
                    new SQLiteParameter { ParameterName = "@localSavedTime", Value = now },
                    new SQLiteParameter { ParameterName = "@remoteSaved", Value = 0 },
                    new SQLiteParameter { ParameterName = "@remoteSavedTime", Value = now },
                    new SQLiteParameter { ParameterName = "@remoteSaveLastTryTime", Value = now}
                };

                #endregion par

                SQLiteDBHelper.ExecuteNonQuery(dbFullName, sql, parameter);
                return true;
            }
            catch (Exception ex)
            {
                LogsManager.InfoToTelegram("保存PT电游亏赢数据到本地 SqlLite 数据库失败，详细信息：" + ex.Message + ",堆栈：" + ex.StackTrace);
                return false;
            }
        }

        ///// <summary>
        ///// 保存数据至远端
        ///// </summary>
        //public static void SaveDataToRemote(IMPTApiParamModel model)
        //{
        //    string selectSql = @"
        //        SELECT *
        //        FROM [IMPTProfitLossInfo]
        //        WHERE remoteSaved = 0 AND (remoteSaveTryCount >= 0 AND remoteSaveTryCount < 10)
        //        ORDER BY Id LIMIT 0,100";

        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        dt = SQLiteDBHelper.ExecuteDataTable(dbFullName, selectSql, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogsManager.InfoToTelegram("查询本地PT电游数据时失败，详细信息：" + ex.Message + ".堆栈:" + ex.StackTrace);
        //        return;
        //    }

        //    try
        //    {
        //        List<int> notifiedUsers = new List<int>();

        //        if (dt.Rows.Count > 0)
        //        {
        //            Dictionary<string, IMBalanceInfo> userAvailableScores = new Dictionary<string, IMBalanceInfo>();

        //            string jxPlayerHeader = model.Linecode + "_";

        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                string account = dr["PlayerName"].ToString();

        //                //0.Account
        //                string userId = dr["PlayerName"].ToString().Replace(jxPlayerHeader, "");
        //                //1.下注时间
        //                DateTime betTime = DateTime.Now;
        //                //2.开奖时间
        //                DateTime profitLossTime = DateTime.Now;
        //                //3.亏赢类型
        //                string profitLossType = "亏盈";
        //                //4.有效下注额
        //                object profitLossMoney = dr["Bet"];
        //                //5.亏赢
        //                object winMoney = decimal.Parse(dr["Win"].ToString()) - decimal.Parse(dr["Bet"].ToString());
        //                //6.有效下注额 + 亏赢
        //                object prizeMoney = decimal.Parse(dr["Win"].ToString());
        //                //7.备注
        //                string memo = dr["Memo"].ToString();
        //                //8.订单编号
        //                string palyId = dr["GameCode"].ToString();
        //                //9.游戏类型
        //                string gameType = dr["GameName"].ToString();

        //                if (!userAvailableScores.Keys.Contains(userId))
        //                {
        //                    model.PlayerId = account;
        //                    IMBalanceInfo balanceData = Transfer.GetBalance(model);

        //                    if (balanceData != null && balanceData.Code == (int)APIErrorCode.Success)
        //                    {
        //                        userAvailableScores.Add(userId, balanceData);
        //                    }
        //                }

        //                string startDate = dr["GameDate"].ToString();

        //                if (!string.IsNullOrEmpty(dr["GameDate"].ToString()))
        //                {
        //                    betTime = Convert.ToDateTime(startDate);
        //                    profitLossTime = Convert.ToDateTime(startDate);
        //                }

        //                //10.剩余金额
        //                decimal availableScores = 0M;

        //                if (userAvailableScores.Keys.Contains(userId.ToString()))
        //                {
        //                    availableScores = decimal.Parse(userAvailableScores[userId.ToString()].Balance);
        //                    //freezeScores = availableScores - 0;
        //                }
        //                else
        //                {
        //                    var transfer = new Transfer(_environmentUser, DbConnectionTypes.Master);
        //                    decimal freezeScores = 0M;

        //                    //避免把資料更新為0, 重新取得本地餘額
        //                    transfer.SetLocalUserScores(Convert.ToInt32(userId), ref availableScores, ref freezeScores);
        //                }

        //                //20201110暫凍金額沒有用到，也不需要更新回去，故拿掉
        //                SqlParameter[] parametersForInsert = {
        //                    new SqlParameter("@UserID",SqlDbType.Int,50),                   //0.Account
        //                    new SqlParameter("@BetTime", SqlDbType.DateTime),               //1.下注时间
        //                    new SqlParameter("@ProfitLossTime", SqlDbType.DateTime),        //2.开奖时间
        //                    new SqlParameter("@ProfitLossType", SqlDbType.NVarChar, 50),    //3.亏赢类型
        //                    new SqlParameter("@ProfitLossMoney", SqlDbType.Decimal, 18),    //4.有效下注额
        //                    new SqlParameter("@WinMoney", SqlDbType.Decimal, 18),           //5.亏赢
        //                    new SqlParameter("@PrizeMoney", SqlDbType.Decimal, 18),         //6.有效下注额 + 亏赢
        //                    new SqlParameter("@Memo", SqlDbType.NVarChar, 500),             //7.备注
        //                    new SqlParameter("@PalyID", SqlDbType.NVarChar, 50),            //8.订单编号
        //                    new SqlParameter("@GameType", SqlDbType.NVarChar, 50),          //9.游戏类型
        //                    new SqlParameter("@AvailableScores", SqlDbType.Decimal, 18),    //10.剩余金额
        //                    //new SqlParameter("@FreezeScores", SqlDbType.Decimal, 18),       //11.可用金额
        //                    //new SqlParameter("@AllBetMoney", SqlDbType.Decimal, 18)         //12.總投注額
        //                };

        //                parametersForInsert[0].Value = userId;
        //                parametersForInsert[1].Value = betTime;
        //                parametersForInsert[2].Value = profitLossTime;
        //                parametersForInsert[3].Value = profitLossType;
        //                parametersForInsert[4].Value = profitLossMoney;
        //                parametersForInsert[5].Value = winMoney;
        //                parametersForInsert[6].Value = prizeMoney;
        //                parametersForInsert[7].Value = memo;
        //                parametersForInsert[8].Value = palyId;
        //                parametersForInsert[9].Value = gameType;
        //                parametersForInsert[10].Value = availableScores;
        //                //parametersForInsert[11].Value = freezeScores;
        //                //parametersForInsert[12].Value = dr["StakeAmount"];

        //                LogsManager.Info("获取到用户 " + account + " 的亏赢单据 " + palyId);

        //                string result = string.Empty;
        //                try
        //                {
        //                    DataSet ds = DbHelperSQL.RunProcedure("Pro_AddIMPTProfitLoss", parametersForInsert, "tab");
        //                    result = ds.Tables[0].Rows[0][0].ToString();
        //                }
        //                catch (Exception ex)
        //                {
        //                    result = ex.Message;
        //                }

        //                //INSERT  IMPTPlayInfo
        //                try
        //                {
        //                    SqlParameter[] Paras = {
        //                        new SqlParameter("@UserID",SqlDbType.NVarChar,50),
        //                        new SqlParameter("@BetTime",SqlDbType.DateTime),
        //                        new SqlParameter("@ProfitLossTime",SqlDbType.DateTime),
        //                        new SqlParameter("@ProfitLossMoney",SqlDbType.Decimal,18),
        //                        new SqlParameter("@WinMoney",SqlDbType.Decimal,18),
        //                        new SqlParameter("@Memo",SqlDbType.NVarChar,500),
        //                        new SqlParameter("@PalyID",SqlDbType.NVarChar,50),
        //                        new SqlParameter("@GameType",SqlDbType.NVarChar,50)
        //                    };

        //                    Paras[0].Value = userId;
        //                    Paras[1].Value = betTime;
        //                    Paras[2].Value = profitLossTime;
        //                    Paras[3].Value = profitLossMoney;
        //                    Paras[4].Value = winMoney;
        //                    Paras[5].Value = memo;
        //                    Paras[6].Value = palyId;
        //                    Paras[7].Value = gameType;
        //                    DataSet ds = DbHelperSQL.RunProcedure("Pro_AddIMPTPlayInfo", Paras, "tab");
        //                }
        //                catch (Exception ex)
        //                {
        //                    LogsManager.Info("保存IMPT下注数据 " + palyId + " 到数据库失败，详细信息：" + ex.Message);
        //                }

        //                SQLiteParameter[] parameterForUpdate = {
        //                    new SQLiteParameter { ParameterName = "@Id" }
        //                };

        //                parameterForUpdate[0].Value = dr["Id"].ToString();

        //                if (!string.IsNullOrEmpty(result))
        //                {
        //                    LogsManager.Info("保存PT电游亏赢数据 " + palyId + " 到数据库失败，详细信息：" + result);

        //                    try
        //                    {
        //                        string updateSqlForFailure = @"
        //                            UPDATE [IMPTProfitLossInfo]
        //                            SET remoteSaveTryCount = remoteSaveTryCount + 1, remoteSaveLastTryTime = datetime('now', 'localtime')
        //                            WHERE Id = @Id";

        //                        SQLiteDBHelper.ExecuteNonQuery(dbFullName, updateSqlForFailure, parameterForUpdate);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        LogsManager.InfoToTelegram("标识 SqlLite PT电游亏赢数据 " + palyId + " 状态为“远程保存失败”时失败，详细信息：" + ex.Message);
        //                    }
        //                }
        //                else
        //                {
        //                    LogsManager.Info("保存PT电游亏赢数据 " + palyId + " 到数据库成功");

        //                    if (!notifiedUsers.Contains(int.Parse(userId)))
        //                    {
        //                        notifiedUsers.Add(int.Parse(userId));
        //                    }

        //                    try
        //                    {
        //                        string updateSqlForSuccess = @"
        //                            UPDATE [IMPTProfitLossInfo]
        //                            SET remoteSaved = 1, remoteSavedTime = datetime('now', 'localtime')
        //                            WHERE Id = @Id";

        //                        SQLiteDBHelper.ExecuteNonQuery(dbFullName, updateSqlForSuccess, parameterForUpdate);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        LogsManager.InfoToTelegram("标识 SqlLite PT电游亏赢数据 " + palyId + " 状态为“远程保存成功”时失败，详细信息：" + ex.Message);
        //                    }
        //                }
        //            }
        //        }

        //        var messageQueueService = DependencyUtil.ResolveServiceForModel<IMessageQueueService>(JxApplication.IMPTTransferService);

        //        foreach (var userId in notifiedUsers)
        //        {
        //            try
        //            {
        //                messageQueueService.SendRefreshUserInfoMessage(userId);
        //            }
        //            catch (Exception ex)
        //            {
        //                LogsManager.Info("发送刷新用户余额消息时失败，详细信息:" + ex.Message + ex.StackTrace);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogsManager.InfoToTelegram("保存数据到远端时错误:" + ex.Message + ex.StackTrace);
        //    }
        //}

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

            try
            {
                SQLiteDBHelper.ExecuteNonQuery(dbFullName, Sql, parameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询 last_version_key
        /// </summary>
        /// <param name="trans_id"></param>
        /// <returns></returns>
        public static string SelectLastSearchTime()
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
                LogsManager.Info("查询 LastSearchTime 时错误：错误信息:" + ex.Message + ",错误堆栈：" + ex.StackTrace);
            }

            return result;
        }
    }
}