using JxBackendService.Common;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.User;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameAGApiService : BaseTPGameApiService
    {
        private static readonly int _timeOutSeconds = 5;

        private static readonly string _apiSuccessCode = "0";

        private readonly IAGAppSetting _agAppSetting;

        private readonly Lazy<IDebugUserService> _debugUserService;

        public TPGameAGApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            var gameAppSettingService = DependencyUtil.ResolveKeyed<IGameAppSettingService>(envLoginUser.Application, SharedAppSettings.PlatformMerchant).Value;
            _agAppSetting = gameAppSettingService.GetAGAppSetting();
            _debugUserService = DependencyUtil.ResolveService<IDebugUserService>();
        }

        public override PlatformProduct Product => PlatformProduct.AG;

        public override BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            if (apiResult.IsNullOrEmpty())
            {
                return new BaseReturnModel("get order api result fail");
            }

            string info = Regex.Match(apiResult, "(?<=info=\")[\\s\\S]*?(?=\")", RegexOptions.IgnoreCase).Value;

            if (info != _apiSuccessCode)
            {
                string errorMsg = Regex.Match(apiResult, "(?<=msg=\")[\\s\\S]*?(?=\")", RegexOptions.IgnoreCase).Value;

                return new BaseReturnModel(errorMsg);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        protected override DetailRequestAndResponse GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            HttpClient client = CreateAGHttpClient();

            string queryString = string.Format("cagent={0}/\\\\/billno={1}/\\\\/method=qos/\\\\/actype={2}/\\\\/cur={3}",
                _agAppSetting.VendorID,
                CreateAGOrderID(tpGameMoneyInfo),
                _agAppSetting.AcType,
                _agAppSetting.Currency);

            var desTool = new DESTool(_agAppSetting.DesKey);
            string targetParam = desTool.DESEnCode(queryString);  //parma

            string key = CreateKey(targetParam);
            client.Url = string.Format(CreateTemplateUrl(), targetParam, key);

            _debugUserService.Value.ForcedDebug(tpGameMoneyInfo.UserID, new { queryString, client.Url }.ToJsonString());
            string apiResult = client.GetString();

            return CreateDetailRequestAndResponse(client.Url, apiResult);
        }

        public override DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, CreateRemoteAccountParam createRemoteAccountParam, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            //這邊比較特殊,根據原程式要打兩次,第一次prepare,第二次confirm, 故這邊就只有回傳第二次的api結果

            BaseReturnDataModel<DetailRequestAndResponse> prepareResult = PrepareTransferCredit(isMoneyIn, createRemoteAccountParam, tpGameMoneyInfo);

            if (!prepareResult.IsSuccess)
            {
                LogUtilService.Error(prepareResult.Message);
                string requestDetailJson = prepareResult.DataModel.ToJsonString();

                SendTelegramMessageToCustomerService(
                    new BasicUserInfo()
                    {
                        UserId = tpGameMoneyInfo.UserID,
                    },
                    string.Format(MessageElement.ProductTransferFailInfo,
                        Product.Name,
                        GetActionName(tpGameMoneyInfo is TPGameMoneyInInfo),
                        tpGameMoneyInfo.OrderID,
                        tpGameMoneyInfo.UserID,
                        prepareResult.Message,
                        requestDetailJson));

                return prepareResult.DataModel;
            }

            return GetTransferCreditConfirmApiResult(isMoneyIn, createRemoteAccountParam, tpGameMoneyInfo);
        }

        public override string GetRemoteUserScoreApiResult(CreateRemoteAccountParam createRemoteAccountParam, IInvocationUserParam invocationUserParam)
        {
            return BaseGetRemoteUserScoreApiResult(createRemoteAccountParam).DataModel;
        }

        public override BaseReturnDataModel<UserScore> GetTransferReturnModel(string apiResult)
        {
            BaseReturnModel baseReturnModel = ConvertTransferCreditConfirm(apiResult);

            if (!baseReturnModel.IsSuccess)
            {
                return new BaseReturnDataModel<UserScore>(baseReturnModel.Message);
            }

            return new BaseReturnDataModel<UserScore>(ReturnCode.Success);
        }

        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            string balanceResult = ParseXmlResult(apiResult, out string errorMsg);
            decimal? balance = balanceResult.ToDecimalNullable();

            if (!balance.HasValue)
            {
                return new BaseReturnDataModel<UserScore>(errorMsg);
            }

            return new BaseReturnDataModel<UserScore>(ReturnCode.Success, new UserScore() { AvailableScores = balance.Value });
        }

        protected override BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam createRemoteAccountParam)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteCheckAccountExistApiResult(createRemoteAccountParam);

            if (!returnModel.IsSuccess)
            {
                return new BaseReturnModel(returnModel.Message);
            }

            string parseResult = ParseXmlResult(returnModel.DataModel, out string errorMsg);
            decimal? balance = parseResult.ToDecimalNullable();

            if (balance.HasValue)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            BaseReturnDataModel<string> createAccountResult = GetRemoteCreateAccountApiResult(createRemoteAccountParam);

            if (!createAccountResult.IsSuccess)
            {
                return new BaseReturnModel(createAccountResult.Message);
            }

            parseResult = ParseXmlResult(createAccountResult.DataModel, out errorMsg);

            if (parseResult != _apiSuccessCode || !errorMsg.IsNullOrEmpty())
            {
                LogUtilService.Error("用户" + createRemoteAccountParam.TPGameAccount + "在AG平台创建账号失败，错误信息：" + errorMsg);

                return new BaseReturnModel("校验账号时系统异常");
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        protected override BaseReturnDataModel<string> GetRemoteForwardGameUrl(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            return GetRemoteLoginApiResult(tpGameRemoteLoginParam);
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            throw new NotImplementedException();
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(CreateRemoteAccountParam createRemoteAccountParam)
            => BaseGetRemoteUserScoreApiResult(createRemoteAccountParam);

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            HttpClient client = CreateAGHttpClient();

            /*创建或验证新用户-------------------------------------------------------------------------------------------------------------*/
            string queryString = string.Format("cagent={0}/\\\\/loginname={1}/\\\\/method=lg/\\\\/actype={2}/\\\\/password={3}/\\\\/oddtype={4}/\\\\/cur={5}",
                _agAppSetting.VendorID,
                createRemoteAccountParam.TPGameAccount,
                _agAppSetting.AcType,
                createRemoteAccountParam.TPGamePassword,
                _agAppSetting.OddsType,
                _agAppSetting.Currency);

            var desTool = new DESTool(_agAppSetting.DesKey);
            string targetParam = desTool.DESEnCode(queryString);  //parma
            string key = CreateKey(targetParam);
            client.Url = string.Format(CreateTemplateUrl(), targetParam, key);

            _debugUserService.Value.ForcedDebug(EnvLoginUser.LoginUser.UserId, new { queryString, client.Url }.ToJsonString());
            string result = client.GetString();

            return new BaseReturnDataModel<string>(ReturnCode.Success, result);
        }

        protected override string CreateTPGamePasswordByRule(int userId, string tpGameAccount) => HashExtension.MD5($"{userId}").Substring(0, 16);

        protected override BaseReturnDataModel<string> GetRemoteLoginApiResult(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            string gameType = GetThirdPartyRemoteCode(tpGameRemoteLoginParam.LoginInfo);
            string forwardUrl = _agAppSetting.LoginBaseUrl.TrimEnd("/") + "/forwardGame.do?params={0}&key={1}";//Gi域名

            string sid = _agAppSetting.VendorID + "1234567890987";
            string param = $@"cagent={_agAppSetting.VendorID}/\\\\/loginname={tpGameRemoteLoginParam.CreateRemoteAccountParam.TPGameAccount}/\\\\/actype={_agAppSetting.AcType}/\\\\/" +
                $@"password={tpGameRemoteLoginParam.CreateRemoteAccountParam.TPGamePassword}/\\\\/dm={_agAppSetting.DM}/\\\\/sid={sid}/\\\\/lang=1/\\\\/gameType={gameType}" +
                $@"/\\\\/oddtype={_agAppSetting.OddsType}/\\\\/cur={_agAppSetting.Currency}";

            var desTool = new DESTool(_agAppSetting.DesKey);
            string targetParam = desTool.DESEnCode(param);  //parma
            string key = CreateKey(targetParam);
            string url = string.Format(forwardUrl, targetParam, key);

            return new BaseReturnDataModel<string>(ReturnCode.Success, url);
        }

        protected override void DoKickUser(Model.Entity.ThirdPartyUserAccount thirdPartyUserAccount) => new NotImplementedException();

        private BaseReturnDataModel<string> BaseGetRemoteUserScoreApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            HttpClient client = CreateAGHttpClient();
            string tpGameAccount = createRemoteAccountParam.TPGameAccount;

            //get userid, username from tpGameAccount
            Dictionary<string, BaseBasicUserInfo> userMap = TPGameAccountReadService.GetUsersByTPGameAccounts(
                PlatformProduct.AG,
                new HashSet<string>() { tpGameAccount });

            BaseBasicUserInfo user = userMap[tpGameAccount];

            string param = string.Format("cagent={0}/\\\\/loginname={1}/\\\\/method=gb/\\\\/actype={2}/\\\\/password={3}/\\\\/cur={4}",
                _agAppSetting.VendorID,
                tpGameAccount,
                _agAppSetting.AcType,
                createRemoteAccountParam.TPGamePassword,
                _agAppSetting.Currency);

            var desTool = new DESTool(_agAppSetting.DesKey);
            string targetParam = desTool.DESEnCode(param);  //parma
            string key = CreateKey(targetParam);
            client.Url = string.Format(CreateTemplateUrl(), targetParam, key);

            _debugUserService.Value.ForcedDebug(user.UserId, new { param, client.Url }.ToJsonString());
            string apiResult = client.GetString();

            return new BaseReturnDataModel<string>(ReturnCode.Success, apiResult);
        }

        //private string CreateAGGamePwd(int userId)
        //    => HashExtension.MD5($"{userId}").Substring(0, 16);

        private HttpClient CreateAGHttpClient()
        {
            HttpClient client = new HttpClient
            {
                TimeOut = _timeOutSeconds,
                Verb = HttpVerb.GET,
                UserAgent = $"WEB_LIB_GI_{_agAppSetting.VendorID}",
                DefaultEncoding = Encoding.UTF8
            };

            return client;
        }

        private string CreateTemplateUrl() => _agAppSetting.ServiceBaseUrl.TrimEnd() + "/doBusiness.do?params={0}&key={1}";//Gi域名

        private string CreateKey(string targetParam) => HashExtension.MD5(targetParam + _agAppSetting.MD5Key).ToLower();

        private string CreateAGOrderID(BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            return _agAppSetting.VendorID + tpGameMoneyInfo.OrderID;
        }

        /// <summary>
        /// 预备转账
        /// </summary>
        protected virtual BaseReturnDataModel<DetailRequestAndResponse> PrepareTransferCredit(bool isMoneyIn,
            CreateRemoteAccountParam createRemoteAccountParam, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            /*
            /// 0 成功
            /// key_error Key 值错误 (参考3.3.1.2)
            /// duplicate_transfer 重复转账
            /// account_not_exist 游戏账号不存在
            /// network_error 网络问题导致资料遗失
            /// not_enough_credit 余额不足, 未能转账
            /// error 转账错误, 参考msg 的错误描述信息
            /// -1 网络异常 */

            HttpClient client = CreateAGHttpClient();

            string queryString = string.Format("cagent={0}/\\\\/method=tc/\\\\/loginname={1}/\\\\/billno={2}/\\\\/type={3}" +
                "/\\\\/credit={4}/\\\\/actype={5}/\\\\/password={6}/\\\\/cur={7}",
                _agAppSetting.VendorID,
                createRemoteAccountParam.TPGameAccount,
                CreateAGOrderID(tpGameMoneyInfo),
                ConvertToAGTransferType(isMoneyIn),
                tpGameMoneyInfo.Amount.Floor(2),
                _agAppSetting.AcType,
                createRemoteAccountParam.TPGamePassword,
                _agAppSetting.Currency);

            var desTool = new DESTool(_agAppSetting.DesKey);
            string targetParam = desTool.DESEnCode(queryString);  //parma

            string key = CreateKey(targetParam);
            client.Url = string.Format(CreateTemplateUrl(), targetParam, key);

            _debugUserService.Value.ForcedDebug(tpGameMoneyInfo.UserID, new { queryString, client.Url }.ToJsonString());
            string apiResult = client.GetString();

            if (apiResult.IsNullOrEmpty())
            {
                return new BaseReturnDataModel<DetailRequestAndResponse>("get api result fail", CreateDetailRequestAndResponse(client.Url, null));
            }

            DetailRequestAndResponse detail = CreateDetailRequestAndResponse(client.Url, apiResult);

            string info = Regex.Match(apiResult, "(?<=info=\")[\\s\\S]*?(?=\")", RegexOptions.IgnoreCase).Value;

            if (info != _apiSuccessCode)
            {
                string errorMsg = Regex.Match(apiResult, "(?<=msg=\")[\\s\\S]*?(?=\")", RegexOptions.IgnoreCase).Value;

                return new BaseReturnDataModel<DetailRequestAndResponse>(errorMsg, detail);
            }

            return new BaseReturnDataModel<DetailRequestAndResponse>(ReturnCode.Success, detail);
        }

        /// <summary>
        /// 确认转账Api Result
        /// </summary>
        protected virtual DetailRequestAndResponse GetTransferCreditConfirmApiResult(bool isMoneyIn,
            CreateRemoteAccountParam createRemoteAccountParam, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            HttpClient client = CreateAGHttpClient();

            string queryString = string.Format("cagent={0}/\\\\/method=tcc/\\\\/loginname={1}/\\\\/billno={2}/\\\\/type={3}" +
                "/\\\\/credit={4}/\\\\/actype={5}/\\\\/flag=1/\\\\/password={6}/\\\\/cur={7}",
                _agAppSetting.VendorID,
                createRemoteAccountParam.TPGameAccount,
                CreateAGOrderID(tpGameMoneyInfo),
                ConvertToAGTransferType(isMoneyIn),
                tpGameMoneyInfo.Amount.Floor(2),
                _agAppSetting.AcType,
                createRemoteAccountParam.TPGamePassword,
                _agAppSetting.Currency);

            var desTool = new DESTool(_agAppSetting.DesKey);
            string targetParam = desTool.DESEnCode(queryString);  //parma

            string key = CreateKey(targetParam);
            client.Url = string.Format(CreateTemplateUrl(), targetParam, key);

            _debugUserService.Value.ForcedDebug(tpGameMoneyInfo.UserID, new { queryString, client.Url }.ToJsonString());
            string apiResult = client.GetString();

            return CreateDetailRequestAndResponse(client.Url, apiResult);
        }

        /// <summary>
        /// 确认转账
        /// </summary>
        public BaseReturnModel ConvertTransferCreditConfirm(string apiResult)
        {
            if (apiResult.IsNullOrEmpty())
            {
                return new BaseReturnModel("get api result fail");
            }

            string info = Regex.Match(apiResult, "(?<=info=\")[\\s\\S]*?(?=\")", RegexOptions.IgnoreCase).Value;

            if (info != _apiSuccessCode)
            {
                string errorMsg = Regex.Match(apiResult, "(?<=msg=\")[\\s\\S]*?(?=\")", RegexOptions.IgnoreCase).Value;

                return new BaseReturnModel(errorMsg);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        private string GetActionName(bool isMoneyIn)
        {
            return TPGameMoneyTransferActionType.GetName(isMoneyIn);
        }

        private string ConvertToAGTransferType(bool isMoneyIn)
        {
            if (isMoneyIn)
            {
                return "IN";
            }
            else
            {
                return "OUT";
            }
        }

        private string ParseXmlResult(string result, out string msg)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            XmlNode node = xmlDoc.SelectSingleNode("result");
            string infoValue = node.Attributes["info"].Value;
            msg = node.Attributes["msg"].Value;

            return infoValue;
        }

        private DetailRequestAndResponse CreateDetailRequestAndResponse(string url, string apiResult)
        {
            return new DetailRequestAndResponse()
            {
                RequestUrl = url,
                ResponseContent = apiResult
            };
        }

        #region 沒有測試環境, 故整個class都從舊程式搬來

        private class HttpClient
        {
            #region fields

            private int timeOut = 30;

            private bool keepContext;

            private string defaultLanguage = "zh-CN";

            private Encoding defaultEncoding = Encoding.UTF8;

            private string accept = "*/*";

            private string userAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

            private HttpVerb verb = HttpVerb.GET;

            private HttpClientContext context;

            private readonly List<HttpUploadingFile> files = new List<HttpUploadingFile>();

            private readonly Dictionary<string, string> postingData = new Dictionary<string, string>();

            private string url;

            private WebHeaderCollection responseHeaders;

            private int startPoint;

            private int endPoint;

            #endregion fields

            #region events

            public event EventHandler<StatusUpdateEventArgs> StatusUpdate;

            private void OnStatusUpdate(StatusUpdateEventArgs e)
            {
                EventHandler<StatusUpdateEventArgs> temp = StatusUpdate;

                if (temp != null)
                    temp(this, e);
            }

            #endregion events

            #region properties

            /// <summary>
            /// 超时，秒
            /// </summary>
            public int TimeOut
            {
                get { return timeOut; }
                set { timeOut = value; }
            }

            /// <summary>
            /// 是否自动在不同的请求间保留Cookie, Referer
            /// </summary>
            public bool KeepContext
            {
                get { return keepContext; }
                set { keepContext = value; }
            }

            /// <summary>
            /// 期望的回应的语言
            /// </summary>
            public string DefaultLanguage
            {
                get { return defaultLanguage; }
                set { defaultLanguage = value; }
            }

            /// <summary>
            /// GetString()如果不能从HTTP头或Meta标签中获取编码信息,则使用此编码来获取字符串
            /// </summary>
            public Encoding DefaultEncoding
            {
                get { return defaultEncoding; }
                set { defaultEncoding = value; }
            }

            /// <summary>
            /// 指示发出Get请求还是Post请求
            /// </summary>
            public HttpVerb Verb
            {
                get { return verb; }
                set { verb = value; }
            }

            /// <summary>
            /// 要上传的文件.如果不为空则自动转为Post请求
            /// </summary>
            public List<HttpUploadingFile> Files
            {
                get { return files; }
            }

            /// <summary>
            /// 要发送的Form表单信息
            /// </summary>
            public Dictionary<string, string> PostingData
            {
                get { return postingData; }
            }

            /// <summary>
            /// 获取或设置请求资源的地址
            /// </summary>
            public string Url
            {
                get { return url; }
                set { url = value; }
            }

            /// <summary>
            /// 用于在获取回应后,暂时记录回应的HTTP头
            /// </summary>
            public WebHeaderCollection ResponseHeaders
            {
                get { return responseHeaders; }
            }

            /// <summary>
            /// 获取或设置期望的资源类型
            /// </summary>
            public string Accept
            {
                get { return accept; }
                set { accept = value; }
            }

            /// <summary>
            /// 获取或设置请求中的Http头User-Agent的值
            /// </summary>
            public string UserAgent
            {
                get { return userAgent; }
                set { userAgent = value; }
            }

            /// <summary>
            /// 获取或设置Cookie及Referer
            /// </summary>
            public HttpClientContext Context
            {
                get { return context; }
                set { context = value; }
            }

            /// <summary>
            /// 获取或设置获取内容的起始点,用于断点续传,多线程下载等
            /// </summary>
            public int StartPoint
            {
                get { return startPoint; }
                set { startPoint = value; }
            }

            /// <summary>
            /// 获取或设置获取内容的结束点,用于断点续传,多下程下载等.
            /// 如果为0,表示获取资源从StartPoint开始的剩余内容
            /// </summary>
            public int EndPoint
            {
                get { return endPoint; }
                set { endPoint = value; }
            }

            #endregion properties

            #region constructors

            /// <summary>
            /// 构造新的HttpClient实例
            /// </summary>
            public HttpClient()
                : this(null)
            {
            }

            /// <summary>
            /// 构造新的HttpClient实例
            /// </summary>
            /// <param name="url">要获取的资源的地址</param>
            public HttpClient(string url)
                : this(url, null)
            {
            }

            /// <summary>
            /// 构造新的HttpClient实例
            /// </summary>
            /// <param name="url">要获取的资源的地址</param>
            /// <param name="context">Cookie及Referer</param>
            public HttpClient(string url, HttpClientContext context)
                : this(url, context, false)
            {
            }

            /// <summary>
            /// 构造新的HttpClient实例
            /// </summary>
            /// <param name="url">要获取的资源的地址</param>
            /// <param name="context">Cookie及Referer</param>
            /// <param name="keepContext">是否自动在不同的请求间保留Cookie, Referer</param>
            public HttpClient(string url, HttpClientContext context, bool keepContext)
            {
                this.url = url;
                this.context = context;
                this.keepContext = keepContext;
                if (this.context == null)
                    this.context = new HttpClientContext();
            }

            #endregion constructors

            #region AttachFile

            /// <summary>
            /// 在请求中添加要上传的文件
            /// </summary>
            /// <param name="fileName">要上传的文件路径</param>
            /// <param name="fieldName">文件字段的名称(相当于&lt;input type=file name=fieldName&gt;)里的fieldName)</param>
            public void AttachFile(string fileName, string fieldName)
            {
                HttpUploadingFile file = new HttpUploadingFile(fileName, fieldName);
                files.Add(file);
            }

            /// <summary>
            /// 在请求中添加要上传的文件
            /// </summary>
            /// <param name="data">要上传的文件内容</param>
            /// <param name="fileName">文件名</param>
            /// <param name="fieldName">文件字段的名称(相当于&lt;input type=file name=fieldName&gt;)里的fieldName)</param>
            public void AttachFile(byte[] data, string fileName, string fieldName)
            {
                HttpUploadingFile file = new HttpUploadingFile(data, fileName, fieldName);
                files.Add(file);
            }

            #endregion AttachFile

            /// <summary>
            /// 清空PostingData, Files, StartPoint, EndPoint, ResponseHeaders, 并把Verb设置为Get.
            /// 在发出一个包含上述信息的请求后,必须调用此方法或手工设置相应属性以使下一次请求不会受到影响.
            /// </summary>
            public void Reset()
            {
                verb = HttpVerb.GET;
                files.Clear();
                postingData.Clear();
                responseHeaders = null;
                startPoint = 0;
                endPoint = 0;
            }

            private HttpWebRequest CreateRequest()
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.AllowAutoRedirect = false;
                req.CookieContainer = new CookieContainer();
                req.Headers.Add("Accept-Language", defaultLanguage);
                req.Accept = accept;
                req.UserAgent = userAgent;
                req.KeepAlive = false;
                req.Timeout = timeOut * 1000;

                if (context.Cookies != null)
                    req.CookieContainer.Add(context.Cookies);
                if (!string.IsNullOrEmpty(context.Referer))
                    req.Referer = context.Referer;

                if (verb == HttpVerb.HEAD)
                {
                    req.Method = "HEAD";
                    return req;
                }

                if (postingData.Count > 0 || files.Count > 0)
                    verb = HttpVerb.POST;

                if (verb == HttpVerb.POST)
                {
                    req.Method = "POST";

                    MemoryStream memoryStream = new MemoryStream();
                    StreamWriter writer = new StreamWriter(memoryStream);

                    if (files.Count > 0)
                    {
                        string newLine = "\r\n";
                        string boundary = Guid.NewGuid().ToString().Replace("-", "");
                        req.ContentType = "multipart/form-data; boundary=" + boundary;

                        foreach (string key in postingData.Keys)
                        {
                            writer.Write("--" + boundary + newLine);
                            writer.Write("Content-Disposition: form-data; name=\"{0}\"{1}{1}", key, newLine);
                            writer.Write(postingData[key] + newLine);
                        }

                        foreach (HttpUploadingFile file in files)
                        {
                            writer.Write("--" + boundary + newLine);
                            writer.Write(
                                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}",
                                file.FieldName,
                                file.FileName,
                                newLine
                                );
                            writer.Write("Content-Type: application/octet-stream" + newLine + newLine);
                            writer.Flush();
                            memoryStream.Write(file.Data, 0, file.Data.Length);
                            writer.Write(newLine);
                            writer.Write("--" + boundary + newLine);
                        }
                    }
                    else
                    {
                        req.ContentType = "application/x-www-form-urlencoded";
                        StringBuilder sb = new StringBuilder();
                        foreach (string key in postingData.Keys)
                        {
                            sb.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(postingData[key]));
                        }
                        if (sb.Length > 0)
                            sb.Length--;
                        writer.Write(sb.ToString());
                    }

                    writer.Flush();

                    using (Stream stream = req.GetRequestStream())
                    {
                        memoryStream.WriteTo(stream);
                    }
                }

                if (startPoint != 0 && endPoint != 0)
                    req.AddRange(startPoint, endPoint);
                else if (startPoint != 0 && endPoint == 0)
                    req.AddRange(startPoint);

                return req;
            }

            /// <summary>
            /// 发出一次新的请求,并返回获得的回应
            /// 调用此方法永远不会触发StatusUpdate事件.
            /// </summary>
            /// <returns>相应的HttpWebResponse</returns>
            public HttpWebResponse GetResponse()
            {
                HttpWebRequest req = CreateRequest();
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                responseHeaders = res.Headers;
                if (keepContext)
                {
                    context.Cookies = res.Cookies;
                    context.Referer = url;
                }
                return res;
            }

            /// <summary>
            /// 发出一次新的请求,并返回回应内容的流
            /// 调用此方法永远不会触发StatusUpdate事件.
            /// </summary>
            /// <returns>包含回应主体内容的流</returns>
            public Stream GetStream()
            {
                return GetResponse().GetResponseStream();
            }

            /// <summary>
            /// 发出一次新的请求,并以字节数组形式返回回应的内容
            /// 调用此方法会触发StatusUpdate事件
            /// </summary>
            /// <returns>包含回应主体内容的字节数组</returns>
            public byte[] GetBytes()
            {
                HttpWebResponse res = GetResponse();
                int length = (int)res.ContentLength;

                MemoryStream memoryStream = new MemoryStream();
                byte[] buffer = new byte[0x100];
                Stream rs = res.GetResponseStream();
                for (int i = rs.Read(buffer, 0, buffer.Length); i > 0; i = rs.Read(buffer, 0, buffer.Length))
                {
                    memoryStream.Write(buffer, 0, i);
                    OnStatusUpdate(new StatusUpdateEventArgs((int)memoryStream.Length, length));
                }
                rs.Close();

                return memoryStream.ToArray();
            }

            /// <summary>
            /// 发出一次新的请求,以Http头,或Html Meta标签,或DefaultEncoding指示的编码信息对回应主体解码
            /// 调用此方法会触发StatusUpdate事件
            /// </summary>
            /// <returns>解码后的字符串</returns>
            public string GetString()
            {
                byte[] data = GetBytes();
                string encodingName = GetEncodingFromHeaders();

                if (encodingName == null)
                    encodingName = GetEncodingFromBody(data);

                Encoding encoding;
                if (encodingName == null)
                {
                    encoding = defaultEncoding;
                }
                else
                {
                    try
                    {
                        encoding = Encoding.GetEncoding(encodingName);
                    }
                    catch (ArgumentException ex)
                    {
                        var logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
                        logUtilService.Value.ForcedDebug($"HttpClient.GetString() ArgumentException = {ex.ToString()}");
                        encoding = defaultEncoding;
                    }
                }

                return encoding.GetString(data);
            }

            /// <summary>
            /// 发出一次新的请求,对回应的主体内容以指定的编码进行解码
            /// 调用此方法会触发StatusUpdate事件
            /// </summary>
            /// <param name="encoding">指定的编码</param>
            /// <returns>解码后的字符串</returns>
            public string GetString(Encoding encoding)
            {
                byte[] data = GetBytes();
                return encoding.GetString(data);
            }

            private string GetEncodingFromHeaders()
            {
                string encoding = null;
                string contentType = responseHeaders["Content-Type"];
                if (contentType != null)
                {
                    int i = contentType.IndexOf("charset=");
                    if (i != -1)
                    {
                        encoding = contentType.Substring(i + 8);
                    }
                }
                return encoding;
            }

            private string GetEncodingFromBody(byte[] data)
            {
                string encodingName = null;
                string dataAsAscii = Encoding.ASCII.GetString(data);
                if (dataAsAscii != null)
                {
                    int i = dataAsAscii.IndexOf("charset=");
                    if (i != -1)
                    {
                        int j = dataAsAscii.IndexOf("\"", i);
                        if (j != -1)
                        {
                            int k = i + 8;
                            encodingName = dataAsAscii.Substring(k, (j - k) + 1);
                            char[] chArray = new char[2] { '>', '"' };
                            encodingName = encodingName.TrimEnd(chArray);
                        }
                    }
                }
                return encodingName;
            }

            /// <summary>
            /// 发出一次新的Head请求,获取资源的长度
            /// 此请求会忽略PostingData, Files, StartPoint, EndPoint, Verb
            /// </summary>
            /// <returns>返回的资源长度</returns>
            public int HeadContentLength()
            {
                Reset();
                HttpVerb lastVerb = verb;
                verb = HttpVerb.HEAD;
                using (HttpWebResponse res = GetResponse())
                {
                    verb = lastVerb;
                    return (int)res.ContentLength;
                }
            }

            /// <summary>
            /// 发出一次新的请求,把回应的主体内容保存到文件
            /// 调用此方法会触发StatusUpdate事件
            /// 如果指定的文件存在,它会被覆盖
            /// </summary>
            /// <param name="fileName">要保存的文件路径</param>
            public void SaveAsFile(string fileName)
            {
                SaveAsFile(fileName, FileExistsAction.Overwrite);
            }

            /// <summary>
            /// 发出一次新的请求,把回应的主体内容保存到文件
            /// 调用此方法会触发StatusUpdate事件
            /// </summary>
            /// <param name="fileName">要保存的文件路径</param>
            /// <param name="existsAction">指定的文件存在时的选项</param>
            /// <returns>是否向目标文件写入了数据</returns>
            public bool SaveAsFile(string fileName, FileExistsAction existsAction)
            {
                byte[] data = GetBytes();
                switch (existsAction)
                {
                    case FileExistsAction.Overwrite:
                        using (BinaryWriter writer = new BinaryWriter(new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write)))
                            writer.Write(data);
                        return true;

                    case FileExistsAction.Append:
                        using (BinaryWriter writer = new BinaryWriter(new FileStream(fileName, FileMode.Append, FileAccess.Write)))
                            writer.Write(data);
                        return true;

                    default:
                        if (!File.Exists(fileName))
                        {
                            using (
                                BinaryWriter writer =
                                    new BinaryWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write)))
                                writer.Write(data);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                }
            }
        }

        private class HttpClientContext
        {
            private CookieCollection cookies;

            private string referer;

            public CookieCollection Cookies
            {
                get { return cookies; }
                set { cookies = value; }
            }

            public string Referer
            {
                get { return referer; }
                set { referer = value; }
            }
        }

        private enum HttpVerb
        {
            GET,

            POST,

            HEAD,
        }

        private enum FileExistsAction
        {
            Overwrite,

            Append,

            Cancel,
        }

        private class HttpUploadingFile
        {
            private string fileName;

            private string fieldName;

            private byte[] data;

            public string FileName
            {
                get { return fileName; }
                set { fileName = value; }
            }

            public string FieldName
            {
                get { return fieldName; }
                set { fieldName = value; }
            }

            public byte[] Data
            {
                get { return data; }
                set { data = value; }
            }

            public HttpUploadingFile(string fileName, string fieldName)
            {
                this.fileName = fileName;
                this.fieldName = fieldName;
                using (FileStream stream = new FileStream(fileName, FileMode.Open))
                {
                    byte[] inBytes = new byte[stream.Length];
                    stream.Read(inBytes, 0, inBytes.Length);
                    data = inBytes;
                }
            }

            public HttpUploadingFile(byte[] data, string fileName, string fieldName)
            {
                this.data = data;
                this.fileName = fileName;
                this.fieldName = fieldName;
            }
        }

        private class StatusUpdateEventArgs : EventArgs
        {
            private readonly int bytesGot;

            private readonly int bytesTotal;

            public StatusUpdateEventArgs(int got, int total)
            {
                bytesGot = got;
                bytesTotal = total;
            }

            /// <summary>
            /// 已经下载的字节数
            /// </summary>
            public int BytesGot
            {
                get { return bytesGot; }
            }

            /// <summary>
            /// 资源的总字节数
            /// </summary>
            public int BytesTotal
            {
                get { return bytesTotal; }
            }
        }

        #endregion 沒有測試環境, 故整個class都從舊程式搬來
    }
}