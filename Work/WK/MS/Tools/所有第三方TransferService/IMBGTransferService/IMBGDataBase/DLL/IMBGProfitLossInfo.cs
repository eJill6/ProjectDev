using IMBGDataBase.BLL;
using IMBGDataBase.Common;
using IMBGDataBase.DBUtility;
using IMBGDataBase.Enums;
using IMBGDataBase.Model;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace IMBGDataBase.DLL
{
    public class IMBGProfitLossInfo : OldProfitLossInfo
    {
        public static readonly bool databaseOnline = true;

        public static readonly string dbFullName = string.Empty;

        protected override string SqliteProfitLossInfoTableName => "IMBGProfitLossInfo";

        protected override int ExecuteNonQuery(string sql, SQLiteParameter[] sqlParams) => SQLiteDBHelper.ExecuteNonQuery(dbFullName, sql, sqlParams);

        protected override DataTable ExecuteDataTable(string sql, SQLiteParameter[] sqlParams) => SQLiteDBHelper.ExecuteDataTable(dbFullName, sql, null);

        static IMBGProfitLossInfo()
        {
            try
            {
                dbFullName = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\data.db";

                if (!File.Exists(dbFullName))
                {
                    SQLiteDBHelper.CreateDataBase(dbFullName);
                }

                if (!SQLiteDBHelper.TableIsExist(dbFullName, "IMBGProfitLossInfo"))
                {
                    SQLiteDBHelper.CreateIMBGProfitLossInfo(dbFullName);
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
        public static bool ExistsOrder(long trans_id)
        {
            bool isExist = true;
            try
            {
                string sql = @"
                    SELECT Id
                    FROM [IMBGProfitLossInfo]
                    WHERE Id=@Id
                    LIMIT 0,1";

                SQLiteParameter[] parameter = { new SQLiteParameter { ParameterName = "@Id" } };
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
                string nameField = "@UserId" + i;
                parameters.Add(new SqlParameter(nameField, usernames[i]));
                usernameStr.Append(nameField);
                if (i < (usernames.Count - 1))
                    usernameStr.Append(",");
            }

            string cmdTxt = @"
                SELECT UserName, UserID
                FROM UserInfo WITH (NOLOCK)
                WHERE UserId IN ({0})";

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
        public static bool SaveDataToLocal(IMBGBetLog bet, string memo)
        {
            try
            {
                #region sql

                string sql = @"
                    INSERT INTO [IMBGProfitLossInfo]
                    (
                        Id, AgentId, UserCode, GameId, RoomId,
                        DealId, DeskId, SeatId, InitMoney, Money,
                        TotalBet, EffectBet, WinLost, WinLostAbs, Fee,
                        PayAmount, AllBills, AllLost, AllWin, OpenTime,
                        EndTime, Memo,
                        localSavedTime, remoteSaved, remoteSavedTime, remoteSaveTryCount, remoteSaveLastTryTime
                    )
                    VALUES
                    (
                        @Id, @AgentId, @UserCode, @GameId, @RoomId,
                        @DealId, @DeskId, @SeatId, @InitMoney, @Money,
                        @TotalBet, @EffectBet, @WinLost, @WinLostAbs, @Fee,
                        @PayAmount, @AllBills, @AllLost, @AllWin, @OpenTime,
                        @EndTime, @Memo,
                        @localSavedTime, @remoteSaved, @remoteSavedTime, @remoteSaveTryCount, @remoteSaveLastTryTime
                    )";

                #endregion sql

                #region par

                SQLiteParameter[] parameter = {
                    new SQLiteParameter { ParameterName = "@Id"},
                    new SQLiteParameter { ParameterName = "@AgentId"},
                    new SQLiteParameter { ParameterName = "@UserCode"},
                    new SQLiteParameter { ParameterName = "@GameId"},
                    new SQLiteParameter { ParameterName = "@RoomId"},
                    new SQLiteParameter { ParameterName = "@DealId"},
                    new SQLiteParameter { ParameterName = "@DeskId"},
                    new SQLiteParameter { ParameterName = "@SeatId"},
                    new SQLiteParameter { ParameterName = "@InitMoney"},
                    new SQLiteParameter { ParameterName = "@Money"},
                    new SQLiteParameter { ParameterName = "@TotalBet"},
                    new SQLiteParameter { ParameterName = "@EffectBet"},
                    new SQLiteParameter { ParameterName = "@WinLost"},
                    new SQLiteParameter { ParameterName = "@WinLostAbs"},
                    new SQLiteParameter { ParameterName = "@Fee"},
                    new SQLiteParameter { ParameterName = "@PayAmount"},
                    new SQLiteParameter { ParameterName = "@AllBills"},
                    new SQLiteParameter { ParameterName = "@AllLost"},
                    new SQLiteParameter { ParameterName = "@AllWin"},
                    new SQLiteParameter { ParameterName = "@OpenTime"},
                    new SQLiteParameter { ParameterName = "@EndTime"},
                    new SQLiteParameter { ParameterName = "@Memo"},
                    new SQLiteParameter { ParameterName = "@localSavedTime" },
                    new SQLiteParameter { ParameterName = "@remoteSaved"},
                    new SQLiteParameter { ParameterName = "@remoteSavedTime"},
                    new SQLiteParameter { ParameterName = "@remoteSaveTryCount"},
                    new SQLiteParameter { ParameterName = "@remoteSaveLastTryTime"}
                };

                parameter[0].Value = bet.Id;
                parameter[1].Value = bet.AgentId;
                parameter[2].Value = bet.UserCode;
                parameter[3].Value = bet.GameId;
                parameter[4].Value = bet.RoomId;
                parameter[5].Value = bet.DealId;
                parameter[6].Value = bet.DeskId;
                parameter[7].Value = bet.SeatId;
                parameter[8].Value = bet.InitMoney;
                parameter[9].Value = bet.Money;
                parameter[10].Value = bet.TotalBet;
                parameter[11].Value = bet.EffectBet;
                parameter[12].Value = bet.WinLost;
                parameter[13].Value = bet.WinLostAbs;
                parameter[14].Value = bet.Fee;
                parameter[15].Value = bet.PayAmount;
                parameter[16].Value = bet.AllBills;
                parameter[17].Value = bet.AllLost;
                parameter[18].Value = bet.AllWin;
                parameter[19].Value = bet.OpenTime;
                parameter[20].Value = bet.EndTime;
                parameter[21].Value = memo;
                parameter[22].Value = DateTime.Now;
                parameter[23].Value = 0;
                parameter[24].Value = DateTime.Now;
                parameter[25].Value = 0;
                parameter[26].Value = DateTime.Now;

                #endregion par

                SQLiteDBHelper.ExecuteNonQuery(dbFullName, sql, parameter);
                return true;
            }
            catch (Exception ex)
            {
                LogsManager.InfoToTelegram("保存IM棋牌亏赢数据到本地 SqlLite 数据库失败，详细信息：" + ex.Message + ",堆栈：" + ex.StackTrace);
                return false;
            }
        }

        ///// <summary>
        ///// 保存数据至远端
        ///// </summary>
        //public static void SaveDataToRemote(IMBGApiParamModel model, Dictionary<int, string> gameNameMapping)
        //{
        //    DataTable dt;

        //    try
        //    {
        //        dt = GetBatchDataFromLocalDB();
        //    }
        //    catch (Exception ex)
        //    {
        //        LogsManager.InfoToTelegram("查询本地IM棋牌数据时失败，详细信息：" + ex.Message + ".堆栈:" + ex.StackTrace);
        //        return;
        //    }

        //    try
        //    {
        //        List<int> notifiedUsers = new List<int>();

        //        if (dt.Rows.Count > 0)
        //        {
        //            Dictionary<string, decimal> userAvailableScores = new Dictionary<string, decimal>();

        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                string userID = dr["UserCode"].ToString().Replace(jxPlayerHeader, "");
        //                string account = dr["UserCode"].ToString();
        //                string BetId = dr["Id"].ToString();
        //                string memo = dr["Memo"] != null ? dr["Memo"].ToString() : "";
        //                int? gameId = null;

        //                if (dr["GameId"] != null && int.TryParse(dr["GameId"].ToString(), out int value))
        //                {
        //                    gameId = value;
        //                }

        //                string gameName = TransactionLogs.GetGameName(gameId);

        //                if (!userAvailableScores.Keys.Contains(userID))
        //                {
        //                    model.PlayerId = account;
        //                    IMBGResp<IMBGBalanceResp> balanceData = Transfer.GetBalance(model);

        //                    if (balanceData != null && balanceData.Data != null &&
        //                        balanceData.Data.Code == (int)APIErrorCode.Success)
        //                    {
        //                        userAvailableScores.Add(userID, balanceData.Data.FreeMoney);
        //                    }
        //                }

        //                DateTime gameStartTime = DateTime.Now;
        //                DateTime gameEndTime = DateTime.Now;

        //                if (!string.IsNullOrEmpty(dr["OpenTime"].ToString()))
        //                {
        //                    gameStartTime = DateTimeUtility.Instance.ToLocalTime(dr["OpenTime"].ToString());
        //                }

        //                if (!string.IsNullOrEmpty(dr["EndTime"].ToString()))
        //                {
        //                    gameEndTime = DateTimeUtility.Instance.ToLocalTime(dr["EndTime"].ToString());
        //                }

        //                decimal availableScores = 0M;

        //                if (userAvailableScores.Keys.Contains(userID.ToString()))
        //                {
        //                    availableScores = userAvailableScores[userID.ToString()];
        //                }
        //                else
        //                {
        //                    var transfer = new Transfer(_environmentUser, DbConnectionTypes.Master);
        //                    decimal freezeScores = 0M;

        //                    //避免把資料更新為0, 重新取得本地餘額
        //                    transfer.SetLocalUserScores(Convert.ToInt32(userID), ref availableScores, ref freezeScores);
        //                }

        //                SqlParameter[] parametersForInsert = {
        //                    new SqlParameter("@UserID", SqlDbType.Int,50),                  //0.Account
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
        //                };

        //                parametersForInsert[0].Value = userID;
        //                parametersForInsert[1].Value = gameStartTime;
        //                parametersForInsert[2].Value = gameEndTime;
        //                parametersForInsert[3].Value = "亏盈";
        //                parametersForInsert[4].Value = dr["AllBills"]; //有效下注额
        //                parametersForInsert[5].Value = dr["WinLost"].ToString();
        //                parametersForInsert[6].Value = decimal.Parse(dr["AllBills"].ToString()) + decimal.Parse(dr["WinLost"].ToString());
        //                parametersForInsert[7].Value = memo;
        //                parametersForInsert[8].Value = BetId;
        //                parametersForInsert[9].Value = null;
        //                parametersForInsert[10].Value = availableScores;

        //                LogsManager.Info("获取到用户 " + account + " 的亏赢单据 " + BetId);

        //                string result = string.Empty;
        //                try
        //                {
        //                    DataSet ds = DbHelperSQL.RunProcedure("Pro_AddIMBGProfitLoss", parametersForInsert, "tab");
        //                    result = ds.Tables[0].Rows[0][0].ToString();
        //                }
        //                catch (Exception ex)
        //                {
        //                    result = ex.Message;
        //                }

        //                //INSERT  IMBGPlayInfo
        //                try
        //                {
        //                    SqlParameter[] Paras = {
        //                        new SqlParameter("@UserID", SqlDbType.NVarChar, 50),
        //                        new SqlParameter("@BetTime", SqlDbType.DateTime),
        //                        new SqlParameter("@ProfitLossTime", SqlDbType.DateTime),
        //                        new SqlParameter("@ProfitLossMoney", SqlDbType.Decimal, 18),
        //                        new SqlParameter("@WinMoney", SqlDbType.Decimal, 18),
        //                        new SqlParameter("@Memo", SqlDbType.NVarChar, 500),
        //                        new SqlParameter("@PalyID", SqlDbType.NVarChar, 50),
        //                        new SqlParameter("@GameType", SqlDbType.NVarChar, 50)
        //                    };

        //                    Paras[0].Value = userID;
        //                    Paras[1].Value = gameStartTime;
        //                    Paras[2].Value = gameEndTime;
        //                    Paras[3].Value = dr["AllBills"];
        //                    Paras[4].Value = dr["WinLost"].ToString();
        //                    Paras[5].Value = memo;
        //                    Paras[6].Value = BetId;
        //                    Paras[7].Value = gameName;
        //                    DataSet ds = DbHelperSQL.RunProcedure("Pro_AddIMBGPlayInfo", Paras, "tab");
        //                }
        //                catch (Exception ex)
        //                {
        //                    LogsManager.Info("保存IM棋牌下注数据 " + BetId + " 到数据库失败，详细信息：" + ex.Message);
        //                }

        //                SQLiteParameter[] parameterForUpdate = {
        //                    new SQLiteParameter { ParameterName = "@Id" }
        //                };

        //                parameterForUpdate[0].Value = dr["Id"].ToString();

        //                if (!string.IsNullOrEmpty(result))
        //                {
        //                    LogsManager.Info("保存IM棋牌亏赢数据 " + BetId + " 到数据库失败，详细信息：" + result);

        //                    try
        //                    {
        //                        string updateSqlForFailure = @"
        //                            UPDATE [IMBGProfitLossInfo]
        //                            SET remoteSaveTryCount = remoteSaveTryCount + 1, remoteSaveLastTryTime = datetime('now', 'localtime')
        //                            WHERE Id = @Id";

        //                        SQLiteDBHelper.ExecuteNonQuery(dbFullName, updateSqlForFailure, parameterForUpdate);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        LogsManager.InfoToTelegram("标识 SqlLite IM棋牌亏赢数据 " + BetId + " 状态为“远程保存失败”时失败，详细信息：" + ex.Message);
        //                    }
        //                }
        //                else
        //                {
        //                    LogsManager.Info("保存IM棋牌亏赢数据 " + BetId + " 到数据库成功");

        //                    if (!notifiedUsers.Contains(int.Parse(userID)))
        //                    {
        //                        notifiedUsers.Add(int.Parse(userID));
        //                    }

        //                    try
        //                    {
        //                        string updateSqlForSuccess = @"
        //                            UPDATE [IMBGProfitLossInfo]
        //                            SET remoteSaved = 1, remoteSavedTime = datetime('now', 'localtime')
        //                            WHERE Id = @Id";

        //                        SQLiteDBHelper.ExecuteNonQuery(dbFullName, updateSqlForSuccess, parameterForUpdate);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        LogsManager.InfoToTelegram("标识 SqlLite IM棋牌亏赢数据 " + BetId + " 状态为“远程保存成功”时失败，详细信息：" + ex.Message);
        //                    }
        //                }
        //            }
        //        }

        //        var messageQueueService = DependencyUtil.ResolveServiceForModel<IMessageQueueService>(JxApplication.IMBGTransferService);

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
            SQLiteDBHelper.ExecuteNonQuery(dbFullName, Sql, parameter);
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