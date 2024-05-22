using AgDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendServiceNF.Common.Util;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Security;
using System.Xml;

namespace AgDataBase.Common
{
    public interface IAgApi
    {
        decimal GetBalance(string loginName, string loginId, bool isDelayRequest = false);

        string GetDateTimeRemotePath();

        string GetDateTimeRemotePath(int addDays);

        ResultModel PrepareTransferCredit(AGTransferMoneyApiParamModel apiParam, ref string apiResult);

        ResultModel QueryOrderStatus(AGQueryOrderApiParamModel apiParam);

        ResultModel TransferCreditConfirm(AGTransferMoneyApiParamModel apiParam);
    }

    public class AgApi : IAgApi
    {
        /// <summary>
        /// 获取用户余额，返回结果为-9999表示调用失败
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginId"></param>
        /// <param name="isDelayRequest">是否需延遲呼叫個人餘額</param>
        /// <returns></returns>
        public decimal GetBalance(string loginName, string loginId, bool isDelayRequest = false)
        {
            string password = FormsAuthentication.HashPasswordForStoringInConfigFile(loginId.ToString() + loginName, "MD5").Substring(0, 16);
            string userAgent = "WEB_LIB_GI_" + AGConstParams.cagent;  //请求消息头
            string url = AGConstParams.url + "/doBusiness.do?params={0}&key={1}";//Gi域名
            string param = string.Format("cagent={0}/\\\\/loginname={1}/\\\\/method=gb/\\\\/actype={2}/\\\\/password={3}/\\\\/cur={4}",
                AGConstParams.cagent, loginName, AGConstParams.actype.ToString(), password, AGConstParams.cur);

            var desTool = new DESUtility(AGConstParams.desKey);
            string targetParam = desTool.DESEnCode(param);  //parma

            string key = FormsAuthentication.HashPasswordForStoringInConfigFile(targetParam + AGConstParams.md5Key, "MD5").ToLower();
            url = string.Format(url, targetParam, key);

            HttpClient client = new HttpClient
            {
                TimeOut = 8,
                Verb = HttpVerb.GET,
                UserAgent = userAgent,
                DefaultEncoding = Encoding.UTF8,
                Url = url
            };

            try
            {
                string msg = string.Empty;

                if (isDelayRequest)
                {
                    Thread.Sleep(2 * 1000);
                }

                LogUtil.ForcedDebug(new { param, client.Url }.ToJsonString());
                string result = client.GetString();

                var info = ParseResult(result, out msg);
                decimal balance;
                bool b = decimal.TryParse(info, out balance);

                if (b)
                {
                    return balance;
                }
                else
                {
                    LogsManager.Info("用户" + loginName + "在AG平台获取余额失败，错误信息：" + msg);

                    return -9999;
                }
            }
            catch (Exception ex)
            {
                LogsManager.Info("用户" + loginName + "在AG平台获取余额失败，错误信息：" + ex.Message + "，堆栈信息：" + ex.StackTrace);

                return -9999;
            }
        }

        private string ParseResult(string result, out string msg)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            var node = xmlDoc.SelectSingleNode("result");
            string v = node.Attributes["info"].Value;
            msg = node.Attributes["msg"].Value;
            return v;
        }

        /// <summary>
        /// 预备转账
        /// </summary>
        /// 0 成功
        /// key_error Key 值错误 (参考3.3.1.2)
        /// duplicate_transfer 重复转账
        /// account_not_exist 游戏账号不存在
        /// network_error 网络问题导致资料遗失
        /// not_enough_credit 余额不足, 未能转账
        /// error 转账错误, 参考msg 的错误描述信息
        /// -1 网络异常
        public ResultModel PrepareTransferCredit(AGTransferMoneyApiParamModel apiParam, ref string apiResult)
        {
            string userAgent = "WEB_LIB_GI_" + apiParam.Cagent;  //请求消息头
            HttpClient client = new HttpClient();
            client.TimeOut = 30;
            client.Verb = HttpVerb.GET;
            client.UserAgent = userAgent;
            client.DefaultEncoding = Encoding.UTF8;

            string param = string.Format("cagent={0}/\\\\/method=tc/\\\\/loginname={1}/\\\\/billno={2}/\\\\/type={3}/\\\\/credit={4}/\\\\/actype={5}/\\\\/password={6}/\\\\/cur={7}"
                , apiParam.Cagent,
                apiParam.LoginName,
                apiParam.Billno,
                apiParam.Type,
                apiParam.Credit,
                apiParam.Actype,
                apiParam.Password,
                apiParam.Cur);

            DESUtility desTool = new DESUtility(apiParam.DesKey);
            string targetParam = desTool.DESEnCode(param);  //parma

            string key = FormsAuthentication.HashPasswordForStoringInConfigFile(targetParam + apiParam.Md5Key, "MD5").ToLower();   //key
            client.Url = string.Format(apiParam.Url, targetParam, key);

            LogUtil.ForcedDebug(new { param, client.Url }.ToJsonString());
            apiResult = client.GetString();

            if (!string.IsNullOrWhiteSpace(apiResult))
            {
                string info = Regex.Match(apiResult, "(?<=info=\")[\\s\\S]*?(?=\")", RegexOptions.IgnoreCase).Value;
                var model = new ResultModel();

                if (info == "0")
                {
                    model.IsSuccess = true;
                    model.Info = info;
                    model.Msg = string.Empty;
                }
                else
                {
                    string msg = Regex.Match(apiResult, "(?<=msg=\")[\\s\\S]*?(?=\")", RegexOptions.IgnoreCase).Value;
                    model.IsSuccess = false;
                    model.Info = info;
                    model.Msg = msg;
                }

                return model;
            }

            return null;
        }

        /// <summary>
        /// 确认转账
        /// </summary>
        public ResultModel TransferCreditConfirm(AGTransferMoneyApiParamModel apiParam)
        {
            string userAgent = "WEB_LIB_GI_" + apiParam.Cagent;  //请求消息头
            HttpClient client = new HttpClient();
            client.TimeOut = 30;
            client.Verb = HttpVerb.GET;
            client.UserAgent = userAgent;
            client.DefaultEncoding = Encoding.UTF8;

            string param = string.Format("cagent={0}/\\\\/method=tcc/\\\\/loginname={1}/\\\\/billno={2}/\\\\/type={3}/\\\\/credit={4}/\\\\/actype={5}/\\\\/flag=1/\\\\/password={6}/\\\\/cur={7}"
                , apiParam.Cagent,
                apiParam.LoginName,
                apiParam.Billno,
                apiParam.Type,
                apiParam.Credit,
                apiParam.Actype,
                apiParam.Password,
                apiParam.Cur);

            DESUtility desTool = new DESUtility(apiParam.DesKey);
            string targetParam = desTool.DESEnCode(param);  //parma

            string key = FormsAuthentication.HashPasswordForStoringInConfigFile(targetParam + apiParam.Md5Key, "MD5").ToLower();   //key
            client.Url = string.Format(apiParam.Url, targetParam, key);

            LogUtil.ForcedDebug(new { param, client.Url }.ToJsonString());
            string result = client.GetString();

            if (!string.IsNullOrWhiteSpace(result))
            {
                var model = new ResultModel();
                string info = Regex.Match(result, "(?<=info=\")[\\s\\S]*?(?=\")", RegexOptions.IgnoreCase).Value;

                if (info == "0")
                {
                    model.IsSuccess = true;
                    model.Msg = string.Empty;
                    model.Info = info;
                }
                else
                {
                    string msg = Regex.Match(result, "(?<=msg=\")[\\s\\S]*?(?=\")", RegexOptions.IgnoreCase).Value;
                    model.IsSuccess = false;
                    model.Msg = msg;
                    model.Info = info;
                }

                return model;
            }

            return null;
        }

        /// <summary>
        /// 订单查询
        /// </summary>
        public ResultModel QueryOrderStatus(AGQueryOrderApiParamModel apiParam)
        {
            string userAgent = "WEB_LIB_GI_" + apiParam.Cagent;  //请求消息头
            HttpClient client = new HttpClient();
            client.TimeOut = 30;
            client.Verb = HttpVerb.GET;
            client.UserAgent = userAgent;
            client.DefaultEncoding = Encoding.UTF8;

            string param = string.Format("cagent={0}/\\\\/billno={1}/\\\\/method=qos/\\\\/actype={2}/\\\\/cur={3}",
                apiParam.Cagent,
                apiParam.Billno,
                apiParam.Actype,
                apiParam.Cur);

            DESUtility desTool = new DESUtility(apiParam.DesKey);
            string targetParam = desTool.DESEnCode(param);  //parma

            string key = FormsAuthentication.HashPasswordForStoringInConfigFile(targetParam + apiParam.Md5Key, "MD5").ToLower();   //key
            client.Url = string.Format(apiParam.Url, targetParam, key);

            LogUtil.ForcedDebug(new { param, client.Url }.ToJsonString());
            string result = client.GetString();

            if (!string.IsNullOrWhiteSpace(result))
            {
                var model = new ResultModel();
                string info = Regex.Match(result, "(?<=info=\")[\\s\\S]*?(?=\")", RegexOptions.IgnoreCase).Value;

                if (info == "0")
                {
                    model.IsSuccess = true;
                    model.Info = info;
                    model.Msg = string.Empty;
                }
                else
                {
                    string msg = Regex.Match(result, "(?<=msg=\")[\\s\\S]*?(?=\")", RegexOptions.IgnoreCase).Value;
                    model.IsSuccess = false;
                    model.Msg = msg;
                    model.Info = info;
                }

                return model;
            }

            return null;
        }

        public string GetDateTimeRemotePath() => GetDateTimeRemotePath(0);

        public string GetDateTimeRemotePath(int addDays)
        {
            // 美東時間
            DateTime easternStandardTime = DateTime.Now.AddHours(-12).AddMinutes(-30);

            if (addDays != 0)
            {
                easternStandardTime = easternStandardTime.AddDays(addDays);
            }

            return easternStandardTime.ToString("yyyyMMdd");
        }
    }

    public class AgMockApi : IAgApi
    {
        public decimal GetBalance(string loginName, string loginId, bool isDelayRequest = false)
        {
            return 100;
        }

        public ResultModel PrepareTransferCredit(AGTransferMoneyApiParamModel apiParam, ref string apiResult)
        {
            return new ResultModel()
            {
                Info = "0",
                IsSuccess = true,
                Msg = string.Empty
            };
        }

        public ResultModel QueryOrderStatus(AGQueryOrderApiParamModel apiParam)
        {
            return new ResultModel()
            {
                Info = "0",
                IsSuccess = true,
                Msg = string.Empty
            };

            //return new ResultModel()
            //{
            //    Info = "1",
            //    IsSuccess = false,
            //    Msg = "network error"
            //};
        }

        public ResultModel TransferCreditConfirm(AGTransferMoneyApiParamModel apiParam)
        {
            return new ResultModel()
            {
                Info = "0",
                IsSuccess = true,
                Msg = string.Empty
            };
        }

        public string GetDateTimeRemotePath() => "20230104";

        public string GetDateTimeRemotePath(int addDays) => DateTime.Parse("2023-01-04").AddDays(addDays).ToString("yyyyMMdd");
    }
}