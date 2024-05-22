using IMBGDataBase.Enums;
using IMBGDataBase.Model;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace IMBGDataBase.Common
{
    public class ApiClient
    {
        /// <summary>
        /// 玩家下注日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GetBetLog(IMBGApiParamModel model)
        {
            var betLogType = 9;
            var startTime = DateTimeUtility.Instance.ConvertToTimestamp(model.StartTime).ToString();
            var endTime = DateTimeUtility.Instance.ConvertToTimestamp(model.EndTime).ToString();
            var param = HttpUtility.UrlEncode(DESEncrypt($"ac={betLogType}&all=0&startTime={startTime}&endTime={endTime}", model.DesKey));


            return DoGetRequest(model.ServiceUrl, param, model.MerchantCode, model.MD5Key, "logHandle");
        }

        /// <summary>
        /// 资金转账
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string Transfer(IMBGApiParamModel model)
        {
            string result = "";
            var param = string.Empty;
            try
            {
                if (model.ActType == ApiAction.Recharge)    //上分
                {
                    var transferInType = 3;
                    param = HttpUtility.UrlEncode(DESEncrypt($"ac={transferInType}&userCode={model.PlayerId}&money={model.Amount}&orderId={model.TransactionId}", model.DesKey));
                }
                if (model.ActType == ApiAction.Withdraw)    //下分
                {
                    var transferOutType = 4;
                    param = HttpUtility.UrlEncode(DESEncrypt($"ac={transferOutType}&userCode={model.PlayerId}&money={model.Amount}&orderId={model.TransactionId}", model.DesKey));
                }
                result = DoGetRequest(model.ServiceUrl, param, model.MerchantCode, model.MD5Key);
            }
            catch (Exception ex)
            {
                LogsManager.InfoToTelegram("IMBG Transfer 异常，信息：" + ex.Message + ",堆栈:" + ex.StackTrace);
                result = string.Empty;
            }

            return result;
        }

        /// <summary>
        /// 查询转账交易状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string CheckTransferStatus(IMBGApiParamModel model)
        {
            var transferStatusType = 5;
            var param = HttpUtility.UrlEncode(DESEncrypt($"ac={transferStatusType}&userCode={model.PlayerId}&orderId={model.TransactionId}", model.DesKey));


            return DoGetRequest(model.ServiceUrl, param, model.MerchantCode, model.MD5Key);
        }

        /// <summary>
        /// 查询玩家余额
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GetBalance(IMBGApiParamModel model)
        {
            var balanceType = 2;
            var param = HttpUtility.UrlEncode(DESEncrypt($"ac={balanceType}&userCode={model.PlayerId}", model.DesKey));


            return DoGetRequest(model.ServiceUrl, param, model.MerchantCode, model.MD5Key);
        }

        static string DoGetRequest(string apiUrl, string param, string merchantCode, string md5Key, string apiHandle = "agentHandle")
        {
            var timestampMS = DateTimeUtility.Instance.GetTimestampMs().ToString();
            var sign = MD5Encrypt($"{merchantCode}{timestampMS}{md5Key}");
            var url = $"{apiUrl}{apiHandle}?agentId={merchantCode}&timestamp={timestampMS}&param={param}&sign={sign}";

            string strResult = string.Empty;
            HttpWebRequest hwRequest = (System.Net.HttpWebRequest)WebRequest.Create(url);
            hwRequest.Timeout = 20 * 1000;
            hwRequest.Method = "GET";
            hwRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

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

        public static string MD5Encrypt(string str)
        {
            var ue = new System.Text.UTF8Encoding();
            var bytes = ue.GetBytes(str);

            // encrypt bytes
            var md5 = new MD5CryptoServiceProvider();
            var hashBytes = md5.ComputeHash(bytes);

            // Convert the encrypted bytes back to a string (base 16)
            var encrypt32Byte = hashBytes.Aggregate("", (current, t) => current + Convert.ToString(t, 16).PadLeft(2, '0'));


            return encrypt32Byte.PadLeft(32, '0').ToLower();
        }

        public static string DESEncrypt(string str, string key)
        {
            return IMBGEncrypt.DESEncrypt(str, key);
        }

        static void SetConnectionLimit(int ConnectionLimit)
        {
            if (ServicePointManager.DefaultConnectionLimit < ConnectionLimit)
            {
                ServicePointManager.DefaultConnectionLimit = ConnectionLimit;
            }
        }

    }
}
