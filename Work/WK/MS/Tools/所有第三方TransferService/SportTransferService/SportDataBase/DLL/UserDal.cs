using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Maticsoft.DBUtility;
using SportDataBase.Model;

namespace SportDataBase.DLL
{
    public static class UserDal
    {
        /// <summary>
        /// 转入转出失败时，回滚数据
        /// </summary>
        public static List<SportUserInfo> GetUserInfo()
        {
            List<SportUserInfo> userInfos = new List<SportUserInfo>();

            string sql = @"SELECT   UserID ,
                                    UserName ,
                                    TransferIn ,
                                    TransferOut ,
                                    WinOrLoss ,
                                    Rebate ,
                                    AvailableScores ,
                                    FreezeScores
                            FROM    dbo.SportUserInfo WITH ( NOLOCK );";


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

                    SportUserInfo userinfo = new SportUserInfo();
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

        public static List<SportUserInfo> GetNegativeAvailableScoreUsers()
        {
            List<SportUserInfo> userInfos = new List<SportUserInfo>();

            string sql = @"SELECT   UserID ,
                                    UserName ,
                                    TransferIn ,
                                    TransferOut ,
                                    WinOrLoss ,
                                    Rebate ,
                                    AvailableScores ,
                                    FreezeScores
                            FROM    dbo.SportUserInfo WITH ( NOLOCK ) WHERE AvailableScores < 0;";


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

                    SportUserInfo userinfo = new SportUserInfo();
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

        public static bool UpdateAvailableScores(SportUserInfo userInfo)
        {
            string sql = @"UPDATE dbo.SportUserInfo SET AvailableScores = " + userInfo.AvailableScores.ToString() + ",FreezeScores = "+userInfo.FreezeScores.ToString()+" WHERE UserID = " + userInfo.UserID.ToString();

            if (DbHelperSQL.ExecuteSql(sql) > 0)
            {
                return true;
            }

            return false;
        }
    }
}
