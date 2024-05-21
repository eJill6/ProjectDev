using System;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using SLPolyGame.Web.DBUtility;//Please add references

namespace SLPolyGame.Web.DAL
{
    /// <summary>
    /// 数据访问类:SysSettings
    /// </summary>
    public class SysSettings
    {
        public SysSettings()
        { }

        #region Method

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperSQL.GetMaxID("SettingsID", "SysSettings");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int SettingsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from SysSettings");
            strSql.Append(" where SettingsID=@SettingsID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@SettingsID", SqlDbType.Int,4)};
            parameters[0].Value = SettingsID;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int SettingsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from SysSettings ");
            strSql.Append(" where SettingsID=@SettingsID");
            SqlParameter[] parameters = {
                    new SqlParameter("@SettingsID", SqlDbType.Int,4)
};
            parameters[0].Value = SettingsID;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string SettingsIDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from SysSettings ");
            strSql.Append(" where SettingsID in (" + SettingsIDlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion Method

        /// <summary>
        /// 获取系统配置信息
        /// </summary>
        public SLPolyGame.Web.Model.SysSettings GetSysSettings()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"
                    SELECT  TOP 1
                        SettingsID,
                        ISNULL(MaxBetCount,0) AS MaxBetCount,
                        MaxOneBetMoney,
                        ISNULL(MaxOneBetMoney,0) AS MaxOneBetMoney,
                        ISNULL(MinOneBetMoney,0) AS MinOneBetMoney,
                        ISNULL(MaxBonusMoney,0) AS MaxBonusMoney,
                        ISNULL(MinMoneyIn,0) AS MinMoneyIn,
                        ISNULL(MaxUserRebatePro,0) AS MaxUserRebatePro
                        FROM SysSettings WITH(NOLOCK) ");

            //            strSql.Append(@"
            //select  top 1 SettingsID,isnull(MaxBetCount,0) as MaxBetCount,isnull(MinMoneyOut,0) as MinMoneyOut,
            //isnull(MaxMoneyOut,0) as MaxMoneyOut,isnull(MaxMoneyOutCount,0)as MaxMoneyOutCount,isnull(MaxOneBetMoney,0) as MaxOneBetMoney,
            //isnull(MinOneBetMoney,0)as MinOneBetMoney,isnull(MaxBonusMoney,0)as MaxBonusMoney,
            //isnull(MinMoneyIn,0)as MinMoneyIn,isnull(MaxUserRebatePro,0)as MaxUserRebatePro from SysSettings with(nolock) ");

            SLPolyGame.Web.Model.SysSettings model = new SLPolyGame.Web.Model.SysSettings();
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["SettingsID"].ToString() != "")
                {
                    model.SettingsID = int.Parse(ds.Tables[0].Rows[0]["SettingsID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["MaxBetCount"].ToString() != "")
                {
                    model.MaxBetCount = int.Parse(ds.Tables[0].Rows[0]["MaxBetCount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["MaxOneBetMoney"].ToString() != "")
                {
                    model.MaxOneBetMoney = decimal.Parse(ds.Tables[0].Rows[0]["MaxOneBetMoney"].ToString());
                }
                if (ds.Tables[0].Rows[0]["MinOneBetMoney"].ToString() != "")
                {
                    model.MinOneBetMoney = decimal.Parse(ds.Tables[0].Rows[0]["MinOneBetMoney"].ToString());
                }

                if (ds.Tables[0].Rows[0]["MaxBonusMoney"].ToString() != "")
                {
                    model.MaxBonusMoney = decimal.Parse(ds.Tables[0].Rows[0]["MaxBonusMoney"].ToString());
                }
                if (ds.Tables[0].Rows[0]["MinMoneyIn"].ToString() != "")
                {
                    model.MinMoneyIn = decimal.Parse(ds.Tables[0].Rows[0]["MinMoneyIn"].ToString());
                }
                if (ds.Tables[0].Rows[0]["MaxUserRebatePro"].ToString() != "")
                {
                    model.MaxUserRebatePro = decimal.Parse(ds.Tables[0].Rows[0]["MaxUserRebatePro"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool SysUpdate(SLPolyGame.Web.Model.SysSettings model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SysSettings set ");
            strSql.Append("MaxBetCount=@MaxBetCount,");
            strSql.Append("MaxOneBetMoney=@MaxOneBetMoney,");
            strSql.Append("MinOneBetMoney=@MinOneBetMoney,");
            strSql.Append("MaxBonusMoney=@MaxBonusMoney,");
            //strSql.Append("MinMoneyOut=@MinMoneyOut,");
            //strSql.Append("MaxMoneyOutCount=@MaxMoneyOutCount,");
            //strSql.Append("MaxMoneyOut=@MaxMoneyOut,");
            strSql.Append("MinMoneyIn=@MinMoneyIn,");
            strSql.Append("MaxUserRebatePro=@MaxUserRebatePro");
            strSql.Append(" where SettingsID=@SettingsID");
            SqlParameter[] parameters = {
                    new SqlParameter("@SettingsID", SqlDbType.Int,4),
                    new SqlParameter("@MaxBetCount", SqlDbType.Int,4),
                    new SqlParameter("@MaxOneBetMoney", SqlDbType.Float,8),
                    new SqlParameter("@MinOneBetMoney", SqlDbType.Float,8),
                    new SqlParameter("@MaxBonusMoney", SqlDbType.Float,8),
                    //new SqlParameter("@MinMoneyOut", SqlDbType.Float,8),
                    //new SqlParameter("@MaxMoneyOutCount", SqlDbType.Int,4),
                    //new SqlParameter("@MaxMoneyOut", SqlDbType.Float,8),
					new SqlParameter("@MinMoneyIn", SqlDbType.Float,8),
                    new SqlParameter("@MaxUserRebatePro", SqlDbType.Float,8)};
            parameters[0].Value = model.SettingsID;
            parameters[1].Value = model.MaxBetCount;
            parameters[2].Value = model.MaxOneBetMoney;
            parameters[3].Value = model.MinOneBetMoney;
            parameters[4].Value = model.MaxBonusMoney;
            //parameters[5].Value = model.MinMoneyOut;
            //parameters[6].Value = model.MaxMoneyOutCount;
            //parameters[7].Value = model.MaxMoneyOut;
            parameters[5].Value = model.MinMoneyIn;
            parameters[6].Value = model.MaxUserRebatePro;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetCustomerServiceUrl()
        {
            string sql = "select CustomerService from dbo.SysSettings with(nolock)";
            DataSet ds = DbHelperSQL.Query(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0] != DBNull.Value)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
            }
            return string.Empty;
        }
    }
}