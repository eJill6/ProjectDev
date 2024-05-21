using Microsoft.Data.SqlClient;
using SLPolyGame.Web.DBUtility;
using System.Data;
using System.Text;

namespace SLPolyGame.Web.DAL
{
    /// <summary>
    /// 数据访问类:UserInfo
    /// </summary>
    public class UserInfo
    {
        public UserInfo()
        {
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UserID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from UserInfo");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = UserID;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(SLPolyGame.Web.Model.UserInfo model)
        {
            SqlParameter[] parameters = {
                        new SqlParameter("@UserName", SqlDbType.NVarChar,50),
                        new SqlParameter("@UserId", SqlDbType.Int),
                        new SqlParameter("@RebatePro", SqlDbType.Decimal,9)};

            parameters[0].Value = model.UserName;
            parameters[1].Value = model.UserId;
            parameters[2].Value = model.RebatePro;
            DataSet dt = DbHelperSQL.RunProcedure("Pro_AddUser", parameters, "tab");

            if (dt.Tables == null)
            {
                return 1;
            }
            if (dt.Tables[0].Rows == null)
            {
                return 1;
            }
            if (dt.Tables[0].Columns == null)
            {
                return 1;
            }
            return int.Parse(dt.Tables[0].Rows[0][0].ToString());
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public SLPolyGame.Web.Model.UserInfo GetModel(int UserID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 UserID,UserName,RebatePro,AddedRebatePro from UserInfo WITH(NOLOCK) ");
            strSql.Append(" where UserID=@UserID");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4)
};
            parameters[0].Value = UserID;

            SLPolyGame.Web.Model.UserInfo model = new SLPolyGame.Web.Model.UserInfo();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["UserID"].ToString() != "")
                {
                    model.UserId = int.Parse(ds.Tables[0].Rows[0]["UserID"].ToString());
                }
                model.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                if (ds.Tables[0].Rows[0]["RebatePro"].ToString() != "")
                {
                    model.RebatePro = decimal.Parse(ds.Tables[0].Rows[0]["RebatePro"].ToString());
                }
                if (ds.Tables[0].Rows[0]["AddedRebatePro"].ToString() != "")
                {
                    model.AddedRebatePro = decimal.Parse(ds.Tables[0].Rows[0]["AddedRebatePro"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }
    }
}