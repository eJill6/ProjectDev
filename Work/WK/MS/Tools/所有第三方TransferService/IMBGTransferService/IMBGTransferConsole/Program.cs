using IMBGDataBase.BLL;
using IMBGDataBase.Common;
using IMBGDataBase.DBUtility;
using IMBGDataBase.Enums;
using IMBGDataBase.Model;
using JxBackendService.Common.Util;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace IMBGTransferConsole
{
    public class Program
    {
        protected static string _environmentCode = ConfigurationManager.AppSettings["EnvironmentCode"].Trim();
        protected static string _serviceUrl = ConfigurationManager.AppSettings["ServiceUrl"].Trim();
        protected static string _merchantCode = ConfigurationManager.AppSettings["MerchantCode"].Trim();
        protected static string _language = ConfigurationManager.AppSettings["Language"].Trim();
        protected static string _linecode = ConfigurationManager.AppSettings["Linecode"].Trim();
        protected static string _md5Key = ConfigurationManager.AppSettings["MD5Key"].Trim();
        protected static string _desKey = ConfigurationManager.AppSettings["DesKey"].Trim();
        protected static int _perOnceQueryMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["PerOnceQueryMinutes"].Trim());

        static void Main(string[] args)
        {

        }

        private static void GetBetLog(string[] args)
        {
            DateTime begin, end;

            try
            {
                if (args.Count() == 2)
                {
                    begin = Convert.ToDateTime(args[0]);
                    end = Convert.ToDateTime(args[1]);
                }
                else
                {
                    begin = DateTime.Now.AddDays(-1);
                    end = DateTime.Now;
                }

                Console.WriteLine("Run DateTime:" + begin + "~" + end);

                var param = new IMBGApiParamModel
                {
                    MerchantCode = _merchantCode,
                    ServiceUrl = _serviceUrl,
                    StartTime = begin,
                    EndTime = end,
                    MD5Key = _md5Key,
                    DesKey = _desKey
                };

                var apiResult = ApiClient.GetBetLog(param);
                if (!string.IsNullOrWhiteSpace(apiResult))
                {
                    var result = apiResult.Deserialize<IMBGResp<IMBGBetList<IMBGBetLog>>>();

                    if (result != null &&
                        result.Data.Code == (int)APIErrorCode.Success &&
                        result.Data.List != null &&
                        result.Data.List.Count > 0)
                    {
                        foreach (var bet in result.Data.List)
                        {
                            if (bet.UserCode.Contains(_linecode + "_"))
                            {
                                var model = new IMBGApiParamModel
                                {
                                    MerchantCode = _merchantCode,
                                    ServiceUrl = _serviceUrl,
                                    PlayerId = bet.UserCode,
                                    MD5Key = _md5Key,
                                    DesKey = _desKey
                                };
                                IMBGResp<IMBGBalanceResp> balanceData = Transfer.GetBalance(model);

                                if (balanceData != null)
                                {
                                    var todb = SaveDB(bet, balanceData.Data.FreeMoneyStr);
                                    Console.WriteLine(bet.Id + ":" + todb);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Run End");
            Console.ReadKey();
        }

        private static string SaveDB(IMBGBetLog betinfo, string Balance)
        {
            var betTime = DateTimeUtility.Instance.ToLocalTime(betinfo.OpenTime).ToString("yyyy-MM-dd HH:mm:ss");
            var settleTime = DateTimeUtility.Instance.ToLocalTime(betinfo.EndTime).ToString("yyyy-MM-dd HH:mm:ss");

            StringBuilder memo = new StringBuilder();
            memo.Append("下注单号：" + betinfo.Id + ", ");
            memo.Append("下注：" + betinfo.TotalBet + ", 盈利：" + betinfo.WinLost + ", 下注时间：" + betTime + ", 结算时间：" + settleTime);

            decimal availableScores = Convert.ToDecimal(Balance);
            decimal freezeScores = availableScores - 0;

            SqlParameter[] parametersForInsert = {
                            new SqlParameter("@UserID",SqlDbType.Int,50),                   //0.Account
                            new SqlParameter("@BetTime", SqlDbType.DateTime),               //1.下注时间
                            new SqlParameter("@ProfitLossTime", SqlDbType.DateTime),        //2.开奖时间
                            new SqlParameter("@ProfitLossType", SqlDbType.NVarChar, 50),    //3.亏赢类型
                            new SqlParameter("@ProfitLossMoney", SqlDbType.Decimal, 18),    //4.有效下注额
                            new SqlParameter("@WinMoney", SqlDbType.Decimal, 18),           //5.亏赢
                            new SqlParameter("@PrizeMoney", SqlDbType.Decimal, 18),         //6.有效下注额 + 亏赢
                            new SqlParameter("@Memo", SqlDbType.NVarChar, 500),             //7.备注
                            new SqlParameter("@PalyID", SqlDbType.NVarChar, 50),            //8.订单编号
                            new SqlParameter("@GameType", SqlDbType.NVarChar, 50),          //9.游戏类型
                            new SqlParameter("@AvailableScores", SqlDbType.Decimal, 18),    //10.剩余金额
                            new SqlParameter("@FreezeScores", SqlDbType.Decimal, 18),       //11.可用金额
                        };

            parametersForInsert[0].Value = betinfo.UserCode.Replace(_linecode + "_", "");
            parametersForInsert[1].Value = betTime;
            parametersForInsert[2].Value = settleTime;
            parametersForInsert[3].Value = "亏盈";
            parametersForInsert[4].Value = betinfo.TotalBet;
            parametersForInsert[5].Value = betinfo.WinLost;
            parametersForInsert[6].Value = betinfo.TotalBet + betinfo.WinLost;
            parametersForInsert[7].Value = memo.ToString();
            parametersForInsert[8].Value = betinfo.Id;
            parametersForInsert[9].Value = null;
            parametersForInsert[10].Value = availableScores;
            parametersForInsert[11].Value = freezeScores;

            string result = string.Empty;
            try
            {
                DataSet ds = DbHelperSQL.RunProcedure("Pro_AddIMBGProfitLoss", parametersForInsert, "tab");
                result = ds.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

    }
}
