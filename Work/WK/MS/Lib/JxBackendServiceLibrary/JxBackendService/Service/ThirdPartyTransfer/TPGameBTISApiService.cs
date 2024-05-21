using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Xml;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty.Handicap;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.BTI;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Threading;
using System.Web;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameBTISApiService : BaseTPGameApiService
    {
        private static readonly int s_maxSearchRangeMinutes = 180;

        private static readonly int s_requestBettingLogRetryIntervalSeconds = 6;

        private static readonly int s_maxSearchMinutesAgo = 16;

        private static readonly string _highLoadingErrorMessage = "General Error";

        private static readonly BTISSharedAppSetting AppSettings = BTISSharedAppSetting.Instance;

        public static int MaxSearchRangeMinutes => s_maxSearchRangeMinutes;

        public static int MaxSearchMinutesAgo => s_maxSearchMinutesAgo;

        public override PlatformProduct Product => PlatformProduct.BTIS;

        public TPGameBTISApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        /// <summary>
        /// 轉帳
        /// </summary>
        protected override DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            NameValueCollection postParams = GetBasicWalletPostParams();
            postParams.Add("MerchantCustomerCode", tpGameAccount);
            postParams.Add("Amount", tpGameMoneyInfo.Amount.ToString());
            postParams.Add("RefTransactionCode", tpGameMoneyInfo.OrderID);
            postParams.Add("BonusCode", null);

            string postUrl = null;

            if (isMoneyIn)
            {
                postUrl = AppSettings.TransferToWHLUrl;
            }
            else
            {
                postUrl = AppSettings.TransferFromWHLUrl;
            }

            DoWalletPostRequest(postUrl, postParams.ToString(), out DetailRequestAndResponse detail);

            return detail;
        }

        /// <summary>
        /// 查詢訂單
        /// </summary>
        protected override DetailRequestAndResponse GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            NameValueCollection postParams = GetBasicWalletPostParams();
            postParams.Add("RefTransactionCode", tpGameMoneyInfo.OrderID);

            DoWalletPostRequest(AppSettings.CheckTransactionUrl, postParams.ToString(), out DetailRequestAndResponse detail);

            return detail;
        }

        /// <summary>
        /// 查詢餘額
        /// </summary>
        protected override string GetRemoteUserScoreApiResult(string tpGameAccount)
        {
            NameValueCollection postParams = GetBasicWalletPostParams();
            postParams.Add("MerchantCustomerCode", tpGameAccount);

            BaseReturnDataModel<string> apiReturnModel = DoWalletPostRequest(AppSettings.GetBalanceUrl, postParams.ToString(), out DetailRequestAndResponse detail);

            return apiReturnModel.DataModel;
        }

        /// <summary>
        /// 取得盈虧資料
        /// </summary>
        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            /*
                The method returns the all the settled bets per player, per time period.
                We recommend to fetch data from last 15 mins data per 15 mins.
                Example: When 10:30 fetch 10:15~10:30, and 10:45 fetch 10:30~10:45.
                Note: The maximum range of time allowed per query is 24 hours.
            */

            DateTime maxSearchEndDate = DateTime.UtcNow.AddMinutes(-s_maxSearchMinutesAgo);
            DateTime searchStartDate = DateTime.UtcNow;

            if (!lastSearchToken.IsNullOrEmpty())
            {
                searchStartDate = DateTime.Parse(lastSearchToken);
            }

            DateTime searchEndDate = searchStartDate.AddMinutes(s_maxSearchRangeMinutes);

            if (searchEndDate > maxSearchEndDate)
            {
                searchEndDate = maxSearchEndDate;
            }

            if (searchEndDate < searchStartDate)
            {
                searchEndDate = searchStartDate;
            }

            string dataApiToken = GetDataApiToken();

            //取得Data Api Token
            BettingHistoryRequest bettingHistoryRequest = new BettingHistoryRequest()
            {
                Token = dataApiToken,
                From = ConvertToBTISDateTimeString(searchStartDate),
                To = ConvertToBTISDateTimeString(searchEndDate),
                Pagination = new BTISPagination()
                {
                    Page = 0
                }
            };

            try
            {
                BaseReturnDataModel<RequestAndResponse> returnDataModel = GetResponseWithRetry(bettingHistoryRequest);

                if (!returnDataModel.IsSuccess)
                {
                    return returnDataModel;
                }

                var historyResponse = returnDataModel.DataModel.ResponseContent.Deserialize<BTISBettingHistoryResponse>();

                //取得其他頁資料
                for (int i = historyResponse.CurrentPage + 1; i < historyResponse.TotalPages; i++)
                {
                    Thread.Sleep(s_requestBettingLogRetryIntervalSeconds * 1000);
                    bettingHistoryRequest.Pagination.Page = i;
                    BaseReturnDataModel<RequestAndResponse> pagedDataResult = GetResponseWithRetry(bettingHistoryRequest);

                    if (!pagedDataResult.IsSuccess)
                    {
                        return pagedDataResult;
                    }

                    var pagedHistoryResponse = pagedDataResult.DataModel.ResponseContent.Deserialize<BTISBettingHistoryResponse>();

                    if (!pagedHistoryResponse.IsSuccess)
                    {
                        return new BaseReturnDataModel<RequestAndResponse>($"ErrorCode={pagedHistoryResponse.ErrorCode}", new RequestAndResponse());
                    }

                    historyResponse.Bets.AddRange(pagedHistoryResponse.Bets);
                }

                returnDataModel.DataModel.ResponseContent = historyResponse.ToJsonString();

                return returnDataModel;
            }
            catch (Exception ex)
            {
                if (_highLoadingErrorMessage.Equals(ex.Message, StringComparison.OrdinalIgnoreCase))
                {
                    LogUtil.ForcedDebug(ex.Message);

                    return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.NoDataChanged);
                }

                throw ex;
            }
        }

        /// <summary>
        /// 處理轉帳回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetTransferReturnModel(string apiResult)
        {
            if (!XmlUtil.IsValidXml(apiResult))
            {
                return new BaseReturnDataModel<UserScore>(apiResult, null);
            }

            var walletResponse = XmlUtil.Deserialize<BTISWalletResponse>(apiResult);

            if (walletResponse.IsSuccess)
            {
                UserScore userScore = null;

                if (walletResponse.Balance.HasValue && walletResponse.OpenBetsBalance.HasValue)
                {
                    userScore = new UserScore()
                    {
                        AvailableScores = walletResponse.Balance.Value,
                        FreezeScores = walletResponse.OpenBetsBalance.Value
                    };
                }

                return new BaseReturnDataModel<UserScore>(ReturnCode.Success, userScore);
            }

            return new BaseReturnDataModel<UserScore>(walletResponse.ErrorCode, null);
        }

        /// <summary>
        /// 處理查詢訂單回傳結果
        /// </summary>
        public override BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            if (!XmlUtil.IsValidXml(apiResult))
            {
                return null; //傳null讓後續流程終止
            }

            var walletResponse = XmlUtil.Deserialize<BTISWalletResponse>(apiResult);

            if (walletResponse.IsSuccess)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            return new BaseReturnModel($"error:{walletResponse.ErrorCode}");
        }

        /// <summary>
        /// 處理取得餘額回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            if (!XmlUtil.IsValidXml(apiResult))
            {
                return new BaseReturnDataModel<UserScore>(apiResult, null);
            }

            var walletResponse = XmlUtil.Deserialize<BTISWalletResponse>(apiResult);

            if (walletResponse.IsSuccess)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success, new UserScore()
                {
                    AvailableScores = walletResponse.Balance.GetValueOrDefault(),
                    FreezeScores = walletResponse.OpenBetsBalance.GetValueOrDefault()
                });
            }

            return new BaseReturnDataModel<UserScore>(walletResponse.ErrorCode, null);
        }

        /// <summary>
        /// 檢查以及創建帳號
        /// </summary>
        protected override BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam param)
        {
            BaseReturnDataModel<bool> checkAccountReturnModel = CheckRemoteAccountExist(param.TPGameAccount);

            if (checkAccountReturnModel.IsSuccess)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            BaseReturnDataModel<string> returnModel = GetRemoteCreateAccountApiResult(param);

            if (returnModel.IsSuccess)
            {
                if (!XmlUtil.IsValidXml(returnModel.DataModel))
                {
                    return new BaseReturnModel(returnModel.DataModel);
                }

                var walletResponse = XmlUtil.Deserialize<BTISWalletResponse>(returnModel.DataModel);

                if (walletResponse.IsSuccess)
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                return new BaseReturnModel(walletResponse.ErrorCode);
            }

            return new BaseReturnModel(returnModel.Message);
        }

        /// <summary>
        /// 取得遊戲入口網址
        /// </summary>
        protected override BaseReturnDataModel<string> GetRemoteForwardGameUrl(string tpGameAccount, string ipAddress, bool isMobile, LoginInfo loginInfo)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteLoginApiResult(tpGameAccount, ipAddress, isMobile, loginInfo);

            if (!returnModel.IsSuccess)
            {
                return returnModel;
            }

            if (!XmlUtil.IsValidXml(returnModel.DataModel))
            {
                return new BaseReturnDataModel<string>(ReturnCode.SystemError, returnModel.DataModel);
            }

            var walletResponse = XmlUtil.Deserialize<BTISWalletResponse>(returnModel.DataModel);

            if (!walletResponse.IsSuccess)
            {
                return new BaseReturnDataModel<string>(ReturnCode.SystemError, walletResponse.ErrorCode);
            }

            //組網址
            string queryString = string.Format("?operatorToken={0}", HttpUtility.UrlEncode(walletResponse.AuthToken));

            string url = string.Format("{0}{1}", AppSettings.ENTRANCE_URL, queryString);

            return new BaseReturnDataModel<string>(ReturnCode.Success, url);
        }

        protected override BaseReturnDataModel<string> GetRemoteLoginApiResult(string tpGameAccount, string ipAddress, bool isMobile, LoginInfo loginInfo)
            => GetRemoteCheckAccountExistApiResult(tpGameAccount);

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(string tpGameAccount)
        {
            NameValueCollection postParams = GetBasicWalletPostParams();
            postParams.Add("MerchantCustomerCode", tpGameAccount);

            return DoWalletPostRequest(AppSettings.GetCustomerAuthTokenUrl, postParams.ToString(), out DetailRequestAndResponse detail);
        }

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam param)
        {
            NameValueCollection postParams = GetBasicWalletPostParams();
            postParams.Add("MerchantCustomerCode", param.TPGameAccount);
            postParams.Add("LoginName", param.TPGameAccount);
            postParams.Add("CurrencyCode", AppSettings.WALLET_API_CurrencyCode);
            postParams.Add("CountryCode", AppSettings.WALLET_API_CountryCode);
            postParams.Add("FirstName", param.TPGameAccount);
            postParams.Add("LastName", param.TPGameAccount);
            postParams.Add("Group1ID", AppSettings.WALLET_API_Group1ID);
            postParams.Add("CustomerDefaultLanguage", AppSettings.WALLET_API_CustomerDefaultLanguage);
            postParams.Add("City", null);
            postParams.Add("CustomerMoreInfo", null);
            postParams.Add("DomainID", null);
            postParams.Add("DateOfBirth", null);

            return DoWalletPostRequest(AppSettings.CreateAccountUrl, postParams.ToString(), out DetailRequestAndResponse detail);
        }

        protected override PlatformHandicap ConvertToPlatformHandicap(string handicap)
        {
            BTISHandicap btisHandicap = BTISHandicap.GetSingle(handicap);

            if (btisHandicap == null)
            {
                return null;
            }

            return btisHandicap.PlatformHandicap;
        }

        private BaseReturnDataModel<RequestAndResponse> GetResponseWithRetry(BettingHistoryRequest bettingHistoryRequest)
        {
            BaseReturnDataModel<RequestAndResponse> returnDataModel = null;

            for (int i = 1; i <= 5; i++)
            {
                string requestBody = null;
                BaseReturnDataModel<string> returnModel = DoDataApiPostRequest(
                    AppSettings.BETTINGHISTORY_API_URL,
                    bettingHistoryRequest,
                    (body) => { requestBody = body; });

                returnDataModel = returnModel.CastByCodeAndMessage<RequestAndResponse>();

                returnDataModel.DataModel = new RequestAndResponse()
                {
                    RequestBody = requestBody,
                    ResponseContent = returnModel.DataModel
                };

                //遇到頻率太高的錯誤,就delay後重查
                if (returnDataModel.IsSuccess)
                {
                    var response = returnDataModel.DataModel.ResponseContent.Deserialize<BTISBaseDataResponse>();

                    if (response.ErrorCode != BTISDataErrorCode.ExceededApiCalls)
                    {
                        return returnDataModel;
                    }
                }

                Thread.Sleep(s_requestBettingLogRetryIntervalSeconds * 1000);
            }

            if (returnDataModel == null)
            {
                returnDataModel = new BaseReturnDataModel<RequestAndResponse>(ReturnCode.SystemError);
            }

            return returnDataModel;
        }

        /// <summary>
        /// 檢查帳號
        /// </summary>
        private BaseReturnDataModel<bool> CheckRemoteAccountExist(string tpGameAccount)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteCheckAccountExistApiResult(tpGameAccount);

            if (returnModel.IsSuccess)
            {
                string apiResult = returnModel.DataModel;

                if (!XmlUtil.IsValidXml(apiResult))
                {
                    return new BaseReturnDataModel<bool>(apiResult, false);
                }

                var walletResponse = XmlUtil.Deserialize<BTISWalletResponse>(apiResult);

                if (walletResponse.IsSuccess)
                {
                    return new BaseReturnDataModel<bool>(ReturnCode.Success, true);
                }

                return new BaseReturnDataModel<bool>(walletResponse.ErrorCode, false);
            }

            return new BaseReturnDataModel<bool>(returnModel.Message, false);
        }

        /// <summary>
        /// 打第三方錢包API
        /// </summary>
        private BaseReturnDataModel<string> DoWalletPostRequest(string relativeUrl, string requestBody, out DetailRequestAndResponse detail)
        {
            string fullUrl = GetCombineUrl(AppSettings.WALLET_API_URL, relativeUrl);

            var webRequestParam = new WebRequestParam()
            {
                Purpose = $"TPGService.{Product.Value}請求",
                Method = HttpMethod.Post,
                Url = fullUrl,
                Body = requestBody,
                ContentType = HttpWebRequestContentType.WwwFormUrlencoded,
                IsResponseValidJson = false,
            };

            string apiResult = HttpWebRequestUtil.GetResponse(webRequestParam, out HttpStatusCode httpStatusCode);
            detail = new DetailRequestAndResponse(webRequestParam, apiResult);

            return ConverToApiReturnDataModel(httpStatusCode, apiResult);
        }

        private NameValueCollection GetBasicWalletPostParams()
        {
            //必須透過ParseQueryString()來建立NameValueCollection物件，之後.ToString()才能轉換成queryString
            NameValueCollection postParams = System.Web.HttpUtility.ParseQueryString(string.Empty);
            postParams.Add("AgentUserName", AppSettings.API_AgentUserName);
            postParams.Add("AgentPassword", AppSettings.API_AgentPassword);

            return postParams;
        }

        /// <summary>
        /// 取得Data Api Token
        /// </summary>
        private string GetDataApiToken()
        {
            var request = new BTISDataGetTokenRequest()
            {
                AgentUserName = AppSettings.API_AgentUserName,
                AgentPassword = AppSettings.API_AgentPassword
            };

            BaseReturnDataModel<string> returnModel = DoDataApiPostRequest(AppSettings.GETTOKEN_API_URL, request);

            if (returnModel.IsSuccess)
            {
                var getTokenResponse = returnModel.DataModel.Deserialize<BTISGetTokenResponse>();

                if (getTokenResponse.IsSuccess)
                {
                    return getTokenResponse.Token;
                }

                throw new Exception("取得Data Api Token失敗，請檢查參數設定");
            }

            throw new Exception("取得Data Api Token失敗，請檢查線路");
        }

        /// <summary>
        /// 打第三方data API
        /// </summary>
        private BaseReturnDataModel<string> DoDataApiPostRequest(string dataTypeUrl, BTISBaseDataRequest data, Action<string> setBody = null)
        {
            string fullUrl = dataTypeUrl;//GetCombineUrl(dataTypeUrl, relativeUrl);

            if (data is BTISDataRequest)
            {
                fullUrl += "?token=" + HttpUtility.UrlEncode((data as BTISDataRequest).Token);
            }

            string body = data.ToJsonString();

            if (setBody != null)
            {
                setBody.Invoke(body);
            }

            var httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>();

            string apiResult = httpWebRequestUtilService.GetResponse(
                new WebRequestParam()
                {
                    Purpose = $"TPGService.{Product.Value}請求",
                    Method = HttpMethod.Post,
                    Url = fullUrl,
                    Body = body,
                    ContentType = HttpWebRequestContentType.Json,
                    IsResponseValidJson = true,
                },
            out HttpStatusCode httpStatusCode);

            return ConverToApiReturnDataModel(httpStatusCode, apiResult);
        }

        private string ConvertToBTISDateTimeString(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss");
        }
    }
}