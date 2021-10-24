﻿using JxBackendService.Common;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.AllBet;
using JxBackendService.Model.ThirdParty.PG;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Net;
using System.Web;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public class TPGamePGSLApiService : BaseTPGameApiService
    {
        private static readonly int _maxSearchCount = 1500;

        private PGSLSharedAppSettings AppSettings => PGSLSharedAppSettings.Instance;

        public override PlatformProduct Product => PlatformProduct.PGSL;

        private readonly IAppSettingService _appSettingService;

        public TPGamePGSLApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _appSettingService = ResolveKeyedForModel<IAppSettingService>(envLoginUser.Application);
        }

        /// <summary>
        /// 轉帳
        /// </summary>
        protected override string GetRemoteTransferApiResult(bool isMoneyIn, string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new PGTransferRequestModel
            {
                player_name = tpGameAccount,
                amount = Math.Round(tpGameMoneyInfo.Amount, 2),
                currency = AppSettings.Currncy,
                transfer_reference = tpGameMoneyInfo.OrderID
            };
            BaseReturnDataModel<string> returnModel = DoPostRequest(isMoneyIn ? AppSettings.TransferInUrl : AppSettings.TransferOutUrl, request.ToKeyValueURL());
            return returnModel.DataModel;
        }

        /// <summary>
        /// 查詢訂單
        /// </summary>
        protected override string GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new PGCheckTransferRequestModel
            {
                player_name = tpGameAccount,
                transfer_reference = tpGameMoneyInfo.OrderID
            };

            BaseReturnDataModel<string> returnModel = DoPostRequest(AppSettings.CheckTransferUrl, request.ToKeyValueURL());
            return returnModel.DataModel;
        }

        /// <summary>
        /// 查詢餘額
        /// </summary>
        protected override string GetRemoteUserScoreApiResult(string tpGameAccount)
        {
            var request = new PGUserBaseRequestModel
            {
                player_name = tpGameAccount
            };

            var returnModel = DoPostRequest(AppSettings.GetBalanceUrl, request.ToKeyValueURL());
            return returnModel.DataModel;
        }

        /// <summary>
        /// 取得盈虧資料
        /// </summary>
        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            var request = new PGBetLogRequestModel
            {
                bet_type = 1, //1:真實遊戲
                count = _maxSearchCount,
                row_version = long.Parse(lastSearchToken)
            };

            string requestBody = request.ToKeyValueURL();

            //這邊是用另外一個網址
            BaseReturnDataModel<string> returnModel = DoPostRequest(AppSettings.GrabServiceUrl, AppSettings.GrabUrlPathPrefix + AppSettings.GetBetLogUrl, requestBody);
            return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.GetSingle(returnModel.Code),
                new RequestAndResponse()
                {
                    RequestBody = requestBody,
                    ResponseContent = returnModel.DataModel
                });
        }

        /// <summary>
        /// 處理轉帳回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetTransferReturnModel(string apiResult)
        {
            PGTransferResponseModel transferModel = apiResult.Deserialize<PGTransferResponseModel>();

            if (transferModel.error == null && transferModel.data != null)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success);
            }

            return new BaseReturnDataModel<UserScore>(transferModel.error.message, null);
        }

        /// <summary>
        /// 處理查詢訂單回傳結果
        /// </summary>
        public override BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            PGQueryOrderResponseModel transferModel = apiResult.Deserialize<PGQueryOrderResponseModel>();

            if (transferModel.data != null && transferModel.error == null)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success);
            }

            return new BaseReturnDataModel<UserScore>(transferModel.error.message, null);
        }

        /// <summary>
        /// 處理取得餘額回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            PGBalanceResponseModel userScoreModel = apiResult.Deserialize<PGBalanceResponseModel>();

            if (userScoreModel.error == null)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success, new UserScore() { AvailableScores = userScoreModel.data.cashBalance });
            }

            return new BaseReturnDataModel<UserScore>(userScoreModel.error.message, null);
        }

        /// <summary>
        /// 檢查以及創建帳號
        /// </summary>
        protected override BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam param)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteCreateAccountApiResult(param);

            if (returnModel.IsSuccess)
            {
                PGRegisterResponseModel registerModel = returnModel.DataModel.Deserialize<PGRegisterResponseModel>();
                //檢查帳號重複同一隻API
                if ((registerModel.data != null && registerModel.data.action_result == 1) || registerModel.error.code == PGResponseCode.AccountExist)
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                return new BaseReturnModel(registerModel.error.message);
            }

            return new BaseReturnModel(returnModel.Message);
        }

        /// <summary>
        /// 取得遊戲入口網址
        /// </summary>
        protected override BaseReturnDataModel<string> GetRemoteForwardGameUrl(string tpGameAccount, string ip, bool isMobile)
        {
            string url = string.Empty;
            //加密參數傳遞
            string operator_player_session = string.Empty;

            PGLunchGameRequestModel lunchGameRequestModel = new PGLunchGameRequestModel();
            lunchGameRequestModel.Acc = tpGameAccount;
            lunchGameRequestModel.Key = EnvLoginUser.LoginUser.UserKey;
            
            //正式環境不用傳，減少長度
            if (EnvCode != EnvironmentCode.Production)
            {
                lunchGameRequestModel.EnvCode = EnvCode.Value;
            }

            DESTool tool = new DESTool(_appSettingService.CommonDataHash);
            operator_player_session = tool.DESEnCode(lunchGameRequestModel.ToJsonString());

            //組網址
            string queryString = string.Format("?operator_token={0}&operator_player_session={1}&bet_type=1&language=zh", 
                AppSettings.OperatorToken, HttpUtility.UrlEncode(operator_player_session));

            if (!isMobile)
            {
                url = GetFullUrl(AppSettings.LunchGameUrl, AppSettings.LunchGameUrlPath + queryString);
            }
            else
            {
                url = GetFullUrl(AppSettings.LunchMobileGameUrl, AppSettings.LunchMobileGameUrlPath + queryString);
            }

            return new BaseReturnDataModel<string>(ReturnCode.Success, url);
        }

        /// <summary>
        /// 打第三方API
        /// </summary>
        private BaseReturnDataModel<string> DoPostRequest(string relativeUrl, string requestBody)
        {
            return DoPostRequest(AppSettings.ServiceUrl, AppSettings.UrlPathPrefix + relativeUrl, requestBody);
        }

        /// <summary>
        /// 打第三方API，加密簽名方法
        /// </summary>
        private BaseReturnDataModel<string> DoPostRequest(string url, string relativeUrl, string requestBody)
        {
            url = GetFullUrl(url, relativeUrl);

            string queryString = requestBody + "&operator_token=" + AppSettings.OperatorToken + "&secret_key=" + AppSettings.SecretKey;

            string result = HttpWebRequestUtil.GetResponse(
                new WebRequestParam()
                {
                    Purpose = $"TPGService.{Product.Value}請求",
                    Method = HttpMethod.Post,
                    Url = url,
                    Body = queryString,
                    ContentType = HttpWebRequestContentType.WwwFormUrlencoded,
                    IsResponseValidJson = true
                },
            out HttpStatusCode httpStatusCode);

            return new BaseReturnDataModel<string>()
            {
                IsSuccess = httpStatusCode == HttpStatusCode.OK,
                DataModel = result
            };
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(string tpGameAccount)
        {
            //無此API
            throw new NotSupportedException();
        }

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam param)
        {
            var request = new PGRegisterRequestModel
            {
                player_name = param.TPGameAccount,
                nickname = param.UserName,
                currency = AppSettings.Currncy
            };

            return DoPostRequest(AppSettings.CreateAccountUrl, request.ToKeyValueURL());
        }
    }
}
