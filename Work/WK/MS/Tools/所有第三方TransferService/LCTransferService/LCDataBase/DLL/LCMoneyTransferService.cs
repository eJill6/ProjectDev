using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace LCDataBase.DLL
{
    public class LCMoneyTransferService
    {
        /// <summary>
        /// 获取 LC 转入数据
        /// </summary>
        public List<Model.LCMoneyInInfo> SearchMoneyIn()
        {

            SqlParameter[] parameters = { };
            DataSet ds = DbHelperSQL.RunProcedure("Pro_SearchLCMoneyInInfo", parameters, "tab");
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return MoneyInToList(ds.Tables[0]);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取Ag转出数据
        /// </summary>
        public List<Model.LCMoneyOutInfo> SearchMoneyOut()
        {
            SqlParameter[] parameters = { };
            DataSet ds = DbHelperSQL.RunProcedure("Pro_SearchLCMoneyOutInfo", parameters, "tab");
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return MoneyOutToList(ds.Tables[0]);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 转入转出成功时处理数据
        /// </summary>
        public bool LCTransferSuccess(string TransferID, string ActionType, int userId, decimal availableScores, decimal freezeScores)
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

            DataSet dt = DbHelperSQL.RunProcedure("Pro_LCTransferSuccess", parameters, "tab");

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
        public bool LCTransferRollback(string TransferID, string ActionType, string Msg, bool RollBack)
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

            DataSet dt = DbHelperSQL.RunProcedure("Pro_LCTransferRollback", parameters, "tab");

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

        public static List<Model.LCMoneyInInfo> MoneyInToList(DataTable dt)
        {
            // 定义集合
            List<Model.LCMoneyInInfo> ts = new List<Model.LCMoneyInInfo>();

            // 获得此模型的类型
            Type type = typeof(Model.LCMoneyInInfo);

            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                Model.LCMoneyInInfo t = new Model.LCMoneyInInfo();

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
                            if ((value.GetType() == typeof(Int64) && pi.PropertyType == typeof(string)) || 
                                (value.GetType() == typeof(Int32) && pi.PropertyType == typeof(string)))
                            {
                                pi.SetValue(t, value.ToString(), null);
                            }
                            else
                            {
                                pi.SetValue(t, value, null);
                            }
                        }
                    }
                }
                ts.Add(t);
            }
            return ts;
        }

        public static List<Model.LCMoneyOutInfo> MoneyOutToList(DataTable dt)
        {
            // 定义集合
            List<Model.LCMoneyOutInfo> ts = new List<Model.LCMoneyOutInfo>();

            // 获得此模型的类型
            Type type = typeof(Model.LCMoneyOutInfo);

            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                Model.LCMoneyOutInfo t = new Model.LCMoneyOutInfo();

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
                            if ((value.GetType() == typeof(Int64) && pi.PropertyType == typeof(string)) ||
                                (value.GetType() == typeof(Int32) && pi.PropertyType == typeof(string)))
                            {
                                pi.SetValue(t, value.ToString(), null);
                            }
                            else
                            {
                                pi.SetValue(t, value, null);
                            }
                        }
                    }
                }
                ts.Add(t);
            }
            return ts;

        }
    }
}
