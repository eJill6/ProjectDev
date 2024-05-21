using System;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using SLPolyGame.Web.DBUtility;//Please add references
using System.Collections.Generic;
using System.Reflection;

namespace SLPolyGame.Web.DAL
{
    /// <summary>
    /// 数据访问类:LotteryInfo
    /// </summary>
    public class LotteryInfo
    {
        public LotteryInfo()
        { }

        #region Method

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperSQL.GetMaxID("LotteryID", "LotteryInfo");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int LotteryID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from LotteryInfo");
            strSql.Append(" where LotteryID=@LotteryID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@LotteryID", SqlDbType.Int,4)};
            parameters[0].Value = LotteryID;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(SLPolyGame.Web.Model.LotteryInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into LotteryInfo(");
            strSql.Append("LotteryType)");
            strSql.Append(" values (");
            strSql.Append("@LotteryType)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@LotteryType", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.LotteryType;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(SLPolyGame.Web.Model.LotteryInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update LotteryInfo set ");
            strSql.Append("LotteryType=@LotteryType");
            strSql.Append(" where LotteryID=@LotteryID");
            SqlParameter[] parameters = {
                    new SqlParameter("@LotteryID", SqlDbType.Int,4),
                    new SqlParameter("@LotteryType", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.LotteryID;
            parameters[1].Value = model.LotteryType;

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
        public bool Delete(int LotteryID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from LotteryInfo ");
            strSql.Append(" where LotteryID=@LotteryID");
            SqlParameter[] parameters = {
                    new SqlParameter("@LotteryID", SqlDbType.Int,4)
};
            parameters[0].Value = LotteryID;

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
        public bool DeleteList(string LotteryIDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from LotteryInfo ");
            strSql.Append(" where LotteryID in (" + LotteryIDlist + ")  ");
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

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public SLPolyGame.Web.Model.LotteryInfo GetModel(int LotteryID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 LotteryID,LotteryType from LotteryInfo ");
            strSql.Append(" where LotteryID=@LotteryID");
            SqlParameter[] parameters = {
                    new SqlParameter("@LotteryID", SqlDbType.Int,4)
};
            parameters[0].Value = LotteryID;

            SLPolyGame.Web.Model.LotteryInfo model = new SLPolyGame.Web.Model.LotteryInfo();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["LotteryID"].ToString() != "")
                {
                    model.LotteryID = int.Parse(ds.Tables[0].Rows[0]["LotteryID"].ToString());
                }
                model.LotteryType = ds.Tables[0].Rows[0]["LotteryType"].ToString();
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select LotteryID,LotteryType ");
            strSql.Append(" FROM LotteryInfo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" LotteryID,LotteryType ");
            strSql.Append(" FROM LotteryInfo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "LotteryInfo";
			parameters[1].Value = "";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

        #endregion Method

        /// <summary>
        /// 获取彩种列表
        /// </summary>
        public List<SLPolyGame.Web.Model.LotteryInfo> GetLotteryType()
        {
            string strSql = "select * from dbo.LotteryInfo with(nolock) where status = 1 ORDER BY priority asc";
            DataSet ds = DbHelperSQL_Bak.Query(strSql.ToString());
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ToList(ds.Tables[0]);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取彩种列表
        /// </summary>
        public List<SLPolyGame.Web.Model.LotteryInfo> Get_WebLotteryType(int GameTypeID)
        {
            string strSql = "select * from dbo.LotteryInfo with(nolock) where status in(0,1) and GameTypeID=@GameTypeID";
            SqlParameter[] parameters = {
                    new SqlParameter("@GameTypeID", SqlDbType.Int,4)};
            parameters[0].Value = GameTypeID;
            DataSet ds = DbHelperSQL_Bak.Query(strSql.ToString(), parameters);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ToList(ds.Tables[0]);
                    }
                }
            }
            return null;
        }

        public List<SLPolyGame.Web.Model.LotteryInfo> Get_WebEnableLotteryType(int GameTypeID)
        {
            string strSql = @"SELECT LotteryID, LotteryType, TypeURL, GameTypeID, HotNew, Notice
						FROM LotteryInfo WITH(NOLOCK) WHERE GameTypeID = @GameTypeID AND Status = 1
						ORDER BY Priority";
            SqlParameter[] parameters = {
                    new SqlParameter("@GameTypeID", SqlDbType.Int,4)};
            parameters[0].Value = GameTypeID;
            DataSet ds = DbHelperSQL_Bak.Query(strSql.ToString(), parameters);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ToList(ds.Tables[0]);
                    }
                }
            }
            return null;
        }

        public List<SLPolyGame.Web.Model.LotteryInfo> Pro_SearchLotteryInfo(int playModel)
        {
            //string strSql = @"select * from dbo.LotteryInfo with(nolock)
            //		where status = 1
            //		order by [GameTypeID], [Priority] asc";

            //DataSet ds = DbHelperSQL_Bak.Query(strSql.ToString());
            SqlParameter[] parameters = {
                    new SqlParameter("@PlayModel", SqlDbType.Int)
            };
            parameters[0].Value = playModel;

            DataSet ds = DbHelperSQL_Bak.RunProcedure("Pro_SearchLotteryInfo", parameters, "tab");
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ToList(ds.Tables[0]);
                    }
                }
            }
            return null;
        }

        /// <summary>
        ///  DataTable转List<model>
        /// </summary>
        public static List<SLPolyGame.Web.Model.LotteryInfo> ToList(DataTable dt)
        {
            // 定义集合
            List<SLPolyGame.Web.Model.LotteryInfo> ts = new List<SLPolyGame.Web.Model.LotteryInfo>();

            // 获得此模型的类型
            Type type = typeof(SLPolyGame.Web.Model.LotteryInfo);

            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                SLPolyGame.Web.Model.LotteryInfo t = new SLPolyGame.Web.Model.LotteryInfo();

                // 获得此模型的公共属性
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;

                    // 检查DataTable是否包含此列
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter
                        if (!pi.CanWrite) continue;

                        object value = dr[tempName];
                        if (value != DBNull.Value)
                        {
                            if (
                                (value.GetType() == typeof(Int64) && pi.PropertyType == typeof(string))
                                || (value.GetType() == typeof(Int32) && pi.PropertyType == typeof(string))
                                )
                                pi.SetValue(t, value.ToString(), null);
                            else
                                pi.SetValue(t, value, null);
                        }
                    }
                }

                ts.Add(t);
            }

            return ts;
        }
    }
}