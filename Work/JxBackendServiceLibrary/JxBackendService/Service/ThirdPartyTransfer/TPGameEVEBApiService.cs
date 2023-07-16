using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.EVO;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameEVEBApiService : BaseTPGameApiService
    {
        public static readonly int MaxSearchRangeMinutes = 30;

        public static readonly int TokenKeyKeepSeconds = 5 * 60;

        public static readonly int SubProductCode = 29;//EV子產品代碼

        public EVEBSharedAppSetting AppSettings => EVEBSharedAppSetting.Instance;

        public override PlatformProduct Product => PlatformProduct.EVEB;

        private readonly IJxCacheService _jxCacheService;

        public TPGameEVEBApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _jxCacheService = ResolveServiceForModel<IJxCacheService>(envLoginUser.Application);
        }

        /// <summary>
        /// 轉帳
        /// </summary>
        protected override DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new EVTransferRequestModel
            {
                ProductCode = SubProductCode,
                MemberAccount = tpGameAccount,
                Amount = tpGameMoneyInfo.Amount,
                TransNo = tpGameMoneyInfo.OrderID,
                Type = isMoneyIn ? 1 : 2
            };

            DoPostRequest(AppSettings.TransferUrl, request.ToJsonString(), out DetailRequestAndResponse detail);

            return detail;
        }

        /// <summary>
        /// 查詢訂單
        /// </summary>
        protected override DetailRequestAndResponse GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new EVCheckTransferRequestModel
            {
                ProductCode = SubProductCode,
                TransNo = tpGameMoneyInfo.OrderID,
                MemberAccount = tpGameAccount
            };

            DoPostRequest(AppSettings.CheckTransferUrl, request.ToJsonString(), out DetailRequestAndResponse detail);

            return detail;
        }

        /// <summary>
        /// 查詢餘額
        /// </summary>
        protected override string GetRemoteUserScoreApiResult(string tpGameAccount)
        {
            var request = new EVBaseAccountWithCodeRequestModel
            {
                ProductCode = SubProductCode,
                MemberAccount = tpGameAccount
            };

            var returnModel = DoPostRequest(AppSettings.GetBalanceUrl, request.ToJsonString(), out DetailRequestAndResponse detail);

            return returnModel.DataModel;
        }

        /// <summary>
        /// 取得盈虧資料
        /// </summary>
        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            DateTime searchStartDate = lastSearchToken.ToInt64().ToDateTime();
            DateTime searchEndDate = searchStartDate.AddMinutes(MaxSearchRangeMinutes);

            if (searchEndDate > DateTime.Now)
            {
                searchEndDate = DateTime.Now;
            }

            var request = new EVBetLogRequestModel
            {
                StartTime = searchStartDate.AddHours(-12).ToFormatDateTimeString(), //美東時間
                EndTime = searchEndDate.AddHours(-12).ToFormatDateTimeString(), //美東時間
                Page = 1,
                ProductCode = SubProductCode,
                TimeType = 2
            };

            string requestBody = request.ToJsonString();

            BaseReturnDataModel<string> returnModel = DoPostRequest(AppSettings.GetBetLogUrl, requestBody, out DetailRequestAndResponse detail);
            var returnDataModel = returnModel.CastByCodeAndMessage<RequestAndResponse>();

            returnDataModel.DataModel = new RequestAndResponse()
            {
                RequestBody = requestBody,
                ResponseContent = returnModel.DataModel
            };

            return returnDataModel;
        }

        /// <summary>
        /// 處理轉帳回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetTransferReturnModel(string apiResult)
        {
            EVTransferResponseModel transferModel = apiResult.Deserialize<EVTransferResponseModel>();

            if (transferModel.ErrorCode == EVResponseCode.Success && transferModel.Data == EVTransferSatatus.Success)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success);
            }

            return new BaseReturnDataModel<UserScore>(transferModel.ErrorMessage, null);
        }

        /// <summary>
        /// 處理查詢訂單回傳結果
        /// </summary>
        public override BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            EVQueryOrderResponseModel transferModel = apiResult.Deserialize<EVQueryOrderResponseModel>();

            if (transferModel.ErrorCode == EVResponseCode.Success && transferModel.Data == EVTransferSatatus.Success)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success);
            }

            return new BaseReturnDataModel<UserScore>($"error:{transferModel.ErrorMessage}", null);
        }

        /// <summary>
        /// 處理取得餘額回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            EVBalanceResponseModel userScoreModel = apiResult.Deserialize<EVBalanceResponseModel>();

            if (userScoreModel.ErrorCode == EVResponseCode.Success)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success, new UserScore() { AvailableScores = userScoreModel.Data });
            }

            return new BaseReturnDataModel<UserScore>(userScoreModel.ErrorMessage, null);
        }

        /// <summary>
        /// 檢查以及創建帳號
        /// </summary>
        protected override BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam param)
        {
            BaseReturnDataModel<bool> checkAccountReturnModel = CheckAccountExist(param.TPGameAccount);

            if (checkAccountReturnModel.IsSuccess)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            BaseReturnDataModel<string> returnModel = GetRemoteCreateAccountApiResult(param);

            if (returnModel.IsSuccess)
            {
                EVBaseResponseModel registerModel = returnModel.DataModel.Deserialize<EVBaseResponseModel>();

                if (registerModel.ErrorCode == EVResponseCode.Success || registerModel.ErrorCode == EVResponseCode.AccountExist)
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                return new BaseReturnModel(registerModel.ErrorMessage);
            }

            return new BaseReturnModel(returnModel.Message);
        }

        /// <summary>
        /// 檢查帳號
        /// </summary>
        private BaseReturnDataModel<bool> CheckAccountExist(string tpGameAccount)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteCheckAccountExistApiResult(tpGameAccount);

            if (returnModel.IsSuccess)
            {
                EVCheckExistResponseModel checkAccountModel = returnModel.DataModel.Deserialize<EVCheckExistResponseModel>();

                if (checkAccountModel.ErrorCode == EVResponseCode.Success && checkAccountModel.Data.Accounts.Any(x => x.ProductCode == SubProductCode))
                {
                    return new BaseReturnDataModel<bool>(ReturnCode.Success, true);
                }

                return new BaseReturnDataModel<bool>(checkAccountModel.ErrorMessage, false);
            }

            return new BaseReturnDataModel<bool>(returnModel.Message, false);
        }

        /// <summary>
        /// 取得遊戲入口網址
        /// </summary>
        protected override BaseReturnDataModel<string> GetRemoteForwardGameUrl(string tpGameAccount, string ipAddress, bool isMobile, LoginInfo loginInfo)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteLoginApiResult(tpGameAccount, ipAddress, isMobile, loginInfo);

            if (!returnModel.IsSuccess)
            {
                return new BaseReturnDataModel<string>(returnModel.Message, string.Empty);
            }

            EVLunchGameResponseModel launchGameModel = returnModel.DataModel.Deserialize<EVLunchGameResponseModel>();

            if (launchGameModel.ErrorCode == EVResponseCode.Success)
            {
                return new BaseReturnDataModel<string>(ReturnCode.Success, launchGameModel.Data);
            }

            return new BaseReturnDataModel<string>(launchGameModel.ErrorMessage);
        }

        protected override BaseReturnDataModel<string> GetRemoteLoginApiResult(string tpGameAccount, string ipAddress, bool isMobile, LoginInfo loginInfo)
        {
            var request = new EVLunchGameRequestModel
            {
                GameId = "top_games",
                Ip = ipAddress,
                IsMobile = isMobile,
                MemberAccount = tpGameAccount,
                ProductCode = SubProductCode
            };

            BaseReturnDataModel<string> returnModel = DoPostRequest(AppSettings.LaunchGameUrl, request.ToJsonString(), out DetailRequestAndResponse detail);

            return returnModel;
        }

        private BaseReturnDataModel<string> DoPostRequest(string relativeUrl, string requestBody, out DetailRequestAndResponse detail)
        {
            return DoPostRequest(relativeUrl, requestBody, isUseToken: true, out detail);
        }

        /// <summary>
        /// 打第三方API，加密簽名方法
        /// </summary>
        private BaseReturnDataModel<string> DoPostRequest(string relativeUrl, string requestBody, bool isUseToken, out DetailRequestAndResponse detail)
        {
            string url = GetFullUrl(AppSettings.ServiceUrl, relativeUrl);

            Dictionary<string, string> headers = new Dictionary<string, string>();

            if (isUseToken)
            {
                string token = GetToken();
                headers.Add("Authorization", "Bearer " + token);
            }

            var httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>();

            var webRequestParam = new WebRequestParam()
            {
                Purpose = $"TPGService.{Product.Value}請求",
                Method = HttpMethod.Post,
                Url = url,
                Body = requestBody,
                ContentType = HttpWebRequestContentType.Json,
                IsResponseValidJson = true,
                Headers = headers
            };

            string apiResult = httpWebRequestUtilService.GetResponse(webRequestParam, out HttpStatusCode httpStatusCode);
            detail = new DetailRequestAndResponse(webRequestParam, apiResult);

            return ConverToApiReturnDataModel(httpStatusCode, apiResult);
        }

        /// <summary>
        /// 取得Token並快取
        /// </summary>
        private string GetToken()
        {
            return _jxCacheService.GetCache(new SearchCacheParam()
            {
                CacheSeconds = TokenKeyKeepSeconds,//第三方限制十分鐘, 設定小於以免剛好過期
                Key = CacheKey.EVEBTokenKey,
                IsSlidingExpiration = false
            }, () =>
            {
                EVTokenModel tokenModel = new EVTokenModel();

                BaseReturnDataModel<string> returnModel = DoPostRequest(AppSettings.GetTokenUrl, tokenModel.ToJsonString(), isUseToken: false, out DetailRequestAndResponse detail);

                if (returnModel.IsSuccess)
                {
                    EVBaseResponseWtihDataModel<string> tokeResultModel = returnModel.DataModel.Deserialize<EVBaseResponseWtihDataModel<string>>();
                    if (tokeResultModel.ErrorCode == EVResponseCode.Success)
                    {
                        return tokeResultModel.Data;
                    }

                    throw new Exception("取得Token失敗，請檢查參數設定");
                }

                throw new Exception("取得Token失敗，請檢查線路");
            });
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(string tpGameAccount)
        {
            var request = new EVRegisterRequestModel
            {
                MemberAccount = tpGameAccount
            };

            return DoPostRequest(AppSettings.CheckAccountExistUrl, request.ToJsonString(), out DetailRequestAndResponse detail);
        }

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam param)
        {
            var request = new EVRegisterRequestModel
            {
                MemberAccount = param.TPGameAccount
            };

            return DoPostRequest(AppSettings.CreateAccountUrl, request.ToJsonString(), out DetailRequestAndResponse detail);
        }
    }
}