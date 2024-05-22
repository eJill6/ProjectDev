using IMDataBase.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IMDataBase.DLL
{
    public class IMMoneyTransferService
    {
        /// <summary>
        /// 获取 IM 转入数据
        /// </summary>
        public List<Model.IMMoneyInInfo> SearchMoneyIn()
        {

            SqlParameter[] parameters = { };
            DataSet ds = DbHelperSQL.RunProcedure("Pro_SearchIMMoneyInInfo", parameters, "tab");
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return DataTableToList<Model.IMMoneyInInfo>(ds.Tables[0]);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取Ag转出数据
        /// </summary>
        public List<Model.IMMoneyOutInfo> SearchMoneyOut()
        {
            SqlParameter[] parameters = { };
            DataSet ds = DbHelperSQL.RunProcedure("Pro_SearchIMMoneyOutInfo", parameters, "tab");
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return DataTableToList<Model.IMMoneyOutInfo>(ds.Tables[0]);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 转入转出成功时处理数据
        /// </summary>
        public bool IMTransferSuccess(string TransferID, string ActionType, int userId, decimal availableScores, decimal freezeScores)
        {

            SqlParameter[] parameters = {
                new SqlParameter("@TransferID",SqlDbType.VarChar, 32),
                new SqlParameter("@ActionType",SqlDbType.NVarChar, 50),
                new SqlParameter("@UserID",SqlDbType.Int, 4),
                new SqlParameter("@AvailableScores",SqlDbType.Decimal, 18),
                new SqlParameter("@FreezeScores",SqlDbType.Decimal, 18)
           };

            parameters[0].Value = TransferID;
            parameters[1].Value = ActionType;
            parameters[2].Value = userId;
            parameters[3].Value = availableScores;
            parameters[4].Value = freezeScores;

            DataSet dt = DbHelperSQL.RunProcedure("Pro_IMTransferSuccess", parameters, "tab");

            string issur = string.Empty;

            if (dt.Tables.Count > 0)
            {
                if (dt.Tables[0].Rows.Count > 0)
                {
                    issur = (string)dt.Tables[0].Rows[0][0];
                }
            }

            return issur.Equals("2");
        }

        /// <summary>
        /// 转入转出失败时，回滚数据
        /// </summary>
        public bool IMTransferRollback(string TransferID, string ActionType, string Msg, bool RollBack)
        {

            SqlParameter[] parameters = {
                    new SqlParameter("@TransferID",SqlDbType.VarChar, 32),
                    new SqlParameter("@ActionType",SqlDbType.NVarChar, 50),
                    new SqlParameter("@Msg",SqlDbType.NVarChar, 2000),
                    new SqlParameter("@RollBack", SqlDbType.Bit)};

            parameters[0].Value = TransferID;
            parameters[1].Value = ActionType;
            parameters[2].Value = Msg;
            parameters[3].Value = RollBack;

            DataSet dt = DbHelperSQL.RunProcedure("Pro_IMTransferRollback", parameters, "tab");

            string issur = string.Empty;

            if (dt.Tables.Count > 0)
            {
                if (dt.Tables[0].Rows.Count > 0)
                {
                    issur = (string)dt.Tables[0].Rows[0][0];
                }
            }

            return issur.Equals("2");
        }

        /// <summary> 
        /// 利用反射將DataTable轉換為List<T>物件
        /// </summary> 
        /// <param name="dt">DataTable 物件</param> 
        /// <returns>List<T>集合</returns> 
        public static List<T> DataTableToList<T>(DataTable dt) where T : class, new()
        {
            List<T> ts = new List<T>();
            string tempName = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;
                    if (dt.Columns.Contains(tempName))
                    {
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                        {
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
