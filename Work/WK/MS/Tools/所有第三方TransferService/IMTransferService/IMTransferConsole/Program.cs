using IMDataBase.BLL;
using IMDataBase.DBUtility;
using IMDataBase.Model;
using JxBackendService.Common.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace IMTransferConsole
{
    public class Program
    {
        protected static string _serviceUrl = ConfigurationManager.AppSettings["ServiceUrl"].Trim();
        protected static string _merchantCode = ConfigurationManager.AppSettings["MerchantCode"].Trim();
        protected static string _currency = ConfigurationManager.AppSettings["Currency"].Trim();
        protected static string _language = ConfigurationManager.AppSettings["Language"].Trim();
        protected static string _linecode = ConfigurationManager.AppSettings["Linecode"].Trim();
        protected static string _productWallet = ConfigurationManager.AppSettings["ProductWallet"].Trim();
        protected static int _perOnceQueryMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["PerOnceQueryMinutes"].Trim());

        static void Main(string[] args)
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

                var param = new IMApiParamModel
                {
                    MerchantCode = _merchantCode,
                    ServiceUrl = _serviceUrl,
                    StartTime = begin,
                    EndTime = end,
                    ProductWallet = _productWallet,
                    Page = 1
                };

                var apiResult = IMDataBase.Common.ApiClient.GetBetLog(param);

                var result = apiResult.Deserialize<BetLogResult<List<BetResult>>>();

                if (result.Result != null){
                    foreach (var bet in result.Result)
                    {
                        if (bet.PlayerId.Contains(_linecode + "_")
                            && (bet.IsSettled != 0 || bet.IsCancelled != 0))
                        {
                            var model = new IMApiParamModel
                            {
                                MerchantCode = _merchantCode,
                                ServiceUrl = _serviceUrl,
                                ProductWallet = _productWallet,
                                PlayerId = bet.PlayerId
                            };
                            var balanceData = Transfer.GetBalance(model);
                            var todb = SaveDB(bet, balanceData.Balance);
                            Console.WriteLine(bet.BetId + ":" + todb);
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

        private static string SaveDB(BetResult betinfo , string Balance)
        {

            StringBuilder memo = new StringBuilder();
            memo.Append("下注单号：" + betinfo.BetId + ", ");
            foreach (var Items in betinfo.DetailItems)
            {
                memo.Append("赛事：" + Items.CompetitionName + ", ");
                memo.Append("赔率：" + Items.Odds + ", ");
            }
            memo.Append("下注：" + betinfo.StakeAmount + ", 盈利：" + betinfo.WinLoss + ", 下注时间：" + betinfo.WagerCreationDateTime + ", 结算时间：" + betinfo.SettlementDateTime);

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
                            //new SqlParameter("@AllBetMoney", SqlDbType.Decimal, 18)         //12.總投注額
                        };

            parametersForInsert[0].Value = betinfo.PlayerId.Replace(_linecode + "_", "");
            parametersForInsert[1].Value = betinfo.WagerCreationDateTime;
            parametersForInsert[2].Value = betinfo.SettlementDateTime;
            parametersForInsert[3].Value = "亏盈";
            parametersForInsert[4].Value = betinfo.StakeAmount;
            parametersForInsert[5].Value = betinfo.WinLoss;
            parametersForInsert[6].Value = decimal.Parse(betinfo.StakeAmount) + decimal.Parse(betinfo.WinLoss);
            parametersForInsert[7].Value = memo.ToString();
            parametersForInsert[8].Value = betinfo.BetId;
            parametersForInsert[9].Value = "401";
            parametersForInsert[10].Value = availableScores;
            parametersForInsert[11].Value = freezeScores;
            //parametersForInsert[12].Value = dr["StakeAmount"];

            string result = string.Empty;
            try
            {
                DataSet ds = DbHelperSQL.RunProcedure("Pro_AddIMProfitLoss", parametersForInsert, "tab");
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
