using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Maticsoft.DBUtility;
using System.Reflection;

namespace SportDataBase.DLL
{
    public class SportMoneyInInfo
    {
        /// <summary>
        /// 獲取沙巴體育轉入資訊
        /// </summary>
        public List<Model.SportMoneyInInfo> SearchMoneyIn()
        {

            SqlParameter[] parameters = { };
            DataSet ds = DbHelperSQL.RunProcedure("Pro_SearchSportMoneyInInfo", parameters, "tab");
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
        /// 转入转出失败时，回滚数据
        /// </summary>
        public bool SportTransferRollback(int TransferID, string ActionType, string Msg, bool RollBack)
        {

            SqlParameter[] parameters = {
                    new SqlParameter("@TransferID",SqlDbType.Int,4),
                    new SqlParameter("@ActionType",SqlDbType.NVarChar,50),
                    new SqlParameter("@Msg",SqlDbType.NVarChar,2000),
                    new SqlParameter("@RollBack",SqlDbType.Bit)
                                        };
            parameters[0].Value = TransferID;
            parameters[1].Value = ActionType;
            parameters[2].Value = Msg;
            parameters[3].Value = RollBack;
            DataSet dt = DbHelperSQL.RunProcedure("Pro_SportTransferRollback", parameters, "tab");

            string issur = dt.Tables[0].Rows[0][0].ToString();

            if (issur == "2")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<Model.SportMoneyInInfo> ToList(DataTable dt)
        {
            // 定义集合
            List<Model.SportMoneyInInfo> ts = new List<Model.SportMoneyInInfo>();

            // 获得此模型的类型
            Type type = typeof(Model.SportMoneyInInfo);

            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                Model.SportMoneyInInfo t = new Model.SportMoneyInInfo();

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
