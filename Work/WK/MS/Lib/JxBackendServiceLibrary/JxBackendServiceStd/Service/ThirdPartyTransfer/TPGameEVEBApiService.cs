using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
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

        private EVEBSharedAppSetting AppSettings => EVEBSharedAppSetting.Instance;

        public override PlatformProduct Product => PlatformProduct.EVEB;

        private readonly Lazy<IJxCacheService> _jxCacheService;

        public TPGameEVEBApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _jxCacheService = DependencyUtil.ResolveService<IJxCacheService>();
        }

        protected override string CreateTPGamePasswordByRule(int userId, string tpGameAccount) => null;

        /// <summary>
        /// 轉帳
        /// </summary>
        public override DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, CreateRemoteAccountParam createRemoteAccountParam, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new EVTransferRequestModel
            {
                ProductCode = SubProductCode,
                MemberAccount = createRemoteAccountParam.TPGameAccount,
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
        public override string GetRemoteUserScoreApiResult(CreateRemoteAccountParam createRemoteAccountParam, IInvocationUserParam invocationUserParam)
        {
            var request = new EVBaseAccountWithCodeRequestModel
            {
                ProductCode = SubProductCode,
                MemberAccount = createRemoteAccountParam.TPGameAccount
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

            return new BaseReturnDataModel<UserScore>(transferModel.ErrorMessage);
        }

        /// <summary>
        /// 處理查詢訂單回傳結果
        /// </summary>
        public override BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            EVQueryOrderResponseModel transferModel = apiResult.Deserialize<EVQueryOrderResponseModel>();

            if (transferModel.ErrorCode == EVResponseCode.Success && transferModel.Data == EVTransferSatatus.Success)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            return new BaseReturnModel($"error:{transferModel.ErrorMessage}");
        }

        /// <summary>
        /// 處理取得餘額回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            EVBalanceResponseModel userScoreModel = apiResult.Deserialize<EVBalanceResponseModel>();

            if (userScoreModel.ErrorCode == EVResponseCode.Success && userScoreModel.Data != null)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success, new UserScore() { AvailableScores = userScoreModel.Data.Value });
            }

            return new BaseReturnDataModel<UserScore>(userScoreModel.ErrorMessage, null);
        }

        /// <summary>
        /// 檢查以及創建帳號
        /// </summary>
        protected override BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam createRemoteAccountParam)
        {
            BaseReturnDataModel<bool> checkAccountReturnModel = CheckAccountExist(createRemoteAccountParam);

            if (checkAccountReturnModel.IsSuccess)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            BaseReturnDataModel<string> returnModel = GetRemoteCreateAccountApiResult(createRemoteAccountParam);

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
        private BaseReturnDataModel<bool> CheckAccountExist(CreateRemoteAccountParam createRemoteAccountParam)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteCheckAccountExistApiResult(createRemoteAccountParam);

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
        protected override BaseReturnDataModel<string> GetRemoteForwardGameUrl(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteLoginApiResult(tpGameRemoteLoginParam);

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

        protected override BaseReturnDataModel<string> GetRemoteLoginApiResult(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            var request = new EVLunchGameRequestModel
            {
                GameId = "top_games",
                Ip = tpGameRemoteLoginParam.IpAddress,
                IsMobile = tpGameRemoteLoginParam.IsMobile,
                MemberAccount = tpGameRemoteLoginParam.CreateRemoteAccountParam.TPGameAccount,
                ProductCode = SubProductCode
            };

            BaseReturnDataModel<string> returnModel = DoPostRequest(AppSettings.LaunchGameUrl, request.ToJsonString(), out DetailRequestAndResponse detail);

            return returnModel;
        }

        protected override void DoKickUser(Model.Entity.ThirdPartyUserAccount thirdPartyUserAccount) => new NotImplementedException();

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

            var httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>().Value;

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

            return ConvertToApiReturnDataModel(httpStatusCode, apiResult);
        }

        /// <summary>
        /// 取得Token並快取
        /// </summary>
        private string GetToken()
        {
            return _jxCacheService.Value.GetCache(new SearchCacheParam()
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

                    if (tokeResultModel != null && tokeResultModel.ErrorCode == EVResponseCode.Success)
                    {
                        return tokeResultModel.Data;
                    }

                    throw new Exception("取得Token失敗，請檢查參數設定");
                }

                throw new Exception("取得Token失敗，請檢查線路");
            });
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            var request = new EVRegisterRequestModel
            {
                MemberAccount = createRemoteAccountParam.TPGameAccount
            };

            return DoPostRequest(AppSettings.CheckAccountExistUrl, request.ToJsonString(), out DetailRequestAndResponse detail);
        }

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            var request = new EVRegisterRequestModel
            {
                MemberAccount = createRemoteAccountParam.TPGameAccount
            };

            return DoPostRequest(AppSettings.CreateAccountUrl, request.ToJsonString(), out DetailRequestAndResponse detail);
        }
    }
}