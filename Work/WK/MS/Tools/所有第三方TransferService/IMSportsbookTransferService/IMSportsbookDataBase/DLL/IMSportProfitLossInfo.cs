using IMSportsbookDataBase.Common;
using IMSportsbookDataBase.DBUtility;
using IMSportsbookDataBase.Model;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace IMSportsbookDataBase.DLL
{
    public class IMSportProfitLossInfo : OldProfitLossInfo
    {
        public static readonly int GameNameMaxLength = 197;

        public static readonly bool databaseOnline = true;

        public static readonly string dbFullName = string.Empty;

        protected override string SqliteProfitLossInfoTableName => "IMSportsbookProfitLossInfo";

        protected override int ExecuteNonQuery(string sql, SQLiteParameter[] sqlParams) => SQLiteDBHelper.ExecuteNonQuery(dbFullName, sql, sqlParams);

        protected override DataTable ExecuteDataTable(string sql, SQLiteParameter[] sqlParams) => SQLiteDBHelper.ExecuteDataTable(dbFullName, sql, null);

        static IMSportProfitLossInfo()
        {
            try
            {
                dbFullName = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\data.db";

                if (!File.Exists(dbFullName))
                {
                    SQLiteDBHelper.CreateDataBase(dbFullName);
                }

                if (!SQLiteDBHelper.TableIsExist(dbFullName, "IMSportsbookProfitLossInfo"))
                {
                    SQLiteDBHelper.CreateIMSportsbookProfitLossInfo(dbFullName);
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

                //新增下注结果欄位
                SQLiteDBHelper.AddColumnNX(dbFullName, "IMSportsbookProfitLossInfo", "ResultStatus", "NVARCHAR(10)");

                databaseOnline = true;
            }
            catch (Exception ex)
            {
                LogsManager.InfoToTelegram("初始化 sqllite 数据库失败，详细信息：" + ex.Message + ",堆栈：" + ex.StackTrace);
                databaseOnline = false;
            }
        }

        public static void InIt()
        {
        }

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
                    SELECT BetID
                    FROM [IMSportsbookProfitLossInfo]
                    WHERE BetID=@BetID
                    LIMIT 0,1";

                SQLiteParameter[] parameter = { new SQLiteParameter { ParameterName = "@BetID" } };
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
                {
                    usernameStr.Append(",");
                }
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
        public static bool SaveDataToLocal(SingleBetInfo bet, string memo)
        {
            try
            {
                #region sql

                string sql = @"
                    INSERT INTO [IMSportsbookProfitLossInfo]
                    (
                        UserID, Provider, GameId, BetId, WagerCreationDateTime, PlayerId, ProviderPlayerId, Currency, StakeAmount, MemberExposure,
                        PayoutAmount, WinLoss, ResultStatus, OddsType, WagerType, Platform, IsSettled, IsConfirmed, IsCancelled, BetTradeStatus, BetTradeCommission,
                        BetTradeBuybackAmount, ComboType, LastUpdatedDate,
                        DetailItems, Memo, localSavedTime, remoteSaved, remoteSavedTime, remoteSaveLastTryTime
                    )
                    VALUES
                    (
                        @UserID, @Provider, @GameId, @BetId, @WagerCreationDateTime, @PlayerId, @ProviderPlayerId, @Currency, @StakeAmount, @MemberExposure,
                        @PayoutAmount, @WinLoss, @ResultStatus, @OddsType, @WagerType, @Platform, @IsSettled, @IsConfirmed, @IsCancelled, @BetTradeStatus, @BetTradeCommission,
                        @BetTradeBuybackAmount, @ComboType, @LastUpdatedDate,
                        @DetailItems, @Memo, @localSavedTime, @remoteSaved, @remoteSavedTime, @remoteSaveLastTryTime
                    )";

                #endregion sql

                #region par

                SQLiteParameter[] parameter = {
                    new SQLiteParameter { ParameterName = "@UserID", Value = bet.PlayerId },
                    new SQLiteParameter { ParameterName = "@Provider", Value = bet.Provider },
                    new SQLiteParameter { ParameterName = "@GameId", Value = bet.GameId },
                    new SQLiteParameter { ParameterName = "@BetId", Value = bet.BetId },
                    new SQLiteParameter { ParameterName = "@WagerCreationDateTime", Value = bet.WagerCreationDateTime },
                    new SQLiteParameter { ParameterName = "@PlayerId", Value = bet.PlayerId },
                    new SQLiteParameter { ParameterName = "@ProviderPlayerId", Value = bet.ProviderPlayerId },
                    new SQLiteParameter { ParameterName = "@Currency", Value = bet.Currency },
                    new SQLiteParameter { ParameterName = "@StakeAmount", Value = bet.StakeAmount },
                    new SQLiteParameter { ParameterName = "@MemberExposure", Value = bet.MemberExposure },
                    new SQLiteParameter { ParameterName = "@PayoutAmount", Value = bet.PayoutAmount },
                    new SQLiteParameter { ParameterName = "@WinLoss", Value = bet.WinLoss },
                    new SQLiteParameter { ParameterName = "@ResultStatus", Value = bet.ResultStatus },
                    new SQLiteParameter { ParameterName = "@OddsType", Value = bet.OddsType },
                    new SQLiteParameter { ParameterName = "@WagerType", Value = bet.WagerType },
                    new SQLiteParameter { ParameterName = "@Platform", Value = bet.Platform },
                    new SQLiteParameter { ParameterName = "@IsSettled", Value = bet.IsSettled },
                    new SQLiteParameter { ParameterName = "@IsConfirmed", Value = bet.IsConfirmed },
                    new SQLiteParameter { ParameterName = "@IsCancelled", Value = bet.IsCancelled },
                    new SQLiteParameter { ParameterName = "@BetTradeStatus", Value = bet.BetTradeStatus },
                    new SQLiteParameter { ParameterName = "@BetTradeCommission", Value = bet.BetTradeCommission },
                    new SQLiteParameter { ParameterName = "@BetTradeBuybackAmount", Value = bet.BetTradeBuybackAmount },
                    new SQLiteParameter { ParameterName = "@ComboType", Value =  bet.ComboType },
                    new SQLiteParameter { ParameterName = "@LastUpdatedDate", Value =  bet.LastUpdatedDate },
                    new SQLiteParameter { ParameterName = "@DetailItems", Value = bet.DetailItemsJson },
                    new SQLiteParameter { ParameterName = "@Memo", Value = memo },
                    new SQLiteParameter { ParameterName = "@localSavedTime", Value = DateTime.Now },
                    new SQLiteParameter { ParameterName = "@remoteSaved", Value = 0 },
                    new SQLiteParameter { ParameterName = "@remoteSavedTime", Value = DateTime.Now },
                    new SQLiteParameter { ParameterName = "@remoteSaveLastTryTime", Value = DateTime.Now }
                };

                #endregion par

                SQLiteDBHelper.ExecuteNonQuery(dbFullName, sql, parameter);
                return true;
            }
            catch (Exception ex)
            {
                LogsManager.InfoToTelegram("保存IM体育亏赢数据到本地 SqlLite 数据库失败，详细信息：" + ex.Message + ",堆栈：" + ex.StackTrace);
                return false;
            }
        }

        ///// <summary>
        ///// 保存数据至远端
        ///// </summary>
        //public static void SaveDataToRemote(IMSportApiParamModel model)
        //{
        //    string selectSql = @"
        //        SELECT *
        //        FROM [IMSportsbookProfitLossInfo]
        //        WHERE remoteSaved = 0 AND (remoteSaveTryCount >= 0 AND remoteSaveTryCount < 10)
        //        ORDER BY Id LIMIT 0,100";

        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        dt = SQLiteDBHelper.ExecuteDataTable(dbFullName, selectSql, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogsManager.InfoToTelegram("查询本地IM体育数据时失败，详细信息：" + ex.Message + ".堆栈:" + ex.StackTrace);
        //        return;
        //    }

        //    try
        //    {
        //        List<int> notifiedUsers = new List<int>();

        //        if (dt.Rows.Count > 0)
        //        {
        //            var userAvailableScores = new Dictionary<string, IMSportsbookBalanceInfo>();

        //            string jxPlayerHeader = model.Linecode + "_";

        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                string userID = dr["PlayerId"].ToString().Replace(jxPlayerHeader, "");
        //                string account = dr["PlayerId"].ToString();

        //                if (!userAvailableScores.Keys.Contains(userID))
        //                {
        //                    model.PlayerId = account;
        //                    IMSportsbookBalanceInfo balanceData = Transfer.GetBalance(model);

        //                    if (balanceData != null && balanceData.Code == (int)APIErrorCode.Success)
        //                    {
        //                        userAvailableScores.Add(userID, balanceData);
        //                    }
        //                }

        //                DateTime gameStartTime = DateTime.Now;
        //                DateTime gameEndTime = DateTime.Now;

        //                if (!string.IsNullOrEmpty(dr["WagerCreationDateTime"].ToString()))
        //                {
        //                    gameStartTime = Convert.ToDateTime(dr["WagerCreationDateTime"].ToString());
        //                }

        //                if (!string.IsNullOrEmpty(dr["LastUpdatedDate"].ToString()))
        //                {
        //                    gameEndTime = Convert.ToDateTime(dr["LastUpdatedDate"].ToString());
        //                }

        //                decimal availableScores = 0M;

        //                if (userAvailableScores.Keys.Contains(userID.ToString()))
        //                {
        //                    availableScores = decimal.Parse(userAvailableScores[userID.ToString()].Balance);
        //                }
        //                else
        //                {
        //                    var transfer = new Transfer(_environmentUser, DbConnectionTypes.Master);
        //                    decimal freezeScores = 0M;

        //                    //避免把資料更新為0, 重新取得本地餘額
        //                    transfer.SetLocalUserScores(Convert.ToInt32(userID), ref availableScores, ref freezeScores);
        //                }

        //                SqlParameter[] parametersForInsert = {
        //                    new SqlParameter("@UserID", SqlDbType.Int, 50),                   //0.Account
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
        //                    new SqlParameter("@IsCancelled", SqlDbType.VarChar),            //11.賽事是否取消
        //                };

        //                string betId = dr["BetId"].ToString();
        //                string memo = dr["Memo"].ToString();
        //                string isCancelled = dr["IsCancelled"].ToString();

        //                decimal profitLossMoney = decimal.Parse(dr["StakeAmount"].ToString());
        //                decimal winMoney = decimal.Parse(dr["WinLoss"].ToString());
        //                decimal prizeMoney = profitLossMoney + decimal.Parse(dr["WinLoss"].ToString());

        //                //兌現處理
        //                if (dr["BetTradeStatus"] != null && dr["BetTradeStatus"].ToString() == "Sold")
        //                {
        //                    prizeMoney = decimal.Parse(dr["BetTradeBuyBackAmount"].ToString());
        //                    winMoney = decimal.Parse(dr["BetTradeBuyBackAmount"].ToString()) - profitLossMoney;
        //                }

        //                parametersForInsert[0].Value = userID;
        //                parametersForInsert[1].Value = gameStartTime;
        //                parametersForInsert[2].Value = gameEndTime;
        //                parametersForInsert[3].Value = "亏盈";
        //                parametersForInsert[4].Value = profitLossMoney;
        //                parametersForInsert[5].Value = winMoney;
        //                parametersForInsert[6].Value = prizeMoney;
        //                parametersForInsert[7].Value = memo;
        //                parametersForInsert[8].Value = betId;
        //                parametersForInsert[9].Value = model.ProductWallet;
        //                parametersForInsert[10].Value = availableScores;
        //                parametersForInsert[11].Value = isCancelled;

        //                LogsManager.Info("获取到用户 " + account + " 的亏赢单据 " + betId);

        //                string result = string.Empty;
        //                try
        //                {
        //                    DataSet ds = DbHelperSQL.RunProcedure("Pro_AddIMSportProfitLoss", parametersForInsert, "tab");
        //                    result = ds.Tables[0].Rows[0][0].ToString();
        //                }
        //                catch (Exception ex)
        //                {
        //                    result = ex.Message;
        //                }

        //                string gameName = GetGameName(memo);

        //                //INSERT  IMSportPlayInfo
        //                try
        //                {
        //                    SqlParameter[] Paras = {
        //                                    new SqlParameter("@UserID", SqlDbType.NVarChar, 50),
        //                                    new SqlParameter("@BetTime", SqlDbType.DateTime),
        //                                    new SqlParameter("@ProfitLossTime", SqlDbType.DateTime),
        //                                    new SqlParameter("@ProfitLossMoney", SqlDbType.Decimal, 18),
        //                                    new SqlParameter("@WinMoney", SqlDbType.Decimal, 18),
        //                                    new SqlParameter("@Memo", SqlDbType.NVarChar, 500),
        //                                    new SqlParameter("@PalyID", SqlDbType.NVarChar, 50),
        //                                    new SqlParameter("@GameType", SqlDbType.NVarChar, 200)
        //                                 };

        //                    Paras[0].Value = userID;
        //                    Paras[1].Value = gameStartTime;
        //                    Paras[2].Value = gameEndTime;
        //                    Paras[3].Value = dr["StakeAmount"];
        //                    Paras[4].Value = dr["WinLoss"].ToString();
        //                    Paras[5].Value = memo;
        //                    Paras[6].Value = betId;
        //                    Paras[7].Value = gameName;
        //                    DataSet ds = DbHelperSQL.RunProcedure("Pro_AddIMSportPlayInfo", Paras, "tab");
        //                }
        //                catch (Exception ex)
        //                {
        //                    LogsManager.Info("保存IM体育下注数据 " + betId + " 到数据库失败，详细信息：" + ex.Message);
        //                }

        //                SQLiteParameter[] parameterForUpdate = {
        //                    new SQLiteParameter { ParameterName = "@Id" }
        //                };

        //                parameterForUpdate[0].Value = dr["Id"].ToString();

        //                if (!string.IsNullOrEmpty(result))
        //                {
        //                    LogsManager.Info("保存IM体育亏赢数据 " + betId + " 到数据库失败，详细信息：" + result);

        //                    try
        //                    {
        //                        string updateSqlForFailure = @"
        //                            UPDATE [IMSportsbookProfitLossInfo]
        //                            SET remoteSaveTryCount = remoteSaveTryCount + 1, remoteSaveLastTryTime = datetime('now', 'localtime')
        //                            WHERE Id = @Id";

        //                        SQLiteDBHelper.ExecuteNonQuery(dbFullName, updateSqlForFailure, parameterForUpdate);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        LogsManager.InfoToTelegram("标识 SqlLite IM体育亏赢数据 " + betId + " 状态为“远程保存失败”时失败，详细信息：" + ex.Message);
        //                    }

        //                }
        //                else
        //                {
        //                    LogsManager.Info("保存IM体育亏赢数据 " + betId + " 到数据库成功");

        //                    if (!notifiedUsers.Contains(int.Parse(userID)))
        //                    {
        //                        notifiedUsers.Add(int.Parse(userID));
        //                    }

        //                    try
        //                    {
        //                        string updateSqlForSuccess = @"
        //                            UPDATE [IMSportsbookProfitLossInfo]
        //                            SET remoteSaved = 1, remoteSavedTime = datetime('now', 'localtime')
        //                            WHERE Id = @Id";

        //                        SQLiteDBHelper.ExecuteNonQuery(dbFullName, updateSqlForSuccess, parameterForUpdate);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        LogsManager.InfoToTelegram("标识 SqlLite IM体育亏赢数据 " + betId + " 状态为“远程保存成功”时失败，详细信息：" + ex.Message);
        //                    }
        //                }
        //            }
        //        }

        //        var messageQueueService = DependencyUtil.ResolveServiceForModel<IMessageQueueService>(JxApplication.IMSportTransferService);

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

        private static string GetGameName(string memo)
        {
            string result = string.Empty;

            if (string.IsNullOrWhiteSpace(memo))
            {
                return result;
            }

            string keyworod = "赛事：";
            string[] strArray = memo.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            var results = new List<string>();

            foreach (string content in strArray)
            {
                if (content.Contains(keyworod))
                {
                    results.Add(content.Replace(keyworod, string.Empty));
                }
            }

            if (results.Count() > 0)
            {
                result = string.Join("/", results);
            }

            if (result.Length > GameNameMaxLength)
            {
                result = result.Substring(0, GameNameMaxLength) + "...";
            }

            return result;
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