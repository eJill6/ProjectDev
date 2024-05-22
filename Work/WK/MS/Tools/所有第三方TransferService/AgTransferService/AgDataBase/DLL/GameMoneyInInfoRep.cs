using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Maticsoft.DBUtility;
using System.Reflection;
using AgDataBase.Model;

namespace AgDataBase.DLL
{
   public class GameMoneyInInfoRep : BaseGameMoneyInfoRep
    {
       /// <summary>
       /// 获取Ag转入数据
       /// </summary>
       public List<Model.GameMoneyInInfo> SearchMoneyIn()
       {

           SqlParameter[] parameters = { };
           DataSet ds = DbHelperSQL.RunProcedure("Pro_SearchAgMoneyInInfo", parameters, "tab");
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
       public bool AgTransferRollback(int TransferID, string ActionType, string Msg,bool RollBack)
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
           DataSet dt = DbHelperSQL.RunProcedure("Pro_AgTransferRollback", parameters, "tab");

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

       public static List<Model.GameMoneyInInfo> ToList(DataTable dt)
       {
           // 定义集合
           List<Model.GameMoneyInInfo> ts = new List<Model.GameMoneyInInfo>();

           // 获得此模型的类型
           Type type = typeof(Model.GameMoneyInInfo);

           string tempName = "";

           foreach (DataRow dr in dt.Rows)
           {
               Model.GameMoneyInInfo t = new Model.GameMoneyInInfo();

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

        public bool UpdateMoneyInOrderStatusFromManualToProcessing(int moneyInId)
        {
            string sql = $@"UPDATE inlodb.dbo.GameMoneyInInfo
                                SET [Status] = 1
                            WHERE MoneyInID = @moneyId 
                                  AND [Status] = 9 ";

            return DbHelperSQL.ExecuteSql(sql, new SqlParameter("moneyId", moneyInId)) > 0;
        }

        public List<GameMoneyInInfo> GetTPGameProcessingMoneyInInfo()
        {
            string sql = SearchTPGameProcessingMoneyInfo("MoneyInID", "GameMoneyInInfo");
            return DbHelperSQL.QueryList<GameMoneyInInfo>(sql, new { ProcessingStatus = 1, ManualStatus = 9 });
        }
    }
}
