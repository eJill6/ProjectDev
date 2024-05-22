using IMeBetDataBase.Enums;
using IMeBetDataBase.Model;
using JxBackendService.Common.Util;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace IMeBetDataBase.Common
{
    public class ApiClient
    {
        /// <summary>
        /// 玩家下注日志 (IMeBet eBet真人博彩，IMeBet 电子竞技)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GetBetLog(IMeBetApiParamModel model)
        {
            var url = new Uri(new Uri(model.ReportServiceUrl), model.GetBetLogPath);
            object obj = new
            {
                MerchantCode = model.MerchantCode,
                StartDate = model.StartTime.ToString("yyyy-MM-dd HH.mm.ss"),   //估計可能賽5小時
                EndDate = model.EndTime.ToString("yyyy-MM-dd HH.mm.ss"),
                Page = model.Page,
                PageSize = 5000,
                ProductWallet = model.ProductWallet,
                BetStatus = "1",
                Language = "ZH-CN",
                Currency = "CNY",
            };

            string  retval = DoPostRequest(url, obj);

            return retval;
        }

        /// <summary>
        /// 资金转账
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string Transfer(IMeBetApiParamModel model)
        {
            var url = new Uri(new Uri(model.ServiceUrl), "Transaction/PerformTransfer");
            object obj = new
            {
                MerchantCode = model.MerchantCode,
                ProductWallet = model.ProductWallet,
                PlayerId = model.PlayerId,
                TransactionId = model.TransactionId,
                Amount = (model.ActType == ApiAction.Recharge) ? model.Amount : "-" + model.Amount
            };

            var retval = DoPostRequest(url, obj);
            return retval;
        }

        /// <summary>
        /// 查询转账交易状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string CheckTransferStatus(IMeBetApiParamModel model)
        {
            var url = new Uri(new Uri(model.ServiceUrl), "Transaction/CheckTransferStatus");
            object obj = new
            {
                MerchantCode = model.MerchantCode,
                ProductWallet = model.ProductWallet,
                PlayerId = model.PlayerId,
                TransactionId = model.TransactionId
            };

            var retval = DoPostRequest(url, obj);
            return retval;
        }

        /// <summary>
        /// 查询玩家余额
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GetBalance(IMeBetApiParamModel model)
        {
            var url = new Uri(new Uri(model.ServiceUrl), "Player/GetBalance");
            object obj = new
            {
                MerchantCode = model.MerchantCode,
                ProductWallet = model.ProductWallet,
                PlayerId = model.PlayerId
            };

            var retval = DoPostRequest(url, obj);
            return retval;
        }

        private static string DoPostRequest(Uri url, object obj)
        {
            string strResult = string.Empty;
            HttpWebRequest hwRequest = (System.Net.HttpWebRequest)WebRequest.Create(url);
            hwRequest.Timeout = 20 * 1000;
            hwRequest.Method = "POST";
            hwRequest.ContentType = "application/json";

            using (var streamWriter = new StreamWriter(hwRequest.GetRequestStream()))
            {
                string json = obj.ToJsonString();
                streamWriter.Write(json);
            }

            using (HttpWebResponse hwResponse = (HttpWebResponse)hwRequest.GetResponse())
            {
                using (StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.UTF8))
                {
                    strResult = srReader.ReadToEnd();
                    srReader.Close();
                }
                hwResponse.Close();
            }

            return strResult;
        }

        private static void SetConnectionLimit(int ConnectionLimit)
        {
            if (ServicePointManager.DefaultConnectionLimit < ConnectionLimit)
            {
                ServicePointManager.DefaultConnectionLimit = ConnectionLimit;
            }
        }
    }
}