using JxBackendService.Common;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty.JDB;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.JDB;
using JxBackendService.Model.ThirdParty.JDB.JDBFI;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameJDBFIApiService : BaseTPGameApiService
    {
        private static readonly int s_serverTimeZone = -4;

        /// <summary>
        /// 捕鱼机type
        /// </summary>
        private static readonly int s_gType = 7;

        private static readonly string s_jdbRequestDateTimeFormat = "dd-MM-yyyy HH:mm:00";

        private static readonly string s_jdbResponseDateTimeFormat = "dd-MM-yyyy HH:mm:ss";

        protected override bool IsAllowTransferCompensation => true;

        public override PlatformProduct Product => PlatformProduct.JDBFI;

        public static int ServerTimeZone => s_serverTimeZone;

        public static string JDBRequestDateTimeFormat => s_jdbRequestDateTimeFormat;

        public static string JDBResponseDateTimeFormat => s_jdbResponseDateTimeFormat;

        public TPGameJDBFIApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override string CreateTPGamePasswordByRule(int userId, string tpGameAccount) => null;

        /// <summary>
        /// 轉帳
        /// </summary>
        protected override DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, CreateRemoteAccountParam createRemoteAccountParam, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new JDBFITransferRequestModel
            {
                Ts = DateTimeUtil.ToUnixOfTime(DateTime.Now).ToString(),
                Parent = JDBFISharedAppSetting.Parent,
                Uid = createRemoteAccountParam.TPGameAccount,
                SerialNo = tpGameMoneyInfo.OrderID,
                Amount = isMoneyIn ? tpGameMoneyInfo.Amount : -tpGameMoneyInfo.Amount,//正数：存款 负数：提款
                Remark = ""
            };

            request.SetApiAction(JDBApiAction.Transfer);
            DoPostRequest(JDBFISharedAppSetting.ActionUrl, request.ToJsonString(isCamelCaseNaming: true), out DetailRequestAndResponse detail);

            return detail;
        }

        /// <summary>
        /// 查詢訂單
        /// </summary>
        protected override DetailRequestAndResponse GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new JDBFICheckTransferRequestModel
            {
                Ts = DateTimeUtil.ToUnixOfTime(DateTime.Now).ToString(),
                Parent = JDBFISharedAppSetting.Parent,
                SerialNo = tpGameMoneyInfo.OrderID
            };

            request.SetApiAction(JDBApiAction.QueryOrder);
            DoPostRequest(JDBFISharedAppSetting.ActionUrl, request.ToJsonString(isCamelCaseNaming: true), out DetailRequestAndResponse detail);

            return detail;
        }

        /// <summary>
        /// 查詢餘額
        /// </summary>
        protected override string GetRemoteUserScoreApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            var request = new JDBFIUserInfoRequestModel
            {
                Ts = DateTimeUtil.ToUnixOfTime(DateTime.Now).ToString(),
                Parent = JDBFISharedAppSetting.Parent,
                Uid = createRemoteAccountParam.TPGameAccount
            };

            request.SetApiAction(JDBApiAction.QueryUserScore);
            var returnModel = DoPostRequest(JDBFISharedAppSetting.ActionUrl, request.ToJsonString(isCamelCaseNaming: true), out DetailRequestAndResponse detail);

            return returnModel.DataModel;
        }

        /// <summary>
        /// 取得盈虧資料
        /// </summary>
        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            JDBFIGetBetLogRequestModel request = GetJDBFIBetLogRequest(lastSearchToken);
            BaseReturnDataModel<string> returnModel = DoPostRequest(JDBFISharedAppSetting.ActionUrl,
                request.ToJsonString(isCamelCaseNaming: true), out DetailRequestAndResponse detail);
            var returnDataModel = returnModel.CastByCodeAndMessage<RequestAndResponse>();

            returnDataModel.DataModel = new RequestAndResponse()
            {
                RequestBody = request.ToJsonString(),
                ResponseContent = returnModel.DataModel
            };

            return returnDataModel;
        }

        protected JDBFIGetBetLogRequestModel GetJDBFIBetLogRequest(string lastSearchToken)
        {
            DateTime searchStartDate;
            DateTime serverDate = GetServerDate();

            searchStartDate = DateTime.Parse(lastSearchToken);

            JDBApiAction apiAction = null;

            foreach (JDBApiAction action in JDBApiAction.GetAll().Where(w => w.MaxSearchMinutesAgo > 0).OrderBy(o => o.MinSearchMinutesAgo))
            {
                DateTime apiMinDate = GetFormatDate(serverDate.AddMinutes(-action.MaxSearchMinutesAgo));
                DateTime apiMaxDate = GetFormatDate(serverDate.AddMinutes(-action.MinSearchMinutesAgo));

                //重疊有一分鐘，所以優先用QueryBetLogRecently
                if (searchStartDate >= apiMinDate && searchStartDate <= apiMaxDate)
                {
                    apiAction = action;

                    break;
                }
            }

            if (apiAction == null)
            {
                apiAction = JDBApiAction.QueryBetLogHistory;
                const int overlapMinutes = 10;
                searchStartDate = serverDate.AddMinutes(-apiAction.MaxSearchMinutesAgo + overlapMinutes);//避免打api的時間差
            }

            DateTime searchEndDate = searchStartDate.AddMinutes(apiAction.SearchRangeMinutes);
            DateTime maxSearchEndDate = serverDate.AddMinutes(-apiAction.MinSearchMinutesAgo);

            if (searchEndDate > maxSearchEndDate)
            {
                searchEndDate = maxSearchEndDate;
            }

            var request = new JDBFIGetBetLogRequestModel
            {
                Ts = DateTimeUtil.ToUnixOfTime(DateTime.Now).ToString(),
                Parent = JDBFISharedAppSetting.Parent,
                Starttime = searchStartDate.ToString(s_jdbRequestDateTimeFormat),//JDB开始时间与结束时间中，ss（秒数）的值必须为 00。
                Endtime = searchEndDate.ToString(s_jdbRequestDateTimeFormat),
                GTypes = new int[] { s_gType }
            };

            request.SetApiAction(apiAction);

            return request;
        }

        /// <summary>
        /// 處理轉帳回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetTransferReturnModel(string apiResult)
        {
            JDBFITransferResponseModel transferModel = apiResult.Deserialize<JDBFITransferResponseModel>();

            if (transferModel.Status == JDBFIResponseCode.Success)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success, new UserScore() { AvailableScores = transferModel.UserBalance });
            }

            return new BaseReturnDataModel<UserScore>(transferModel.Err_text, null);
        }

        /// <summary>
        /// 處理查詢訂單回傳結果
        /// </summary>
        public override BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            JDBFIBaseResponseModel responseModel = apiResult.Deserialize<JDBFIBaseResponseModel>();

            if (responseModel.Status == JDBFIResponseCode.Success)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success);
            }
            else if (responseModel.Status == JDBFIResponseCode.DataDoesNotExist)
            {
                return new BaseReturnDataModel<UserScore>(responseModel.Err_text);
            }

            return null;
        }

        /// <summary>
        /// 處理取得餘額回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            JDBFIUserInfoResponseModel userScoreModel = apiResult.Deserialize<JDBFIUserInfoResponseModel>();

            if (userScoreModel.Status == JDBFIResponseCode.Success)
            {
                //單位 分
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success, new UserScore() { AvailableScores = userScoreModel.data[0].balance });
            }

            return new BaseReturnDataModel<UserScore>(userScoreModel.Err_text, null);
        }

        /// <summary>
        /// 檢查以及創建帳號
        /// </summary>
        protected override BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam createRemoteAccountParam)
        {
            //創帳號同啟動遊戲
            BaseReturnDataModel<string> returnModel = GetRemoteCreateAccountApiResult(createRemoteAccountParam);

            if (returnModel.IsSuccess)
            {
                JDBFIRegisterResponseModel registerModel = returnModel.DataModel.Deserialize<JDBFIRegisterResponseModel>();
                //檢查帳號重複同一隻API
                if (registerModel.Status == JDBFIResponseCode.Success ||
                    registerModel.Status == JDBFIResponseCode.AccountAlreadyExist)
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                return new BaseReturnModel(registerModel.Err_text);
            }

            return new BaseReturnModel(returnModel.Message);
        }

        /// <summary>
        /// 取得遊戲入口網址
        /// </summary>
        protected override BaseReturnDataModel<string> GetRemoteForwardGameUrl(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteLoginApiResult(tpGameRemoteLoginParam);

            if (!returnModel.IsSuccess)
            {
                return new BaseReturnDataModel<string>(returnModel.Message, string.Empty);
            }

            JDBFILaunchGameResponseModel launchGameModel = returnModel.DataModel.Deserialize<JDBFILaunchGameResponseModel>();

            if (launchGameModel.Status == JDBFIResponseCode.Success)
            {
                return new BaseReturnDataModel<string>(ReturnCode.Success, launchGameModel.Path);
            }

            return new BaseReturnDataModel<string>(launchGameModel.Err_text);
        }

        protected override BaseReturnDataModel<string> GetRemoteLoginApiResult(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            var request = new JDBFILaunchGameRequestModel
            {
                Ts = DateTimeUtil.ToUnixOfTime(DateTime.Now).ToString(),
                Uid = tpGameRemoteLoginParam.CreateRemoteAccountParam.TPGameAccount,
                IsAPP = tpGameRemoteLoginParam.IsMobile,
                GType = s_gType.ToString(),
                MType = tpGameRemoteLoginParam.LoginInfo.RemoteCode
            };

            request.SetApiAction(JDBApiAction.GetToken);
            BaseReturnDataModel<string> returnModel = DoPostRequest(JDBFISharedAppSetting.ActionUrl,
                request.ToJsonString(isCamelCaseNaming: true), out DetailRequestAndResponse detail);

            return returnModel;
        }

        /// <summary>
        /// 打第三方API，加密簽名方法
        /// </summary>
        private BaseReturnDataModel<string> DoPostRequest(string relativeUrl, string requestBody, out DetailRequestAndResponse detail)
        {
            string aesSign = AESTool.AesEncrypt(
                requestBody,
                JDBFISharedAppSetting.KEY,
                JDBFISharedAppSetting.IV,
                (result) => result.TrimEnd('=').Replace('+', '-').Replace('/', '_'));

            string fulllUrl = GenerateApiPath(GetFullUrl(JDBFISharedAppSetting.ServiceUrl, relativeUrl), JDBFISharedAppSetting.DC, aesSign);
            var httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>();
            var webRequestParam = new WebRequestParam()
            {
                Purpose = $"TPGService.{Product.Value}請求",
                Method = HttpMethod.Post,
                Url = fulllUrl,
                Body = requestBody,
                ContentType = HttpWebRequestContentType.Json,
                IsResponseValidJson = true
            };

            string apiResult = httpWebRequestUtilService.GetResponse(webRequestParam, out HttpStatusCode httpStatusCode);
            detail = new DetailRequestAndResponse(webRequestParam, apiResult);

            return ConverToApiReturnDataModel(httpStatusCode, apiResult);
        }

        private string GenerateApiPath(string host, string dc, string aesSign)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(host);
            stringBuilder.Append("?dc=").Append(dc);
            stringBuilder.Append("&x=").Append(aesSign);

            return stringBuilder.ToString();
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            //無此API
            throw new NotSupportedException();
        }

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            var request = new JDBFIRegisterRequestModel
            {
                Ts = DateTimeUtil.ToUnixOfTime(DateTime.Now).ToString(),
                Parent = JDBFISharedAppSetting.Parent,
                Uid = createRemoteAccountParam.TPGameAccount,
                Name = createRemoteAccountParam.TPGameAccount,
                Credit_allocated = ""
            };

            request.SetApiAction(JDBApiAction.CreateUser);

            return DoPostRequest(JDBFISharedAppSetting.ActionUrl, request.ToJsonString(isCamelCaseNaming: true), out DetailRequestAndResponse detail);
        }

        protected override bool IsDoTransferCompensation(string apiResult)
        {
            var transferModel = apiResult.Deserialize<JDBFITransferResponseModel>();

            if (transferModel.Status == JDBFIResponseCode.NoWithdrawalInGame)
            {
                return true;
            }

            return false;
        }

        private DateTime GetServerDate()
        {
            return DateTime.UtcNow.AddHours(s_serverTimeZone);
        }

        private DateTime GetFormatDate(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);//api不分秒數
        }
    }
}