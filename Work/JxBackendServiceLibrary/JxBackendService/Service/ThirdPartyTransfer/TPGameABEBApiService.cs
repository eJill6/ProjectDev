using JxBackendService.Common;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.AllBet;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Net;
using System.Security.Cryptography;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameABEBApiService : BaseTPGameApiService
    {
        public static readonly int MaxSearchRangeMinutes = 60;

        public ABEBSharedAppSetting AppSettings => ABEBSharedAppSetting.Instance;

        public override PlatformProduct Product => PlatformProduct.ABEB;

        protected override int? TransferAmountFloorDigit => 2;

        public TPGameABEBApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        /// <summary>
        /// 轉帳
        /// </summary>
        protected override DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new ABTransferRequestModel
            {
                client = tpGameAccount,
                credit = tpGameMoneyInfo.Amount.ToString("0.####"),
                operFlag = isMoneyIn ? 1 : 0,
                sn = tpGameMoneyInfo.OrderID
            };

            DoPostRequest(AppSettings.TransferUrl, request.ToKeyValueURL(), out DetailRequestAndResponse detail);

            return detail;
        }

        /// <summary>
        /// 查詢訂單
        /// </summary>
        protected override DetailRequestAndResponse GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new ABCheckTransferRequestModel
            {
                sn = tpGameMoneyInfo.OrderID
            };

            DoPostRequest(AppSettings.CheckTransferUrl, request.ToKeyValueURL(), out DetailRequestAndResponse detail);

            return detail;
        }

        /// <summary>
        /// 查詢餘額
        /// </summary>
        protected override string GetRemoteUserScoreApiResult(string tpGameAccount)
        {
            var request = new ABRegisterRequestModel
            {
                client = tpGameAccount,
            };

            request = SetPassword(request);

            BaseReturnDataModel<string> returnModel = DoPostRequest(AppSettings.GetBalanceUrl, request.ToKeyValueURL(), out DetailRequestAndResponse detail);

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

            var request = new ABBetLogRequestModel
            {
                startTime = searchStartDate.ToFormatDateTimeString(),
                endTime = searchEndDate.ToFormatDateTimeString()
            };

            string requestBody = request.ToKeyValueURL();

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
            ABTransferResponseModel transferModel = apiResult.Deserialize<ABTransferResponseModel>();

            if (transferModel.error_code == ABResponseCode.Success)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success);
            }

            return new BaseReturnDataModel<UserScore>(transferModel.message, null);
        }

        /// <summary>
        /// 處理查詢訂單回傳結果
        /// </summary>
        public override BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            ABQueryOrderResponseModel transferModel = apiResult.Deserialize<ABQueryOrderResponseModel>();

            if (transferModel.error_code == ABResponseCode.Success && transferModel.transferState == ABTransferOrderStatus.Success.Value)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success);
            }

            return new BaseReturnDataModel<UserScore>($"error:{transferModel.message}", null);
        }

        /// <summary>
        /// 處理取得餘額回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            ABBalanceResponseModel userScoreModel = apiResult.Deserialize<ABBalanceResponseModel>();

            if (userScoreModel.error_code == ABResponseCode.Success)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success, new UserScore() { AvailableScores = userScoreModel.balance });
            }

            return new BaseReturnDataModel<UserScore>(userScoreModel.message, null);
        }

        /// <summary>
        /// 檢查以及創建帳號
        /// </summary>
        protected override BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam param)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteCreateAccountApiResult(param);

            if (returnModel.IsSuccess)
            {
                ABRegisterResponseModel registerModel = returnModel.DataModel.Deserialize<ABRegisterResponseModel>();
                //檢查帳號重複同一隻API
                if (registerModel.error_code == ABResponseCode.Success || registerModel.error_code == ABResponseCode.AccountExist)
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                return new BaseReturnModel(registerModel.message);
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
                return new BaseReturnDataModel<string>(returnModel.Message, string.Empty);
            }

            ABLunchGameResponseModel launchGameModel = returnModel.DataModel.Deserialize<ABLunchGameResponseModel>();

            if (launchGameModel.error_code == ABResponseCode.Success)
            {
                return new BaseReturnDataModel<string>(ReturnCode.Success, launchGameModel.gameLoginUrl);
            }

            return new BaseReturnDataModel<string>(launchGameModel.message, string.Empty);
        }

        protected override BaseReturnDataModel<string> GetRemoteLoginApiResult(string tpGameAccount, string ipAddress, bool isMobile, LoginInfo loginInfo)
        {
            var request = new ABUserBaseWithPasswordRequestModel
            {
                client = tpGameAccount,
            };

            request = SetPassword(request);

            string url = AppSettings.LaunchGameUrl;

            if (isMobile)
            {
                request = new ABMobileLunchGameRequestModel
                {
                    client = tpGameAccount,
                    appType = 3
                };

                request = SetPassword(request);
            }

            BaseReturnDataModel<string> returnModel = DoPostRequest(url, request.ToKeyValueURL(), out DetailRequestAndResponse detail);

            return returnModel;
        }

        /// <summary>
        /// 打第三方API，加密簽名方法
        /// </summary>
        private BaseReturnDataModel<string> DoPostRequest(string relativeUrl, string requestBody, out DetailRequestAndResponse detail)
        {
            string url = GetFullUrl(AppSettings.ServiceUrl, relativeUrl);

            requestBody += "&random=" + Random() + "&agent=" + AppSettings.AgentUserName;
            string data = TripleDESTool.Encrypt(requestBody, AppSettings.DesKey, null);
            string stingToSign = data + AppSettings.MD5Key;
            string sign = MD5Tool.Base64edMd5(stingToSign);
            string queryString = "propertyId=" + AppSettings.PropertyId + "&data=" + System.Web.HttpUtility.UrlEncode(data) + "&sign=" + System.Web.HttpUtility.UrlEncode(sign);

            var httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>();

            var webRequestParam = new WebRequestParam()
            {
                Purpose = $"TPGService.{Product.Value}請求",
                Method = HttpMethod.Post,
                Url = url,
                Body = queryString,
                ContentType = HttpWebRequestContentType.WwwFormUrlencoded,
                IsResponseValidJson = true
            };

            string apiResult = httpWebRequestUtilService.GetResponse(webRequestParam, out HttpStatusCode httpStatusCode);
            detail = new DetailRequestAndResponse(webRequestParam, apiResult);

            return ConverToApiReturnDataModel(httpStatusCode, apiResult);
        }

        /// <summary>
        /// 打第三方API用亂數
        /// </summary>
        private static string Random()
        {
            RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider();
            byte[] byteCsp = new byte[5];
            csp.GetBytes(byteCsp);
            return BitConverter.ToString(byteCsp);
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(string tpGameAccount)
        {
            //無此API
            throw new NotSupportedException();
        }

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam param)
        {
            var request = new ABRegisterRequestModel
            {
                client = param.TPGameAccount,
            };

            request = SetPassword(request);

            return DoPostRequest(AppSettings.CreateAccountUrl, request.ToKeyValueURL(), out DetailRequestAndResponse detail);
        }

        private T SetPassword<T>(T request) where T : ABUserBaseWithPasswordRequestModel
        {
            string account = request.client;

            int maxLength = 12; //ABEB密碼長度限制為12
            request.password = account.Substring(0, Math.Min(account.Length, maxLength));

            return request;
        }
    }
}