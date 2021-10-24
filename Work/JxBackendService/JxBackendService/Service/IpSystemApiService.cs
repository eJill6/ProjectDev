using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using IPToolModel;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Exceptions;
using JxBackendService.Model.Param.Cache;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository;
using JxBackendService.Service.Base;
using Newtonsoft.Json;

namespace JxBackendService.Service
{
    public class IpSystemApiService : BaseService, IIpSystemApiService
    {
        private readonly string _ipSystemMid;
        private readonly string _ipSystemKey;
        private readonly string _ipSystemUrl;

        private static readonly int _ipSystemInfoSuspendSeconds = 60 * 60 * 24; //24小時
        private static readonly int _cacheSeconds = 60 * 5; //5分鐘
        private static readonly int _maxExceptionTryCount = 5;
        private static readonly int _maxNormalTryCount = 150;
        private static readonly int _minSuccessCountsToStartErrorHandle = 3;//成功幾次之後開啟error handle
        private static int _currentSuccessCountsToStartErrorHandle = 0;

        public IpSystemApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _ipSystemMid = SharedAppSettings.IpSystemMid;
            _ipSystemKey = SharedAppSettings.IpSystemKey;
            _ipSystemUrl = SharedAppSettings.IpSystemUrl;
        }

        public IpSystemApiResult Query(string queryIP)
        {
            string invokeName = null;

            for (int i = 1; i <= 5; i++)
            {
                StackTrace stackTrace = new StackTrace();
                MethodBase methodBase = stackTrace.GetFrame(i).GetMethod();

                if (methodBase.Name != nameof(Query))
                {
                    invokeName = methodBase.Name;
                    break;
                }
            }

            bool isDoErrorHandle = false;

            if (_currentSuccessCountsToStartErrorHandle >= _minSuccessCountsToStartErrorHandle)
            {
                isDoErrorHandle = true;
            }

            var intervalJobParam = new IntervalJobParam()
            {
                CacheKey = CacheKey.IPSystemInfoSuspend,
                CacheSeconds = _cacheSeconds,
                SuspendSeconds = _ipSystemInfoSuspendSeconds,
                IsSuspendWhenException = true,
                MaxExceptionTryCount = _maxExceptionTryCount,
                IsDoErrorHandle = isDoErrorHandle,
                MaxNormalTryCount = _maxNormalTryCount,
                EnvironmentUser = EnvLoginUser
            };

            try
            {
                return DoIntervalWork(intervalJobParam, () =>
                {
                    var dic = new Dictionary<string, string>
                        {
                        { "mid", _ipSystemMid},
                        { "ip", queryIP},
                        { "timestamp", GetTimestamp().ToString()}
                        };

                    string keyValue = DictionaryToKeyValue(dic.OrderBy(a => a.Key).ToDictionary(b => b.Key, c => c.Value));
                    string hash = MD5Sign(keyValue, _ipSystemKey);
                    keyValue += "sign=" + hash;

                    string apiResult = null;

                    try
                    {
                        apiResult = DoGetRequest(_ipSystemUrl + "?" + keyValue, queryIP);
                    }
                    catch (Exception ex)
                    {
                        _currentSuccessCountsToStartErrorHandle = 0;
                        throw ex;
                    }

                    if (!string.IsNullOrEmpty(apiResult))
                    {
                        LogUtil.Info($"查詢IP：{queryIP}，回传讯息：{apiResult}");
                        var ipInfo = JsonConvert.DeserializeObject<IpSystemApiResult>(apiResult);
                        _currentSuccessCountsToStartErrorHandle++;
                        return ipInfo;
                    }

                    return null;
                }, () =>
                {
                    if (isDoErrorHandle)
                    {
                        ErrorMsgUtil.ErrorHandle(new JobSuspendException(invokeName), EnvLoginUser);
                    }
                });
            }
            catch (Exception)
            {
                //這邊不處理,因為當isDoErrorHandle==false時,會拋出內部拋出ex
                return null;
            }
        }

        public long GetTimestamp()
        {
            var gtm = new DateTime(1970, 1, 1);
            var utc = DateTime.UtcNow;
            var timeStamp = Convert.ToInt64(utc.Subtract(gtm).TotalMilliseconds);
            return timeStamp;
        }

        private string MD5Sign(string data, string key)
        {
            data += "key=" + key;
            var md5 = MD5.Create();
            byte[] source = Encoding.Default.GetBytes(data);//將字串轉為Byte[]
            byte[] crypto = md5.ComputeHash(source);//進行MD5加密
            var strB = new StringBuilder();

            for (int i = 0; i < crypto.Length; i++)
            {
                strB.Append(crypto[i].ToString("x2"));
            }
            string hexString = strB.ToString();
            return hexString;
        }

        private string DictionaryToKeyValue(Dictionary<string, string> data)
        {
            string str = string.Empty;
            foreach (var dic in data)
            {
                str += dic.Key + "=" + dic.Value + "&";
            }
            return str;
        }

        public string CompositeToString(IpSystemApiResult ipSystemApiResult)
        {
            var type = ipSystemApiResult.GetType();
            var props = type.GetProperties().Select(a => a.GetValue(ipSystemApiResult, null).ToString())
                            .Where(a => !string.IsNullOrWhiteSpace(a)).ToArray();
            var str = string.Join("-", props);
            return str;
        }

        public string GetArea(JxIpInformation ipInfo)
        {
            IpSystemApiResult ipSystemApiResult = Query(ipInfo.DestinationIP);

            if (ipSystemApiResult == null)
            {
                return null;
            }

            string area = CompositeToString(ipSystemApiResult);
            return area;
        }

        private string DoGetRequest(string url, string queryIP)
        {
            string strResult = string.Empty;
            HttpWebRequest hwRequest = (System.Net.HttpWebRequest)WebRequest.Create(url);
            hwRequest.Timeout = 3 * 1000;
            hwRequest.Method = "GET";

            using (HttpWebResponse hwResponse = (HttpWebResponse)hwRequest.GetResponse())
            {
                using (StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.UTF8))
                {
                    strResult = srReader.ReadToEnd();
                    srReader.Close();
                }
                if (hwResponse.StatusCode != HttpStatusCode.OK)
                {
                    LogUtil.Info($"查詢IP：{queryIP}，详细信息：{strResult}");
                    strResult = string.Empty;
                }
                hwResponse.Close();
            }
            return strResult;
        }
    }
}
