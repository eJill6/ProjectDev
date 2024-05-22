using JxBackendService.Common.Util;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using Maticsoft.DBUtility;
using SportDataBase.Common;
using SportDataBase.Model;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace SportDataBase.DLL
{
    public class SportProfitLossInfo : OldProfitLossInfo
    {
        public static readonly bool dataBaseOnline = true;

        public static readonly string dbFullName = string.Empty;

        protected override string SqliteProfitLossInfoTableName => "SportProfitLossInfo";

        protected override int ExecuteNonQuery(string sql, SQLiteParameter[] sqlParams) => SQLiteDBHelper.ExecuteNonQuery(dbFullName, sql, sqlParams);

        protected override DataTable ExecuteDataTable(string sql, SQLiteParameter[] sqlParams) => SQLiteDBHelper.ExecuteDataTable(dbFullName, sql, null);

        static SportProfitLossInfo()
        {
            try
            {
                dbFullName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "data.db");

                if (!File.Exists(dbFullName))
                {
                    SQLiteDBHelper.CreateDataBase(dbFullName);
                }

                if (!SQLiteDBHelper.TableIsExist(dbFullName, "SportProfitLossInfo"))
                {
                    SQLiteDBHelper.CreateSportProfitLossInfo(dbFullName);
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

                //新增盤口欄位
                SQLiteDBHelper.AddColumnNX(dbFullName, "SportProfitLossInfo", "Odds_Type", "NVARCHAR(20)");
                //額外狀態
                SQLiteDBHelper.AddColumnNX(dbFullName, "SportProfitLossInfo", "ticket_extra_status", "NVARCHAR(50)");
                //運動類型
                SQLiteDBHelper.AddColumnNX(dbFullName, "SportProfitLossInfo", "sport_type_text", "NVARCHAR(100)");
                //串關資料json
                SQLiteDBHelper.AddColumnNX(dbFullName, "SportProfitLossInfo", "ParlayDataJson", "NVARCHAR(2000)");
                //新增 注单结算的时间 欄位
                SQLiteDBHelper.AddColumnNX(dbFullName, "SportProfitLossInfo", "settlement_time", "NVARCHAR(100)");

                dataBaseOnline = true;
            }
            catch (Exception ex)
            {
                LogsManager.InfoToEmail("初始化sqllite数据库失败，详细信息：" + ex.Message + ",堆栈：" + ex.StackTrace);
                dataBaseOnline = false;
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
            try
            {
                string sql = @"
                                    SELECT
                                        trans_id
                                    FROM [SportProfitLossInfo]
                                    WHERE trans_id=@trans_id
                                    LIMIT 0,1
                                   ";

                SQLiteParameter[] parameter = {
                                               new SQLiteParameter { ParameterName = "@trans_id" }
                                           };
                parameter[0].Value = trans_id;

                if (SQLiteDBHelper.ExecuteScalar(dbFullName, sql, parameter) == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                LogsManager.Info("查询本地单号时错误：错误信息:" + ex.Message + ",错误堆栈：" + ex.StackTrace);
                return true;
            }
        }

        /// <summary>
        /// 保存在本地数据库
        /// </summary>
        /// <returns></returns>
        public static bool SaveDataToLocal(ISabaSportBetDetailInfo detailInfo, ISabaSportBetDetailName detailName)
        {
            try
            {
                #region sql

                string sql = @"
                    INSERT INTO [SportProfitLossInfo](
                        trans_id,
                        sport_type,
                        sport_type_text,
                        after_amount,
                        away_id,
                        away_id_text,
                        bet_type,
                        bet_type_text,
                        currency,
                        home_id,
                        home_id_text,
                        league_id,
                        league_id_text,
                        match_datetime,
                        winlost_datetime,
                        transaction_time,
                        match_id,
                        match_id_text,
                        odds_type,
                        odds,
                        stake,
                        ticket_status,
                        ticket_extra_status,
                        version_key,
                        winlost_amount,
                        vendor_member_id,
                        localSavedTime,
                        Memo,
                        ParlayDataJson,
                        remoteSaved,
                        remoteSavedTime,
                        remoteSaveLastTryTime,
                        settlement_time)
                    VALUES(
                        @trans_id,
                        @sport_type,
                        @sport_type_text,
                        @after_amount,
                        @away_id,
                        @away_id_text,
                        @bet_type,
                        @bet_type_text,
                        @currency,
                        @home_id,
                        @home_id_text,
                        @league_id,
                        @league_id_text,
                        @match_datetime,
                        @winlost_datetime,
                        @transaction_time,
                        @match_id,
                        '',
                        @odds_type,
                        @odds,
                        @stake,
                        @ticket_status,
                        @ticket_extra_status,
                        @version_key,
                        @winlost_amount,
                        @vendor_member_id,
                        @localSavedTime,
                        @Memo,
                        @ParlayDataJson,
                        @remoteSaved,
                        @remoteSavedTime,
                        @remoteSaveLastTryTime,
                        @settlement_time)";

                #endregion sql

                #region parameters

                DateTime now = DateTime.Now;

                SQLiteParameter[] parameter =
                {
                    new SQLiteParameter { ParameterName = "@trans_id", Value = detailInfo.Trans_id },
                    new SQLiteParameter { ParameterName = "@sport_type", Value = detailInfo.Sport_type},
                    new SQLiteParameter { ParameterName = "@sport_type_text", Value = detailName.SportTypeName},
                    new SQLiteParameter { ParameterName = "@after_amount", Value = detailInfo.After_amount},
                    new SQLiteParameter { ParameterName = "@away_id", Value = detailInfo.Away_id},
                    new SQLiteParameter { ParameterName = "@away_id_text", Value = detailName.AwayName},
                    new SQLiteParameter { ParameterName = "@bet_type", Value = detailInfo.Bet_type},
                    new SQLiteParameter { ParameterName = "@bet_type_text", Value = detailName.BetTypeText},
                    new SQLiteParameter { ParameterName = "@currency", Value = detailInfo.Currency},
                    new SQLiteParameter { ParameterName = "@home_id", Value = detailInfo.Home_id},
                    new SQLiteParameter { ParameterName = "@home_id_text", Value = detailName.HomeName},
                    new SQLiteParameter { ParameterName = "@league_id", Value = detailInfo.League_id},
                    new SQLiteParameter { ParameterName = "@league_id_text", Value = detailName.LeagueName},
                    new SQLiteParameter { ParameterName = "@match_datetime", Value = detailInfo.Match_datetime},
                    new SQLiteParameter { ParameterName = "@winlost_datetime", Value = detailInfo.Winlost_datetime},
                    new SQLiteParameter { ParameterName = "@transaction_time", Value = detailInfo.Transaction_time},
                    new SQLiteParameter { ParameterName = "@match_id", Value = detailInfo.Match_id},
                    new SQLiteParameter { ParameterName = "@odds_type", Value = detailInfo.Odds_type},
                    new SQLiteParameter { ParameterName = "@odds", Value = detailInfo.Odds},
                    new SQLiteParameter { ParameterName = "@stake", Value = detailInfo.Stake},
                    new SQLiteParameter { ParameterName = "@ticket_status", Value = detailInfo.Ticket_status},
                    new SQLiteParameter { ParameterName = "@ticket_extra_status", Value = detailInfo.Ticket_extra_status},
                    new SQLiteParameter { ParameterName = "@version_key", Value = detailInfo.Version_key},
                    new SQLiteParameter { ParameterName = "@winlost_amount", Value = detailInfo.Winlost_amount},
                    new SQLiteParameter { ParameterName = "@vendor_member_id", Value = detailInfo.Vendor_member_id},
                    new SQLiteParameter { ParameterName = "@localSavedTime", Value = now},
                    new SQLiteParameter { ParameterName = "@Memo", Value = detailName.Memo},
                    new SQLiteParameter { ParameterName = "@ParlayDataJson", Value = detailInfo.ParlayData.ToJsonString()},
                    new SQLiteParameter { ParameterName = "@remoteSaved", Value = 0},
                    new SQLiteParameter { ParameterName = "@remoteSavedTime", Value = now},
                    new SQLiteParameter { ParameterName = "@remoteSaveLastTryTime", Value = now},
                    new SQLiteParameter { ParameterName = "@settlement_time", Value = detailInfo.Settlement_time}
                };

                #endregion parameters

                SQLiteDBHelper.ExecuteNonQuery(dbFullName, sql, parameter);
                return true;
            }
            catch (Exception ex)
            {
                LogsManager.InfoToEmail("保存体育亏赢数据到本地SqlLite数据库失败，详细信息：" + ex.Message + ",堆栈：" + ex.StackTrace);
                return false;
            }
        }

        ///// <summary>
        ///// 保存数据至远端
        ///// </summary>
        //public static void SaveDataToRemote()
        //{
        //    Dictionary<string, UserBalanceItem> userAvailableScores = new Dictionary<string, UserBalanceItem>();

        //    List<int> notifiedUsers = new List<int>();

        //    string selectSql = @"
        //                    SELECT
        //                        *
        //                    FROM [SportProfitLossInfo]
        //                    WHERE remoteSaved=0 AND (remoteSaveTryCount >= 0 AND remoteSaveTryCount < 10)
        //                    ORDER BY Id  LIMIT 0,100
        //                    ";

        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        dt = SQLiteDBHelper.ExecuteDataTable(dbFullName, selectSql, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogsManager.InfoToEmail("查询本地体育数据时失败，详细信息：" + ex.Message + ".堆栈:" + ex.StackTrace);
        //    }

        //    if (dt.Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            string tpgameAccount = dr["vendor_member_id"].ToString();//第三方帳號

        //            if (!userAvailableScores.Keys.Contains(tpgameAccount))
        //            {
        //                //去打第三方拿到餘額資訊
        //                var balanceData = Utility.GetBalance(tpgameAccount);

        //                if (balanceData != null && balanceData.Data != null && balanceData.error_code == 0)
        //                {
        //                    userAvailableScores.Add(tpgameAccount, balanceData.Data);
        //                }
        //            }
        //        }
        //    }

        //    try
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            //這裡拿到的UserId可能會因環境而有前詞例如 JxSIT_{UserId}，所以要用他們傳回的UserId反推我們的UserId，才能寫回我們的DB
        //            //分環境取得要打過去沙巴的UserId
        //            string venderMemberId = dr["vendor_member_id"].ToString();

        //            BaseReturnDataModel<UserAccountSearchReault> returnModel = TPGameAccountReadService.GetByTPGameAccount
        //            (
        //                PlatformProduct.Sport,
        //                venderMemberId
        //            );

        //            bool foundUser = returnModel.IsSuccess && returnModel.DataModel != null;

        //            if (!foundUser && int.TryParse(venderMemberId, out int memberId))
        //            {
        //                LogsManager.Error("資料異常: " + new { environmentCode, venderMemberId }.ToJsonString());
        //            }

        //            string id = dr["id"].ToString();
        //            //8.订单编号
        //            string palyId = dr["trans_id"].ToString();

        //            if (!foundUser)
        //            {
        //                if (venderMemberId.Contains("d3_") || venderMemberId.Contains("d9_"))
        //                {
        //                    //D3 D9資料更新失敗次數+10 ，使跳過
        //                    UpdateSqlLiteProfitLossInfo(SqlliteUpdateType.UpdateD3Failure, id, palyId);
        //                }
        //                else
        //                {
        //                    //其他資料增加失敗次數
        //                    UpdateSqlLiteProfitLossInfo(SqlliteUpdateType.UpdateFailure, id, palyId);
        //                }

        //                continue;
        //            }

        //            //0.Account
        //            int userId = returnModel.DataModel.PlatformAccountSearchResult.UserID;// LocalUserID;
        //            //1.下注时间
        //            DateTime betTime = DateTime.Now;
        //            //2.开奖时间
        //            DateTime profitLossTime = DateTime.Now;
        //            //3.亏赢类型，SP沒有使用這個變數
        //            string profitLossType = "亏盈";
        //            //4.有效下注额
        //            string profitLossMoney = dr["stake"].ToString();
        //            //5.亏赢
        //            string winMoney = dr["winlost_amount"].ToString();
        //            //6.有效下注额 + 亏赢
        //            decimal prizeMoney = decimal.Parse(dr["stake"].ToString()) + decimal.Parse(dr["winlost_amount"].ToString());
        //            //7.备注
        //            string memo = dr["memo"].ToString();
        //            //9.游戏类型
        //            string gameType = dr["Sport_type"].ToString();
        //            string gameName = GetGameName(dr["league_id_text"].ToString(), dr["home_id_text"].ToString(), dr["away_id_text"].ToString());

        //            SqlParameter[] parametersForInsert = {
        //                new SqlParameter("@UserID",SqlDbType.Int),
        //                new SqlParameter("@BetTime",SqlDbType.DateTime),
        //                new SqlParameter("@ProfitLossTime",SqlDbType.DateTime),
        //                new SqlParameter("@ProfitLossType",SqlDbType.NVarChar,50),
        //                new SqlParameter("@ProfitLossMoney",SqlDbType.Decimal,18),
        //                new SqlParameter("@WinMoney",SqlDbType.Decimal,18),
        //                new SqlParameter("@PrizeMoney",SqlDbType.Decimal,18),
        //                new SqlParameter("@Memo",SqlDbType.NVarChar,500),
        //                new SqlParameter("@PalyID",SqlDbType.NVarChar,50),
        //                new SqlParameter("@GameType",SqlDbType.NVarChar,50),
        //                new SqlParameter("@AvailableScores",SqlDbType.Decimal,18),
        //                new SqlParameter("@FreezeScores",SqlDbType.Decimal,18)};

        //            DateTime.TryParse(dr["transaction_time"].ToString().Replace("T", " "), out betTime);
        //            DateTime.TryParse(dr["winlost_datetime"].ToString().Replace("T", " "), out profitLossTime);

        //            LogsManager.Info("获取到用户 " + dr["vendor_member_id"].ToString() + " 的亏赢单据 " + dr["trans_id"].ToString());
        //            parametersForInsert[0].Value = userId;
        //            parametersForInsert[1].Value = betTime;
        //            parametersForInsert[2].Value = profitLossTime;
        //            parametersForInsert[3].Value = profitLossType;
        //            parametersForInsert[4].Value = profitLossMoney;
        //            parametersForInsert[5].Value = winMoney;
        //            parametersForInsert[6].Value = prizeMoney;
        //            parametersForInsert[7].Value = memo;
        //            parametersForInsert[8].Value = palyId;
        //            parametersForInsert[9].Value = gameType;

        //            decimal availableScores = 0M;
        //            decimal freezeScores = 0M;

        //            if (userAvailableScores.Keys.Contains(dr["vendor_member_id"].ToString()))
        //            {
        //                availableScores = userAvailableScores[dr["vendor_member_id"].ToString()].balance.GetValueOrDefault();// agInfo.beforeCredit + WinMoney;
        //                freezeScores = userAvailableScores[dr["vendor_member_id"].ToString()].outstanding.GetValueOrDefault();
        //            }
        //            else
        //            {
        //                var transfer = new Transfer(_environmentUser, DbConnectionTypes.Master);

        //                //避免把資料更新為0, 重新取得本地餘額
        //                transfer.SetLocalUserScores(Convert.ToInt32(userId), ref availableScores, ref freezeScores);
        //            }

        //            parametersForInsert[10].Value = availableScores;
        //            parametersForInsert[11].Value = freezeScores;

        //            string result = string.Empty;
        //            try
        //            {
        //                DataSet ds = DbHelperSQL.RunProcedure("Pro_AddSportProfitLoss", parametersForInsert, "tab");

        //                result = ds.Tables[0].Rows[0][0].ToString();

        //            }
        //            catch (Exception ex)
        //            {
        //                result = ex.Message;
        //            }

        //            //INSERT  SportPlayInfo
        //            try
        //            {
        //                SqlParameter[] Paras = {
        //                        new SqlParameter("@UserID", SqlDbType.NVarChar, 50),
        //                        new SqlParameter("@BetTime", SqlDbType.DateTime),
        //                        new SqlParameter("@ProfitLossTime", SqlDbType.DateTime),
        //                        new SqlParameter("@ProfitLossMoney", SqlDbType.Decimal, 18),
        //                        new SqlParameter("@WinMoney", SqlDbType.Decimal, 18),
        //                        new SqlParameter("@Memo", SqlDbType.NVarChar, 500),
        //                        new SqlParameter("@PalyID", SqlDbType.NVarChar, 50),
        //                        new SqlParameter("@GameType", SqlDbType.NVarChar, 200)
        //                    };

        //                Paras[0].Value = userId;
        //                Paras[1].Value = betTime;
        //                Paras[2].Value = profitLossTime;
        //                Paras[3].Value = profitLossMoney;
        //                Paras[4].Value = winMoney;
        //                Paras[5].Value = memo;
        //                Paras[6].Value = palyId;
        //                Paras[7].Value = gameName;
        //                DataSet ds = DbHelperSQL.RunProcedure("Pro_AddSportPlayInfo", Paras, "tab");
        //            }
        //            catch (Exception ex)
        //            {
        //                LogsManager.Info("保存体育下注数据 " + palyId + " 到数据库失败，详细信息：" + ex.Message);
        //            }

        //            if (!string.IsNullOrEmpty(result))
        //            {
        //                LogsManager.InfoToEmail("保存体育亏赢数据 " + dr["trans_id"].ToString() + " 到数据库失败，详细信息：" + result);
        //                UpdateSqlLiteProfitLossInfo(SqlliteUpdateType.UpdateFailure, id, palyId);
        //            }
        //            else
        //            {
        //                LogsManager.Info("保存体育亏赢数据 " + dr["trans_id"].ToString() + " 到数据库成功");
        //                if (!notifiedUsers.Contains(userId))
        //                {
        //                    notifiedUsers.Add(userId);
        //                }
        //                UpdateSqlLiteProfitLossInfo(SqlliteUpdateType.UpdateSuccess, id, palyId);
        //            }
        //        }

        //        var messageQueueService = DependencyUtil.ResolveServiceForModel<IMessageQueueService>(JxApplication.SportTransferService);

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
        //        LogsManager.InfoToEmail("保存数据到远端时错误:" + ex.Message + ex.StackTrace);
        //    }
        //}

        public static void UpdateSqlLiteProfitLossInfo(SqlliteUpdateType type, string id, string transId)
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

        private static void UpdateSqlLiteProfitLossInfo(string updateSql, string id, string transId, string memo)
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
                LogsManager.InfoToEmail("标识SqlLite体育亏赢数据 " + transId + $" 状态为“{memo}”时失败，详细信息：" + ex.Message);
            }
        }

        /// <summary>
        /// 保存last_version_key
        /// </summary>
        public static void UpdateVersion_key(string version_key)
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
                LogsManager.InfoToEmail("更新last_version_key时失败，Key：" + version_key + "详细信息：" + ex.Message + ",堆栈：" + ex.Message);
            }
        }

        /// <summary>
        /// 查询last_version_key
        /// </summary>
        /// <param name="trans_id"></param>
        /// <returns></returns>
        public static string SelectVersion_key()
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
                LogsManager.Info("查询version_key时错误：错误信息:" + ex.Message + ",错误堆栈：" + ex.StackTrace);
                return "";
            }
        }        

        public static string GetGameName(string leagueIdText, string homeIdText, string awayIdText)
        {
            int GameNameMaxLength = 197;
            string result = string.Empty;

            if (string.IsNullOrWhiteSpace(leagueIdText) &&
                string.IsNullOrWhiteSpace(homeIdText) &&
                string.IsNullOrWhiteSpace(awayIdText))
            {
                return result;
            }

            result = string.Format("{0}，{1}，{2}", leagueIdText, homeIdText, awayIdText);

            if (result.Length > GameNameMaxLength)
            {
                result = result.Substring(0, GameNameMaxLength) + "...";
            }

            return result;
        }
    }
}