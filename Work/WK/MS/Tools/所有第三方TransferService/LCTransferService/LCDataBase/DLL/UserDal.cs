using System;
using System.Collections.Generic;
using System.Data;
using Maticsoft.DBUtility;
using LCDataBase.Model;

namespace LCDataBase.DLL
{
    public static class UserDal
    {
        /// <summary>
        /// 转入转出失败时，回滚数据
        /// </summary>
        public static List<LCUserInfo> GetUserInfo()
        {
            List<LCUserInfo> userInfos = new List<LCUserInfo>();

            string sql = @"SELECT   UserID ,
                                    UserName ,
                                    TransferIn ,
                                    TransferOut ,
                                    WinOrLoss ,
                                    Rebate ,
                                    AvailableScores ,
                                    FreezeScores
                            FROM    dbo.LCUserInfo WITH ( NOLOCK );";


            DataSet ds = DbHelperSQL.Query(sql);

            if (ds.Tables != null && ds.Tables.Count > 0)
            {
                var dt = ds.Tables[0];

                foreach (DataRow dr in dt.Rows)
                {
                    string userName = dr["UserName"].ToString();
                    int userId = Convert.ToInt32(dr["UserID"]);

                    decimal? transferIn =  null ;
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

                    LCUserInfo userinfo = new LCUserInfo();
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

        public static List<LCUserInfo> GetNegativeAvailableScoreUsers()
        {
            List<LCUserInfo> userInfos = new List<LCUserInfo>();

            string sql = @"SELECT   UserID ,
                                    UserName ,
                                    TransferIn ,
                                    TransferOut ,
                                    WinOrLoss ,
                                    Rebate ,
                                    AvailableScores ,
                                    FreezeScores
                            FROM    dbo.LCUserInfo WITH ( NOLOCK ) WHERE AvailableScores < 0;";


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

                    LCUserInfo userinfo = new LCUserInfo();
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

        public static bool UpdateAvailableScores(LCUserInfo userInfo)
        {
            string sql = @"
                UPDATE dbo.LCUserInfo 
                SET AvailableScores = " + userInfo.AvailableScores.ToString() + 
                ",FreezeScores = "+ userInfo.FreezeScores.ToString() + 
                " WHERE UserID = " + userInfo.UserID.ToString();

            return DbHelperSQL.ExecuteSql(sql) > 0;
        }
    }
}
