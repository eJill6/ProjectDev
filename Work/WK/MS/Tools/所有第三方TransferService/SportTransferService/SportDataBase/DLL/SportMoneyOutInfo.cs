using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Maticsoft.DBUtility;
using System.Reflection;

namespace SportDataBase.DLL
{
   public class SportMoneyOutInfo
    {
        /// <summary>
        /// 获取Ag转出数据
        /// </summary>
       public List<Model.SportMoneyOutInfo> SearchMoneyOut()
        {
            SqlParameter[] parameters = { };
            DataSet ds = DbHelperSQL.RunProcedure("Pro_SearchSportMoneyOutInfo", parameters, "tab");
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
        /// 转入转出成功时处理数据
        /// </summary>
       public bool SportTransferSuccess(int TransferID, string ActionType, int userId, decimal availableScores, decimal freezeScores)
       {

           SqlParameter[] parameters = {
                    new SqlParameter("@TransferID",SqlDbType.Int,4),
                    new SqlParameter("@ActionType",SqlDbType.NVarChar,50),
                    new SqlParameter("@UserID",SqlDbType.Int,4),
                    new SqlParameter("@AvailableScores",SqlDbType.Decimal,18),
                    new SqlParameter("@FreezeScores",SqlDbType.Decimal,18)
                                        };
           parameters[0].Value = TransferID;
           parameters[1].Value = ActionType;
           parameters[2].Value = userId;
           parameters[3].Value = availableScores;
           parameters[4].Value = freezeScores;
           DataSet dt = DbHelperSQL.RunProcedure("Pro_SportTransferSuccess", parameters, "tab");

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

        public static List<Model.SportMoneyOutInfo> ToList(DataTable dt)
        {
            // 定义集合
            List<Model.SportMoneyOutInfo> ts = new List<Model.SportMoneyOutInfo>();

            // 获得此模型的类型
            Type type = typeof(Model.SportMoneyOutInfo);

            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                Model.SportMoneyOutInfo t = new Model.SportMoneyOutInfo();

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
