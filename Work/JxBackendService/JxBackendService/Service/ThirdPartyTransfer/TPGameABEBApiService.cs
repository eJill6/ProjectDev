using JxBackendService.Common;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.ThirdParty;
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
    public class TPGameABEBApiService : BaseTPGameApiService
    {
        public static readonly int MaxSearchRangeMinutes = 60;

        public ABEBSharedAppSettings AppSettings => ABEBSharedAppSettings.Instance;

        public override PlatformProduct Product => PlatformProduct.ABEB;

        public TPGameABEBApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {

        }

        /// <summary>
        /// 轉帳
        /// </summary>
        protected override string GetRemoteTransferApiResult(bool isMoneyIn, string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new ABTransferRequestModel
            {
                client = tpGameAccount,
                credit = Math.Round(tpGameMoneyInfo.Amount, 2),
                operFlag = isMoneyIn ? 1 : 0,
                sn = tpGameMoneyInfo.OrderID
            };
            BaseReturnDataModel<string> returnModel = DoPostRequest(AppSettings.TransferUrl, request.ToKeyValueURL());
            return returnModel.DataModel;
        }

        /// <summary>
        /// 查詢訂單
        /// </summary>
        protected override string GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new ABCheckTransferRequestModel
            {
                sn = tpGameMoneyInfo.OrderID
            };

            BaseReturnDataModel<string> returnModel = DoPostRequest(AppSettings.CheckTransferUrl, request.ToKeyValueURL());
            return returnModel.DataModel;
        }

        /// <summary>
        /// 查詢餘額
        /// </summary>
        protected override string GetRemoteUserScoreApiResult(string tpGameAccount)
        {
            var request = new ABRegisterRequestModel
            {
                client = tpGameAccount,
                password = tpGameAccount
            };

            var returnModel = DoPostRequest(AppSettings.GetBalanceUrl, request.ToKeyValueURL());
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

            BaseReturnDataModel<string> returnModel = DoPostRequest(AppSettings.GetBetLogUrl, requestBody);
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

            return new BaseReturnDataModel<UserScore>(transferModel.message, null);
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
        protected override BaseReturnDataModel<string> GetRemoteForwardGameUrl(string tpGameAccount, string ip, bool isMobile)
        {
            var request = new ABUserBaseWithPasswrodRequestModel
            {
                client = tpGameAccount,
                password = tpGameAccount
            };
            string url = AppSettings.LaunchGameUrl;
            if (isMobile)
            {
                request = new ABMobileLunchGameRequestModel
                {
                    client = tpGameAccount,
                    password = tpGameAccount,
                    appType = 3
                };
            }

            var returnModel = DoPostRequest(url, request.ToKeyValueURL());
            if (returnModel.IsSuccess)
            {
                ABLunchGameResponseModel launchGameModel = returnModel.DataModel.Deserialize<ABLunchGameResponseModel>();

                if (launchGameModel.error_code == ABResponseCode.Success)
                {
                    return new BaseReturnDataModel<string>(ReturnCode.Success, launchGameModel.gameLoginUrl);
                }

                return new BaseReturnDataModel<string>(launchGameModel.message, string.Empty);
            }

            return new BaseReturnDataModel<string>(returnModel.Message, string.Empty);
        }

        /// <summary>
        /// 打第三方API
        /// </summary>
        private BaseReturnDataModel<string> DoPostRequest(string relativeUrl, string requestBody)
        {
            return DoPostRequest(AppSettings.ServiceUrl, relativeUrl, requestBody);
        }

        /// <summary>
        /// 打第三方API，加密簽名方法
        /// </summary>
        private BaseReturnDataModel<string> DoPostRequest(string url, string relativeUrl, string requestBody)
        {
            url = GetFullUrl(url, relativeUrl);

            requestBody += "&random=" + Random() + "&agent=" + AppSettings.AgentUserName;
            string data = TripleDESTool.Encrypt(requestBody, AppSettings.DesKey, null);
            string stingToSign = data + AppSettings.MD5Key;
            string sign = MD5Tool.Base64edMd5(stingToSign);
            string queryString = "propertyId=" + AppSettings.PropertyId + "&data=" + System.Web.HttpUtility.UrlEncode(data) + "&sign=" + System.Web.HttpUtility.UrlEncode(sign);

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
                password = param.TPGameAccount
            };

            return DoPostRequest(AppSettings.CreateAccountUrl, request.ToKeyValueURL());
        }
    }
}
