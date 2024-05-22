using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using JxBackendService.Common.Util;
using LCDataBase.Enums;
using LCDataBase.Model;

namespace LCDataBase.Common
{
    public class ApiClient
    {
        public static string GetAPIResult(LCApiParamModel model)
        {
            string result = string.Empty;
            try
            {
                DateTime time = DateTime.UtcNow.AddHours(8);
                var timestamp = Utility.ConvertToUnixOfTime(time);
                string param = "";
                switch (model.ActType)
                {
                    case ApiAction.Login: //登录接口            
                        //var orderId = agent + time.ToString("yyyyMMddHHmmss") + account;
                        //param = "account=" + model.Account + "&money=" + model.Money + "&lineCode=" + lineCode + "&ip=" + ip + "&orderid=" + orderId + "&lang=zh-CN";
                        break;
                    case ApiAction.QueryBalance: //查询玩家可下分余额                        
                        param = "account=" + model.Account;
                        break;
                    case ApiAction.Recharge: //上分 
                        param = "account=" + model.Account + "&orderid=" + model.OrderID + "&money=" + model.Money;
                        break;
                    case ApiAction.Withdraw: //下分  
                        param = "account=" + model.Account + "&orderid=" + model.OrderID + "&money=" + model.Money;
                        break;
                    case ApiAction.OrderStatus: //查询订单状态                        
                        param = "orderid=" + model.OrderID;
                        break;
                    case ApiAction.IsOnline: //查询玩家是否在线                        
                        param = "account=" + model.Account;
                        break;
                    case ApiAction.PlayGameResult: //获取游戏结果数据                       
                        param = "startTime=" + model.StartTime + "&endTime=" + model.EndTime;
                        break;
                    case ApiAction.TotalBalance: //查询游戏总余额                        
                        param = "account=" + model.Account;
                        break;
                    case ApiAction.KickOut: //根据玩家账号踢玩家下线                        
                        param = "account=" + model.Account;
                        break;
                }

                if (!string.IsNullOrEmpty(param))
                {
                    param = "s=" + (int)model.ActType + "&" + param;

                    var desParam = EncryptTool.AESEncrypt(param, model.DESKey);
                    var desMd5 = EncryptTool.MD5Encrypt(model.AgentID + timestamp + model.MD5Key);

                    string url = model.ServiceUrl + "?agent=" + model.AgentID + "&timestamp=" + timestamp + "&param=" + desParam + "&key=" + desMd5;

                    int retryCount = 0;
                    while (retryCount < 5)
                    {
                        result = DoGetRequest(url);

                        if (!result.IsNullOrEmpty())
                        {
                            break;
                        }
                        retryCount++;
                        Thread.Sleep(5000);
                    }
                }
            }
            catch (Exception ex)
            {
                LogsManager.Info("GetAPIResult發生Exception：" + ex.ToString());
            }

            return result;
        }

        static string DoGetRequest(string url)
        {
            string strResult = string.Empty;

            SetConnectionLimit(120);

            if (url.ToLower().StartsWith("https"))
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

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

        static void SetConnectionLimit(int ConnectionLimit)
        {
            if (ServicePointManager.DefaultConnectionLimit < ConnectionLimit)
            {
                ServicePointManager.DefaultConnectionLimit = ConnectionLimit;
            }
        }

        #region json Serialzer

        public static string obj2json(object obj)
        {
            if (obj == null) return "null";
            var dcs = new DataContractJsonSerializer(obj.GetType());
            using (var stream = new MemoryStream())
            {
                dcs.WriteObject(stream, obj);
                stream.Position = 0;
                return System.Text.Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public static object json2obj(string json, Type type)
        {
            try
            {
                var dcs = new DataContractJsonSerializer(type);
                using (var stream = new MemoryStream())
                {
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Position = 0;
                    return dcs.ReadObject(stream);
                }
            }
            catch (Exception ex)
            {
                LogsManager.Info("反序列化失败：" + ex.Message + ",堆栈：" + ex.StackTrace);
                return null;
            }
        }

        #endregion
    }
}
