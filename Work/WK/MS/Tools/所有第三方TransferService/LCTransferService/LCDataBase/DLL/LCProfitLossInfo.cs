using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using JxBackendService.Service.User;
using LCDataBase.BLL;
using LCDataBase.Common;
using LCDataBase.Enums;
using LCDataBase.Model;
using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCDataBase.DLL
{
    public class LCProfitLossInfo : OldProfitLossInfo
    {
        public static readonly bool databaseOnline = true;
        public static readonly string dbFullName = string.Empty;

        protected override string SqliteProfitLossInfoTableName => "LCProfitLossInfo";

        protected override int ExecuteNonQuery(string sql, SQLiteParameter[] sqlParams) => SQLiteDBHelper.ExecuteNonQuery(dbFullName, sql, sqlParams);

        protected override DataTable ExecuteDataTable(string sql, SQLiteParameter[] sqlParams) => SQLiteDBHelper.ExecuteDataTable(dbFullName, sql, null);

        static LCProfitLossInfo()
        {
            try
            {
                dbFullName = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\data.db";

                if (!File.Exists(dbFullName))
                {
                    SQLiteDBHelper.CreateDataBase(dbFullName);
                }

                if (!SQLiteDBHelper.TableIsExist(dbFullName, "LCProfitLossInfo"))
                {
                    SQLiteDBHelper.CreateLCProfitLossInfo(dbFullName);
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
        public static bool ExistsOrder(string trans_id)
        {
            bool isExist = true;
            try
            {
                string sql = @"
                    SELECT GameID
                    FROM [LCProfitLossInfo]
                    WHERE GameID=@GameID
                    LIMIT 0,1";

                SQLiteParameter[] parameter = { new SQLiteParameter { ParameterName = "@GameID" } };
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
        public static bool SaveDataToLocal(SingleBetInfo bet, string memo)
        {
            try
            {
                #region sql
                string sql = @"
                    INSERT INTO [LCProfitLossInfo]
                    (
                        GameID, UserID, Account, ServerID, KindID, TableID, ChairID, UserCount, CardValue,
                        CellScore, AllBet, Profit, Revenue, GameStartTime, GameEndTime, ChannelID,
                        LineCode, Memo, localSavedTime, remoteSaved, remoteSavedTime, remoteSaveLastTryTime
                    )
                    VALUES
                    (
                        @GameID, @UserID, @Account, @ServerID, @KindID, @TableID, @ChairID, @UserCount, @CardValue,
                        @CellScore, @AllBet, @Profit, @Revenue, @GameStartTime, @GameEndTime, @ChannelID,
                        @LineCode, @Memo, @localSavedTime, @remoteSaved, @remoteSavedTime, @remoteSaveLastTryTime
                    )";
                #endregion

                #region par
                SQLiteParameter[] parameter = {
                    new SQLiteParameter { ParameterName = "@GameID" },
                    new SQLiteParameter { ParameterName = "@UserID" },
                    new SQLiteParameter { ParameterName = "@Account" },
                    new SQLiteParameter { ParameterName = "@ServerID" },
                    new SQLiteParameter { ParameterName = "@KindID" },
                    new SQLiteParameter { ParameterName = "@TableID" },
                    new SQLiteParameter { ParameterName = "@ChairID" },
                    new SQLiteParameter { ParameterName = "@UserCount" },
                    new SQLiteParameter { ParameterName = "@CardValue" },
                    new SQLiteParameter { ParameterName = "@CellScore" },
                    new SQLiteParameter { ParameterName = "@AllBet" },
                    new SQLiteParameter { ParameterName = "@Profit" },
                    new SQLiteParameter { ParameterName = "@Revenue" },
                    new SQLiteParameter { ParameterName = "@GameStartTime" },
                    new SQLiteParameter { ParameterName = "@GameEndTime" },
                    new SQLiteParameter { ParameterName = "@ChannelID" },
                    new SQLiteParameter { ParameterName = "@LineCode" },
                    new SQLiteParameter { ParameterName = "@Memo" },
                    new SQLiteParameter { ParameterName = "@localSavedTime" },
                    new SQLiteParameter { ParameterName = "@remoteSaved"},
                    new SQLiteParameter { ParameterName = "@remoteSavedTime"},
                    new SQLiteParameter { ParameterName = "@remoteSaveLastTryTime"}
                };

                parameter[0].Value = bet.GameID;
                parameter[1].Value = bet.UserID;
                parameter[2].Value = bet.Account;
                parameter[3].Value = bet.ServerID;
                parameter[4].Value = bet.KindID;
                parameter[5].Value = bet.TableID;
                parameter[6].Value = bet.ChairID;
                parameter[7].Value = bet.UserCount;
                parameter[8].Value = bet.CardValue;
                parameter[9].Value = bet.CellScore;
                parameter[10].Value = bet.AllBet;
                parameter[11].Value = bet.Profit;
                parameter[12].Value = bet.Revenue;
                parameter[13].Value = bet.GameStartTime;
                parameter[14].Value = bet.GameEndTime;
                parameter[15].Value = bet.ChannelID;
                parameter[16].Value = bet.LineCode;
                parameter[17].Value = memo;
                parameter[18].Value = DateTime.Now;
                parameter[19].Value = 0;
                parameter[20].Value = DateTime.Now;
                parameter[21].Value = DateTime.Now;
                #endregion

                SQLiteDBHelper.ExecuteNonQuery(dbFullName, sql, parameter);
                return true;
            }
            catch (Exception ex)
            {
                LogsManager.InfoToTelegram("保存棋牌亏赢数据到本地 SqlLite 数据库失败，详细信息：" + ex.Message + ",堆栈：" + ex.StackTrace);
                return false;
            }
        }

        ///// <summary>
        ///// 保存数据至远端
        ///// </summary>
        //public static void SaveDataToRemote(LCApiParamModel model)
        //{
        //    var transfer = new Transfer(_environmentUser, DbConnectionTypes.Master);

        //    string selectSql = @"
        //        SELECT *
        //        FROM [LCProfitLossInfo]
        //        WHERE remoteSaved = 0 AND (remoteSaveTryCount >= 0 AND remoteSaveTryCount < 10)
        //        ORDER BY Id LIMIT 0,100";

        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        dt = SQLiteDBHelper.ExecuteDataTable(dbFullName, selectSql, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogsManager.InfoToTelegram("查询本地棋牌数据时失败，详细信息：" + ex.Message + ".堆栈:" + ex.StackTrace);
        //        return;
        //    }

        //    try
        //    {
        //        List<int> notifiedUsers = new List<int>();

        //        if (dt.Rows.Count > 0)
        //        {
        //            Dictionary<string, LCBalanceInfo> userAvailableScores = new Dictionary<string, LCBalanceInfo>();

        //            string lcUserHeader = model.AgentID + "_";

        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                string userID = dr["UserID"].ToString();
        //                string account = dr["Account"].ToString();
        //                if (!userAvailableScores.Keys.Contains(userID))
        //                {
        //                    model.Account = account.Replace(lcUserHeader, "");

        //                    var balanceData = Transfer.GetBalance(model);
        //                    if (balanceData != null && balanceData.Data != null && balanceData.Data.Code == (int)APIErrorCode.Success)
        //                    {
        //                        userAvailableScores.Add(userID, balanceData.Data);
        //                    }
        //                }
        //            }

        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                int userId = Convert.ToInt32(dr["UserID"].ToString());
        //                string gameId = dr["GameID"].ToString();
        //                string accountId = dr["Account"].ToString();

        //                DateTime gameStartTime = DateTime.Now;
        //                DateTime gameEndTime = DateTime.Now;
        //                if (!string.IsNullOrEmpty(dr["GameStartTime"].ToString()))
        //                {
        //                    gameStartTime = Convert.ToDateTime(dr["GameStartTime"].ToString());
        //                }
        //                if (!string.IsNullOrEmpty(dr["GameEndTime"].ToString()))
        //                {
        //                    gameEndTime = Convert.ToDateTime(dr["GameEndTime"].ToString());
        //                }

        //                var availableScores = 0M;
        //                var freezeScores = 0M;

        //                if (userAvailableScores.Keys.Contains(userId.ToString()))
        //                {
        //                    availableScores = userAvailableScores[userId.ToString()].TotalMoney;
        //                    freezeScores = availableScores - userAvailableScores[userId.ToString()].FreeMoney;
        //                }
        //                else
        //                {
        //                    //避免把資料更新為0, 重新取得本地餘額
        //                    transfer.SetLocalUserScores(userId, ref availableScores, ref freezeScores);
        //                }

        //                var playGameName = EnumHelper<GameKind>
        //                    .GetEnumDescription(
        //                        Enum.GetName(typeof(GameKind),
        //                        Convert.ToInt32(dr["KindID"].ToString())));

        //                var playRoomName = EnumHelper<RoomType>
        //                    .GetEnumDescription(
        //                        Enum.GetName(typeof(RoomType),
        //                        Convert.ToInt32(dr["ServerID"].ToString())));

        //                var playGameAllName = playGameName + (string.IsNullOrEmpty(playRoomName) ? "" : "-" + playRoomName);

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
        //                    new SqlParameter("@FreezeScores", SqlDbType.Decimal, 18),       //11.可用金额
        //                    new SqlParameter("@AllBetMoney", SqlDbType.Decimal, 18)         //12.總投注額
        //                };

        //                parametersForInsert[0].Value = userId;
        //                parametersForInsert[1].Value = gameStartTime;
        //                parametersForInsert[2].Value = gameEndTime;
        //                parametersForInsert[3].Value = "亏盈";
        //                parametersForInsert[4].Value = dr["CellScore"];
        //                parametersForInsert[5].Value = dr["Profit"].ToString();
        //                parametersForInsert[6].Value = decimal.Parse(dr["CellScore"].ToString()) + decimal.Parse(dr["Profit"].ToString());
        //                parametersForInsert[7].Value = dr["Memo"].ToString();
        //                parametersForInsert[8].Value = gameId;
        //                parametersForInsert[9].Value = playGameAllName;
        //                parametersForInsert[10].Value = availableScores;
        //                parametersForInsert[11].Value = freezeScores;
        //                parametersForInsert[12].Value = dr["AllBet"];

        //                LogsManager.Info("获取到用户 " + accountId + " 的亏赢单据 " + gameId);

        //                string result = string.Empty;
        //                try
        //                {
        //                    DataSet ds = DbHelperSQL.RunProcedure("Pro_AddLCProfitLoss", parametersForInsert, "tab");
        //                    result = ds.Tables[0].Rows[0][0].ToString();
        //                }
        //                catch (Exception ex)
        //                {
        //                    result = ex.Message;
        //                }

        //                SQLiteParameter[] parameterForUpdate = {
        //                    new SQLiteParameter { ParameterName = "@Id" }
        //                };

        //                parameterForUpdate[0].Value = dr["Id"].ToString();

        //                //INSERT LC
        //                try
        //                {
        //                    SqlParameter[] Paras = {
        //                        new SqlParameter("@UserID",SqlDbType.Int,50),
        //                        new SqlParameter("@BetTime", SqlDbType.DateTime),
        //                        new SqlParameter("@ProfitLossTime", SqlDbType.DateTime),
        //                        new SqlParameter("@ProfitLossMoney", SqlDbType.Decimal, 18),
        //                        new SqlParameter("@WinMoney", SqlDbType.Decimal, 18),
        //                        new SqlParameter("@Memo", SqlDbType.NVarChar, 500),
        //                        new SqlParameter("@PalyID", SqlDbType.NVarChar, 50),
        //                        new SqlParameter("@GameType", SqlDbType.NVarChar, 50),
        //                    };

        //                    Paras[0].Value = userId;
        //                    Paras[1].Value = gameStartTime;
        //                    Paras[2].Value = gameEndTime;
        //                    Paras[3].Value = dr["CellScore"];
        //                    Paras[4].Value = dr["Profit"].ToString();
        //                    Paras[5].Value = dr["Memo"].ToString();
        //                    Paras[6].Value = gameId;
        //                    Paras[7].Value = playGameName;

        //                    DataSet ds = DbHelperSQL.RunProcedure("Pro_AddLCPlayInfo", Paras, "tab");
        //                }
        //                catch (Exception ex)
        //                {
        //                    LogsManager.Info("保存棋牌下注数据 " + gameId + " 到数据库失败，详细信息：" + ex.Message);
        //                }

        //                if (!string.IsNullOrEmpty(result))
        //                {
        //                    LogsManager.Info("保存棋牌亏赢数据 " + gameId + " 到数据库失败，详细信息：" + result);

        //                    try
        //                    {
        //                        string updateSqlForFailure = @"
        //                            UPDATE [LCProfitLossInfo] 
        //                            SET remoteSaveTryCount = remoteSaveTryCount + 1, remoteSaveLastTryTime = datetime('now', 'localtime') 
        //                            WHERE Id = @Id";

        //                        SQLiteDBHelper.ExecuteNonQuery(dbFullName, updateSqlForFailure, parameterForUpdate);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        LogsManager.InfoToTelegram("标识 SqlLite 棋牌亏赢数据 " + gameId + " 状态为“远程保存失败”时失败，详细信息：" + ex.Message);
        //                    }
        //                }
        //                else
        //                {
        //                    LogsManager.Info("保存棋牌亏赢数据 " + gameId + " 到数据库成功");

        //                    if (!notifiedUsers.Contains(userId))
        //                    {
        //                        notifiedUsers.Add(userId);
        //                    }

        //                    try
        //                    {
        //                        string updateSqlForSuccess = @"
        //                            UPDATE [LCProfitLossInfo] 
        //                            SET remoteSaved = 1, remoteSavedTime = datetime('now', 'localtime') 
        //                            WHERE Id = @Id";

        //                        SQLiteDBHelper.ExecuteNonQuery(dbFullName, updateSqlForSuccess, parameterForUpdate);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        LogsManager.InfoToTelegram("标识 SqlLite 棋牌亏赢数据 " + gameId + " 状态为“远程保存成功”时失败，详细信息：" + ex.Message);
        //                    }
        //                }
        //            }
        //        }

        //        var messageQueueService = DependencyUtil.ResolveServiceForModel<IMessageQueueService>(JxApplication.LCTransferService);

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
