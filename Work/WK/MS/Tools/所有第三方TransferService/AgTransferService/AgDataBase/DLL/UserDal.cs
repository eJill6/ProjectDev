using AgDataBase.Model;
using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;

namespace AgDataBase.DLL
{
    public static class UserDal
    {
        /// <summary>
        /// 转入转出失败时，回滚数据
        /// </summary>
        public static List<AgUserInfo> GetUserInfo()
        {
            List<AgUserInfo> userInfos = new List<AgUserInfo>();

            string sql = @"
                            DECLARE @limit_date DATETIME = DATEADD(MONTH,-3,GETDATE());

                            SELECT   UserID ,
                                    UserName ,
                                    TransferIn ,
                                    TransferOut ,
                                    WinOrLoss ,
                                    Rebate ,
                                    AvailableScores ,
                                    FreezeScores
                            FROM    dbo.AGUserInfo WITH ( NOLOCK )
                            WHERE  (LastAgUpdateTime > @limit_date OR LastFishUpdateTime > @limit_date)
                            ";
            //20210111 過濾掉三個月沒有更新資料的用戶，目前僅有寫入盈虧更新時間

            DataSet ds = DbHelperSQL.Query(sql);

            if (ds.Tables != null && ds.Tables.Count > 0)
            {
                var dt = ds.Tables[0];

                foreach (DataRow dr in dt.Rows)
                {
                    string userName = dr["UserName"].ToString();
                    int userId = Convert.ToInt32(dr["UserID"]);

                    decimal? transferIn = null;
                    if (dr["TransferIn"] != DBNull.Value)
                    {
                        transferIn = Convert.ToDecimal(dr["TransferIn"]);
                    }

                    decimal? transferOut = null;
                    if (dr["TransferOut"] != DBNull.Value)
                    {
                        transferOut = Convert.ToDecimal(dr["TransferOut"]);
                    }

                    decimal? winOrLoss = null;
                    if (dr["WinOrLoss"] != DBNull.Value)
                    {
                        winOrLoss = Convert.ToDecimal(dr["WinOrLoss"]);
                    }

                    decimal? rebate = null;
                    if (dr["Rebate"] != DBNull.Value)
                    {
                        rebate = Convert.ToDecimal(dr["Rebate"]);
                    }

                    decimal? availableScores = null;
                    if (dr["AvailableScores"] != DBNull.Value)
                    {
                        availableScores = Convert.ToDecimal(dr["AvailableScores"]);
                    }

                    decimal? freezeScores = null;
                    if (dr["FreezeScores"] != DBNull.Value)
                    {
                        freezeScores = Convert.ToDecimal(dr["FreezeScores"]);
                    }

                    AgUserInfo userinfo = new AgUserInfo();
                    userinfo.UserID = userId;
                    userinfo.UserName = userName;
                    userinfo.TransferIn = transferIn;
                    userinfo.TransferOut = transferOut;
                    userinfo.WinOrLoss = winOrLoss;
                    userinfo.Rebate = rebate;
                    userinfo.AvailableScores = availableScores;
                    userinfo.FreezeScores = freezeScores;

                    userInfos.Add(userinfo);
                }
            }

            return userInfos;
        }

        public static List<AgUserInfo> GetNegativeAvailableScoreUsers()
        {
            List<AgUserInfo> userInfos = new List<AgUserInfo>();

            string sql = @" DECLARE @limit_date DATETIME = DATEADD(MONTH,-3,GETDATE());

                            SELECT   UserID ,
                                    UserName ,
                                    TransferIn ,
                                    TransferOut ,
                                    WinOrLoss ,
                                    Rebate ,
                                    AvailableScores ,
                                    FreezeScores
                            FROM    dbo.AGUserInfo WITH ( NOLOCK ) WHERE AvailableScores < 0 AND
                                    (LastAgUpdateTime > @limit_date OR LastFishUpdateTime > @limit_date)";
            //20210111 過濾掉三個月沒有更新資料的用戶，目前僅有寫入盈虧更新時間

            DataSet ds = DbHelperSQL.Query(sql);

            if (ds.Tables != null && ds.Tables.Count > 0)
            {
                var dt = ds.Tables[0];

                foreach (DataRow dr in dt.Rows)
                {
                    string userName = dr["UserName"].ToString();
                    int userId = Convert.ToInt32(dr["UserID"]);

                    decimal? transferIn = null;
                    if (dr["TransferIn"] != DBNull.Value)
                    {
                        transferIn = Convert.ToDecimal(dr["TransferIn"]);
                    }

                    decimal? transferOut = null;
                    if (dr["TransferOut"] != DBNull.Value)
                    {
                        transferOut = Convert.ToDecimal(dr["TransferOut"]);
                    }

                    decimal? winOrLoss = null;
                    if (dr["WinOrLoss"] != DBNull.Value)
                    {
                        winOrLoss = Convert.ToDecimal(dr["WinOrLoss"]);
                    }

                    decimal? rebate = null;
                    if (dr["Rebate"] != DBNull.Value)
                    {
                        rebate = Convert.ToDecimal(dr["Rebate"]);
                    }

                    decimal? availableScores = null;
                    if (dr["AvailableScores"] != DBNull.Value)
                    {
                        availableScores = Convert.ToDecimal(dr["AvailableScores"]);
                    }

                    decimal? freezeScores = null;
                    if (dr["FreezeScores"] != DBNull.Value)
                    {
                        freezeScores = Convert.ToDecimal(dr["FreezeScores"]);
                    }

                    AgUserInfo userinfo = new AgUserInfo();
                    userinfo.UserID = userId;
                    userinfo.UserName = userName;
                    userinfo.TransferIn = transferIn;
                    userinfo.TransferOut = transferOut;
                    userinfo.WinOrLoss = winOrLoss;
                    userinfo.Rebate = rebate;
                    userinfo.AvailableScores = availableScores;
                    userinfo.FreezeScores = freezeScores;

                    userInfos.Add(userinfo);
                }
            }

            return userInfos;
        }

        public static bool UpdateAvailableScores(AgUserInfo userInfo)
        {
            string sql = @"UPDATE dbo.AGUserInfo SET AvailableScores = " + userInfo.AvailableScores.ToString() + " WHERE UserID = " + userInfo.UserID.ToString();

            if (DbHelperSQL.ExecuteSql(sql) > 0)
            {
                return true;
            }

            return false;
        }
    }
}