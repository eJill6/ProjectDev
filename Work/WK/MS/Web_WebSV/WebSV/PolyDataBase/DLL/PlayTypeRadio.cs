using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using SLPolyGame.Web.DBUtility;
using System.Reflection;
using System.Collections.Generic;//Please add references
using PolyDataBase.Extensions;

namespace SLPolyGame.Web.DAL
{
    /// <summary>
    /// 数据访问类:PlayTypeRadio
    /// </summary>
    public class PlayTypeRadio
    {
        /// <summary>
        /// 获取各彩种购买单选方式
        /// </summary>
        /// <returns></returns>
        public List<SLPolyGame.Web.Model.PlayTypeRadio> GetPlayTypeRadio()
        {
            string strSql = @"select c.* from dbo.PlayTypeRadio c with(nolock)
            left join dbo.PlayTypeInfo a with(nolock) on c.PlayTypeID=a.PlayTypeID
            left join dbo.LotteryInfo b with(nolock) on a.LotteryID=b.LotteryID where a.[status] = 1 and c.[Status] = 1 order by c.Priority";
            DataSet ds = DbHelperSQL_Bak.Query(strSql.ToString());
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ds.Tables[0].ExtConvertToList<SLPolyGame.Web.Model.PlayTypeRadio>();
                    }
                }
            }
            return null;
        }
    }
}