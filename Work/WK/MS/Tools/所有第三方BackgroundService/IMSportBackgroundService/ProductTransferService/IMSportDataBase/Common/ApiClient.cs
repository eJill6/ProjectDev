using IMSportDataBase.Model;
using JxBackendService.Common.Util;
using System.Net;
using System.Text;

namespace IMSportDataBase.Common
{
    public class ApiClient
    {
        /// <summary>
        /// 玩家下注日志 (IMSportsbook 体育博彩，IMSportsbook 电子竞技)
        /// </summary>
        public static string GetBetLog(IMSportApiParamModel model)
        {
            var url = new Uri(new Uri(model.ReportServiceUrl), model.GetBetLogPath);
            object obj = new
            {
                MerchantCode = model.MerchantCode,
                StartDate = model.StartTime.AddHours(-10).ToString("yyyy-MM-dd HH.mm.ss"),   //估計可能賽5小時
                EndDate = model.EndTime.ToString("yyyy-MM-dd HH.mm.ss"),
                Page = model.Page,
                ProductWallet = model.ProductWallet,
                DateFilterType = "2",
                BetStatus = "1",
                //LastUpdatedDate = model.StartTime.ToString("yyyy-MM-dd HH.mm.ss"),
                Language = "ZH-CN"
            };

            string retval = DoPostRequest(url, obj);

            return retval;
        }

        private static string DoPostRequest(Uri url, object obj)
        {
            string strResult = string.Empty;
            HttpWebRequest hwRequest = (System.Net.HttpWebRequest)WebRequest.Create(url);
            hwRequest.Timeout = 3 * 60 * 1000; // 3 分鐘
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