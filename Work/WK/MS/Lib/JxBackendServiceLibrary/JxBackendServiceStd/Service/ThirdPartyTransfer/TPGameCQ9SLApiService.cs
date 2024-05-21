using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.CQ9SL;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameCQ9SLApiService : BaseTPGameApiService
    {
        private readonly CQ9SLSharedAppSetting _appSetting;

        private readonly Dictionary<string, string> _headers;

        public override PlatformProduct Product => PlatformProduct.CQ9SL;

        public TPGameCQ9SLApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _appSetting = CQ9SLSharedAppSetting.Instance;

            _headers = new Dictionary<string, string>()
            {
                { _appSetting.AuthorizationHeaderName, _appSetting.Authorization}
            };
        }

        protected override string CreateTPGamePasswordByRule(int userId, string tpGameAccount) => tpGameAccount;

        /// <summary>
        /// 轉帳
        /// </summary>
        public override DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, CreateRemoteAccountParam createRemoteAccountParam,
            BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var nameValueCollection = new NameValueCollection()
            {
                { _appSetting.QueryStringKey.Account, createRemoteAccountParam.TPGameAccount },
                { _appSetting.QueryStringKey.Mtcode, tpGameMoneyInfo.OrderID },
                { _appSetting.QueryStringKey.Amount, tpGameMoneyInfo.Amount.ToString() },
            };

            string requestPath;

            if (isMoneyIn)
            {
                requestPath = _appSetting.PlayerDepositPath;
            }
            else
            {
                requestPath = _appSetting.PlayerWithdrawPath;
            }

            string requestBody = nameValueCollection.ToQueryString();

            DoApiRequest(HttpMethod.Post, requestPath, requestBody, out DetailRequestAndResponse detail);

            return detail;
        }

        /// <summary>
        /// 查詢訂單
        /// </summary>
        protected override DetailRequestAndResponse GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            string mtcode = tpGameMoneyInfo.OrderID;
            string requestPath = _appSetting.GetQueryTransactionRecordPath(mtcode);

            DoApiRequest(HttpMethod.Get, requestPath, null, out DetailRequestAndResponse detail);

            return detail;
        }

        /// <summary>
        /// 查詢餘額
        /// </summary>
        public override string GetRemoteUserScoreApiResult(CreateRemoteAccountParam createRemoteAccountParam, IInvocationUserParam invocationUserParam)
        {
            string tpGameAccount = createRemoteAccountParam.TPGameAccount;
            string requestPath = _appSetting.GetPlayerBalancePath(tpGameAccount);

            BaseReturnDataModel<string> apiReturnModel = DoApiRequest(HttpMethod.Get, requestPath, null, out DetailRequestAndResponse detail);

            return apiReturnModel.DataModel;
        }

        /// <summary>
        /// 取得盈虧資料
        /// </summary>
        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            DateTimeOffset searchStartDate = DateTimeOffset.UtcNow;
            DateTimeOffset maxSearchEndDate = DateTimeOffset.UtcNow;

            if (!lastSearchToken.IsNullOrEmpty())
            {
                searchStartDate = new DateTimeOffset(lastSearchToken.ToInt64().ToDateTime()).UtcDateTime;
            }

            DateTimeOffset searchEndDate = searchStartDate.AddMinutes(_appSetting.MaxSearchRangeMinutes);

            if (searchEndDate > maxSearchEndDate)
            {
                searchEndDate = maxSearchEndDate;
            }

            if (searchEndDate < searchStartDate)
            {
                searchEndDate = searchStartDate;
            }

            var pagerInfo = new PagerInfo()
            {
                PageNo = 1,
                PageSize = _appSetting.QueryBetLogPageSize,
                TotalCount = 0,
            };

            CQ9Response<CQ9BetLogViewModel> cq9Response = null;
            var requestAndResponse = new RequestAndResponse();

            while (true)
            {
                var nameValueCollection = new NameValueCollection()
                {
                    { _appSetting.QueryStringKey.StartTime, ToApiDateTimeFormatString(searchStartDate) },
                    { _appSetting.QueryStringKey.EndTime, ToApiDateTimeFormatString(searchEndDate)},
                    { _appSetting.QueryStringKey.Page, pagerInfo.PageNo.ToString()},
                    { _appSetting.QueryStringKey.PageSize, pagerInfo.PageSize.ToString()},
                };

                string path = $"{_appSetting.QueryBetLogPath}?{nameValueCollection.ToQueryString()}";

                BaseReturnDataModel<string> returnDataModel = DoApiRequest(HttpMethod.Get, path, requestBody: null, out DetailRequestAndResponse detailRequestAndResponse);

                if (!returnDataModel.IsSuccess)
                {
                    return new BaseReturnDataModel<RequestAndResponse>(returnDataModel.Message);
                }

                //這邊多做反序列是為了後面的分頁撈取
                CQ9Response<CQ9BetLogViewModel> pageResponse = returnDataModel.DataModel.Deserialize<CQ9Response<CQ9BetLogViewModel>>();

                if (cq9Response == null)
                {
                    //第一次查詢
                    cq9Response = pageResponse;
                    requestAndResponse.RequestBody = path;
                }

                if (pageResponse.Data == null)
                {
                    break;
                }

                pagerInfo.TotalCount = pageResponse.Data.TotalSize; //計算頁碼

                if (pagerInfo.PageNo > 1 && pageResponse.Data.Data.AnyAndNotNull())
                {
                    cq9Response.Data.Data.AddRange(pageResponse.Data.Data);
                }

                pagerInfo.PageNo++;

                if (pagerInfo.PageNo > pagerInfo.TotalPageCount)
                {
                    break;
                }

                TaskUtil.DelayAndWait(_appSetting.QueryPagedBetLogIntervalSeconds * 1000);
            }

            requestAndResponse.ResponseContent = cq9Response.ToJsonString();

            return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.Success, requestAndResponse);
        }

        /// <summary>
        /// 處理轉帳回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetTransferReturnModel(string apiResult)
        {
            return GetUserScoreReturnModel(apiResult);
        }

        /// <summary>
        /// 處理查詢訂單回傳結果
        /// </summary>
        public override BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            CQ9Response<CQ9TransactionRecord> cq9Response = apiResult.Deserialize<CQ9Response<CQ9TransactionRecord>>();

            if (!cq9Response.Status.IsSuccess)
            {
                return new BaseReturnModel(cq9Response.Status.ErrorMessage);
            }

            if (cq9Response.Data != null)
            {
                CQ9TransactionStatus responseStatus = cq9Response.Data.TransactionStatus;

                if (responseStatus == CQ9TransactionStatus.Success)
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                if (responseStatus == CQ9TransactionStatus.Failed)
                {
                    return new BaseReturnModel(responseStatus.Value);
                }
            }

            return null;
        }

        /// <summary>
        /// 處理取得餘額回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            CQ9Response<CQ9PlayerBalance> cq9Response = apiResult.Deserialize<CQ9Response<CQ9PlayerBalance>>();

            if (!cq9Response.Status.IsSuccess)
            {
                return new BaseReturnDataModel<UserScore>(cq9Response.Status.ErrorMessage);
            }

            return new BaseReturnDataModel<UserScore>(ReturnCode.Success,
                new UserScore
                {
                    AvailableScores = cq9Response.Data.Balance
                });
        }

        /// <summary>
        /// 檢查以及創建帳號
        /// </summary>
        protected override BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam createRemoteAccountParam)
        {
            BaseReturnModel checkAccountReturnModel = CheckRemoteAccountExist(createRemoteAccountParam);

            if (checkAccountReturnModel.IsSuccess)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            BaseReturnDataModel<string> returnModel = GetRemoteCreateAccountApiResult(createRemoteAccountParam);

            if (!returnModel.IsSuccess)
            {
                return returnModel;
            }

            CQ9Response<CQ9PlayerInfo> cp9Response = returnModel.DataModel.Deserialize<CQ9Response<CQ9PlayerInfo>>();

            if (!cp9Response.Status.IsSuccess)
            {
                return new BaseReturnModel(cp9Response.Status.ErrorMessage);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        /// <summary>
        /// 取得遊戲入口網址
        /// </summary>
        protected override BaseReturnDataModel<string> GetRemoteForwardGameUrl(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            BaseReturnDataModel<string> returnDataModel = GetRemoteLoginApiResult(tpGameRemoteLoginParam);

            if (!returnDataModel.IsSuccess)
            {
                return returnDataModel;
            }

            CQ9Response<CQ9PlayerLogin> cq9Response = returnDataModel.DataModel.Deserialize<CQ9Response<CQ9PlayerLogin>>();

            if (cq9Response.Status == null || cq9Response.Data == null)
            {
                LogUtilService.Error($"{returnDataModel.DataModel.ToJsonString()}");

                return new BaseReturnDataModel<string>(ReturnCode.OperationFailed);
            }

            if (!cq9Response.Status.IsSuccess)
            {
                return new BaseReturnDataModel<string>(cq9Response.Status.ErrorMessage);
            }

            string userToken = cq9Response.Data.Usertoken;
            string lang = _appSetting.Lang;
            string app = "N";
            string gameplat = "web";

            if (tpGameRemoteLoginParam.IsMobile)
            {
                app = "Y";
                gameplat = "mobile";
            }

            var nameValueCollection = new NameValueCollection()
            {
                { _appSetting.QueryStringKey.UserToken, userToken },
                { _appSetting.QueryStringKey.Lang, lang },
                { _appSetting.QueryStringKey.App, app },
            };

            string path = string.Empty;
            string remoteCode = tpGameRemoteLoginParam.LoginInfo.RemoteCode;

            if (!remoteCode.IsNullOrEmpty())
            {
                path = _appSetting.PlayerGameLinkPath;
                nameValueCollection.Add(_appSetting.QueryStringKey.Gamehall, _appSetting.Gamehall);
                nameValueCollection.Add(_appSetting.QueryStringKey.Gamecode, remoteCode);
                nameValueCollection.Add(_appSetting.QueryStringKey.Gameplat, gameplat);
            }
            else
            {
                path = _appSetting.PlayerLobbyLinkPath;
            }

            string requestBody = nameValueCollection.ToQueryString();

            BaseReturnDataModel<string> reponseDataModel = DoApiRequest(
                HttpMethod.Post,
                path,
                requestBody,
                out DetailRequestAndResponse detailRequestAndResponse);

            CQ9Response<CQ9LobbyLink> cq9LobbyResponse = reponseDataModel.DataModel.Deserialize<CQ9Response<CQ9LobbyLink>>();

            if (!cq9Response.Status.IsSuccess)
            {
                return new BaseReturnDataModel<string>(cq9Response.Status.ErrorMessage);
            }

            return new BaseReturnDataModel<string>(ReturnCode.Success, cq9LobbyResponse.Data.Url);
        }

        protected override BaseReturnDataModel<string> GetRemoteLoginApiResult(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            var nameValueCollection = new NameValueCollection()
            {
                { _appSetting.QueryStringKey.Account , tpGameRemoteLoginParam.CreateRemoteAccountParam.TPGameAccount },
                { _appSetting.QueryStringKey.Password, tpGameRemoteLoginParam.CreateRemoteAccountParam.TPGamePassword }
            };

            string path = _appSetting.PlayerLoginPath;
            string requestBody = nameValueCollection.ToQueryString();

            BaseReturnDataModel<string> returnDataModel = DoApiRequest(
                HttpMethod.Post,
                path,
                requestBody,
                out DetailRequestAndResponse detailRequestAndResponse);

            return returnDataModel;
        }

        protected override void DoKickUser(Model.Entity.ThirdPartyUserAccount thirdPartyUserAccount)
        {
            var nameValueCollection = new NameValueCollection()
            {
                { _appSetting.QueryStringKey.Account, thirdPartyUserAccount.Account },
            };

            string path = _appSetting.PlayerLogoutPath;
            string requestBody = nameValueCollection.ToQueryString();

            DoApiRequest(
                HttpMethod.Post,
                path,
                requestBody,
                out DetailRequestAndResponse detailRequestAndResponse);
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            string path = _appSetting.GetPlayerCheckAccountPath(createRemoteAccountParam.TPGameAccount);
            BaseReturnDataModel<string> returnDataModel = DoApiRequest(HttpMethod.Get, path, requestBody: null, out DetailRequestAndResponse detailRequestAndResponse);

            return returnDataModel;
        }

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            var nameValueCollection = new NameValueCollection()
            {
                { _appSetting.QueryStringKey.Account, createRemoteAccountParam.TPGameAccount },
                { _appSetting.QueryStringKey.Password, createRemoteAccountParam.TPGamePassword }
            };

            string path = _appSetting.CreatePlayerPath;
            string requestBody = nameValueCollection.ToQueryString();

            BaseReturnDataModel<string> returnDataModel = DoApiRequest(
                HttpMethod.Post,
                path,
                requestBody,
                out DetailRequestAndResponse detailRequestAndResponse);

            return returnDataModel;
        }

        /// <summary>
        /// 檢查帳號
        /// </summary>
        private BaseReturnModel CheckRemoteAccountExist(CreateRemoteAccountParam createRemoteAccountParam)
        {
            BaseReturnDataModel<string> returnDataModel = GetRemoteCheckAccountExistApiResult(createRemoteAccountParam);

            if (!returnDataModel.IsSuccess)
            {
                return new BaseReturnModel(returnDataModel.Message);
            }

            CQ9Response<bool> cq9Response = returnDataModel.DataModel.Deserialize<CQ9Response<bool>>();

            if (!cq9Response.Status.IsSuccess)
            {
                return new BaseReturnModel(cq9Response.Status.ErrorMessage);
            }

            if (!cq9Response.Data)
            {
                return new BaseReturnModel(ReturnCode.UserNotFound);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        private BaseReturnDataModel<string> DoApiRequest(HttpMethod httpMethod, string path, string requestBody, out DetailRequestAndResponse detail)
        {
            string fullUrl = GetCombineUrl(_appSetting.APIUrl, path);

            var webRequestParam = new WebRequestParam()
            {
                Purpose = $"TPGService.{Product.Value}請求",
                Method = httpMethod,
                Url = fullUrl,
                Body = requestBody,
                Headers = _headers,
                ContentType = HttpWebRequestContentType.WwwFormUrlencoded,
                IsResponseValidJson = true,
            };

            string apiResult = HttpWebRequestUtil.GetResponse(webRequestParam, out HttpStatusCode httpStatusCode);
            detail = new DetailRequestAndResponse(webRequestParam, apiResult);

            return ConvertToApiReturnDataModel(httpStatusCode, apiResult);
        }

        private string ToApiDateTimeFormatString(DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToRFC3339DateTimeFormatString(_appSetting.TimeZoneOffset);
        }
    }
}