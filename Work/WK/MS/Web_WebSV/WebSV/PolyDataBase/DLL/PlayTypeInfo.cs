using System;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using SLPolyGame.Web.DBUtility;
using System.Collections.Generic;
using System.Reflection;//Please add references
using PolyDataBase.Extensions;

namespace SLPolyGame.Web.DAL
{
    /// <summary>
    /// 数据访问类:PlayTypeInfo
    /// </summary>
    public class PlayTypeInfo
    {
        /// <summary>
        /// 获取各彩种选号方式列表
        /// </summary>
        /// <returns></returns>
        public List<SLPolyGame.Web.Model.PlayTypeInfo> GetPlayTypeInfo()
        {
            string strSql = "select a.* from dbo.PlayTypeInfo a with(nolock) left join dbo.LotteryInfo b with(nolock) on a.LotteryID=b.LotteryID where a.Status=1 order by a.priority asc ";
            DataSet ds = DbHelperSQL_Bak.Query(strSql.ToString());
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ds.Tables[0].ExtConvertToList<SLPolyGame.Web.Model.PlayTypeInfo>();
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 根据彩种ID获取各彩种选号方式列表
        /// </summary>
        /// <returns></returns>
        public List<SLPolyGame.Web.Model.PlayTypeInfo> GetPlayType(int LotteryId)
        {
            string strSql = "select * from dbo.PlayTypeInfo a with(nolock) left join dbo.LotteryInfo b with(nolock) on a.LotteryID=b.LotteryID where a.Status =1 and  a.LotteryID=@LotteryId order by a.priority asc ";
            SqlParameter[] parameters = {
                    new SqlParameter("@LotteryId", SqlDbType.Int,4)};
            parameters[0].Value = LotteryId;
            DataSet ds = DbHelperSQL_Bak.Query(strSql.ToString(), parameters);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ds.Tables[0].ExtConvertToList<SLPolyGame.Web.Model.PlayTypeInfo>();
                    }
                }
            }
            return null;
        }
    }
}