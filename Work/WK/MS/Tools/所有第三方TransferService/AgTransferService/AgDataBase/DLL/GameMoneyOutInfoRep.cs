using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace AgDataBase.DLL
{
    public class GameMoneyOutInfoRep : BaseGameMoneyInfoRep
    {
        /// <summary>
        /// 获取Ag转出数据
        /// </summary>
        public List<Model.GameMoneyOutInfo> SearchMoneyOut()
        {
            SqlParameter[] parameters = { };
            DataSet ds = DbHelperSQL.RunProcedure("Pro_SearchAgMoneyOutInfo", parameters, "tab");
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
        public bool AgTransferSuccess(int TransferID, string ActionType, int userId, decimal availableScores)
        {

            SqlParameter[] parameters = {
                    new SqlParameter("@TransferID",SqlDbType.Int,4),
                    new SqlParameter("@ActionType",SqlDbType.NVarChar,50),
                    new SqlParameter("@UserID",SqlDbType.Int,4),
                    new SqlParameter("@AvailableScores",SqlDbType.Decimal,18)
                                        };
            parameters[0].Value = TransferID;
            parameters[1].Value = ActionType;
            parameters[2].Value = userId;
            parameters[3].Value = availableScores;
            DataSet dt = DbHelperSQL.RunProcedure("Pro_AgTransferSuccess", parameters, "tab");
            if (dt.Tables != null)
            {
                if (dt.Tables[0].Rows != null)
                {
                    string issur = (string)dt.Tables[0].Rows[0][0];
                    if (issur == "2")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static List<Model.GameMoneyOutInfo> ToList(DataTable dt)
        {
            // 定义集合
            List<Model.GameMoneyOutInfo> ts = new List<Model.GameMoneyOutInfo>();

            // 获得此模型的类型
            Type type = typeof(Model.GameMoneyOutInfo);

            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                Model.GameMoneyOutInfo t = new Model.GameMoneyOutInfo();

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