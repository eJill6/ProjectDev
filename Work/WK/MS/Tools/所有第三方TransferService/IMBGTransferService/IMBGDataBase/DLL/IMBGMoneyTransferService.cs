using IMBGDataBase.DBUtility;
using IMBGDataBase.Enums;
using JxBackendService.Service.ThirdPartyTransfer.Old;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IMBGDataBase.DLL
{
    public class IMBGMoneyTransferService
    {
        private readonly int _minRecheckOrderMinutes = 5;
        private readonly int _maxRecheckOrderMinutes = 60 * 24 * 3;

        /// <summary>
        /// 获取 IMBG 转入数据
        /// </summary>
        public List<Model.IMBGMoneyInInfo> SearchMoneyIn()
        {
            DataSet ds = null;
            SqlParameter[] parameters = { };

            ds = DbHelperSQL.RunProcedure("Pro_SearchIMBGMoneyInInfo", parameters, "tab");


            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return DataTableToList<Model.IMBGMoneyInInfo>(ds.Tables[0]);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取 IMBG 转出数据
        /// </summary>
        public List<Model.IMBGMoneyOutInfo> SearchMoneyOut(int orderStatus)
        {
            SqlParameter[] parameters = { };
            DataSet ds = null;

            if (orderStatus == 0) //尚未處理
            {
                ds = DbHelperSQL.RunProcedure("Pro_SearchIMBGMoneyOutInfo", parameters, "tab");
            }
            else if (orderStatus == 1) //處理中
            {
                //status 9 為人工洗資料強迫重新處理的狀態, 通常是資料過期了, 需要額外洗bak的資料
                string sql = $@"
SELECT [MoneyOutID], [Amount], [OrderID], [OrderTime], [Handle], [HandTime]
, [UserID], [UserName], [Status], [Memo] 
FROM IMBGMoneyOutInfo WITH(NOLOCK) 
WHERE ([Status] = 1 
      AND DATEDIFF(MINUTE, OrderTime, GETDATE()) BETWEEN {_minRecheckOrderMinutes} AND {_maxRecheckOrderMinutes} )
      OR [Status] = 9 ";

                ds = DbHelperSQL.Query(sql, parameters);
            }


            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return DataTableToList<Model.IMBGMoneyOutInfo>(ds.Tables[0]);
                    }
                }
            }
            return null;
        }

        //        public bool UpdateOrderStatusFromManualToProcessing(ApiAction apiAction, string moneyId)
        //        {
        //            string tableName;
        //            string moneyIdColumnName;

        //            if (apiAction == ApiAction.Withdraw)
        //            {
        //                tableName = "IMBGMoneyOutInfo";
        //                moneyIdColumnName = "MoneyOutID";
        //            }
        //            else if (apiAction == ApiAction.Recharge)
        //            {
        //                tableName = "IMBGMoneyInInfo";
        //                moneyIdColumnName = "MoneyInID";
        //            }
        //            else
        //            {
        //                return false;
        //            }

        //            string sql = $@"
        //UPDATE {tableName} 
        //SET 
        //	[Status] = 1 
        //WHERE {moneyIdColumnName} = @id AND [Status] = 9 ";
        //            return DbHelperSQL.ExecuteSql(sql,
        //                new SqlParameter[]
        //                {
        //                    moneyId.ToSqlParameter("id", SqlDbType.VarChar, 32)
        //                }) > 0;
        //        }

        /// <summary>
        /// 转入转出成功时处理数据
        /// </summary>
        public bool IMBGTransferSuccess(string TransferID, string ActionType, int userId, decimal availableScores, decimal freezeScores)
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

            DataSet dt = DbHelperSQL.RunProcedure("Pro_IMBGTransferSuccess", parameters, "tab");

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
        public bool IMBGTransferRollback(string TransferID, string ActionType, string Msg, bool RollBack)
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

            DataSet dt = DbHelperSQL.RunProcedure("Pro_IMBGTransferRollback", parameters, "tab");

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
