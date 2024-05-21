using JxBackendService.Common;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.AllBet;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameABEBApiService : BaseTPGameApiService
    {
        public static readonly int MaxSearchRangeMinutes = 60;

        public override PlatformProduct Product => PlatformProduct.ABEB;

        protected override int? TransferAmountFloorDigit => 2;

        public TPGameABEBApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        /// <summary>
        /// 轉帳
        /// </summary>
        public override DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, CreateRemoteAccountParam createRemoteAccountParam,
            BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new ABTransferRequestModel
            {
                Sn = tpGameMoneyInfo.OrderID,
                Player = createRemoteAccountParam.TPGameAccount,
                Amount = tpGameMoneyInfo.Amount.ToString("0.####"),
                Type = isMoneyIn ? 1 : 0,
            };

            DoPostRequest(ABEBSharedAppSetting.TransferUrl, request.ToJsonString(isCamelCaseNaming: true), out DetailRequestAndResponse detail);

            return detail;
        }

        /// <summary>
        /// 查詢訂單
        /// </summary>
        protected override DetailRequestAndResponse GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new ABCheckTransferRequestModel
            {
                Sn = tpGameMoneyInfo.OrderID
            };

            DoPostRequest(ABEBSharedAppSetting.CheckTransferUrl, request.ToJsonString(isCamelCaseNaming: true), out DetailRequestAndResponse detail);

            return detail;
        }

        /// <summary>
        /// 查詢餘額
        /// </summary>
        public override string GetRemoteUserScoreApiResult(CreateRemoteAccountParam createRemoteAccountParam, IInvocationUserParam invocationUserParam)
        {
            var request = new ABUserScoreRequestModel
            {
                Players = new List<string> { createRemoteAccountParam.TPGameAccount },
            };

            BaseReturnDataModel<string> returnModel = DoPostRequest(ABEBSharedAppSetting.GetBalanceUrl,
                request.ToJsonString(isCamelCaseNaming: true), out DetailRequestAndResponse detail);

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

            int totalPageCount = 1; //不知道有多少頁 先預設1
            var requestBodys = new List<string>();
            var responseContents = new List<string>();

            for (int pageIndex = 1; pageIndex <= totalPageCount; pageIndex++)
            {
                // 1分钟30次
                if (pageIndex > 1)
                {
                    TaskUtil.DelayAndWait(2000);
                }

                var requestData = new ABBetLogRequestModel
                {
                    StartDateTime = searchStartDate.ToFormatDateTimeString(),
                    EndDateTime = searchEndDate.ToFormatDateTimeString(),
                    PageNum = pageIndex,
                };

                string requestBody = requestData.ToJsonString(isCamelCaseNaming: true);

                BaseReturnDataModel<string> returnDataModel = DoPostRequest(ABEBSharedAppSetting.GetBetLogUrl, requestBody, out DetailRequestAndResponse detail);

                if (!returnDataModel.IsSuccess)
                {
                    return new BaseReturnDataModel<RequestAndResponse>(returnDataModel.Message, new RequestAndResponse()
                    {
                        RequestBody = requestData.ToJsonString(),
                        ResponseContent = returnDataModel.DataModel
                    });
                }

                var queryBetListResponse = returnDataModel.DataModel.Deserialize<ABBetLogResponseModel>();

                //確保全部分頁的request都要成功
                if (!queryBetListResponse.IsSuccess || queryBetListResponse.Data == null)
                {
                    return new BaseReturnDataModel<RequestAndResponse>(queryBetListResponse.Message, new RequestAndResponse()
                    {
                        RequestBody = requestData.ToJsonString(),
                        ResponseContent = returnDataModel.DataModel
                    });
                }

                requestBodys.Add(requestData.ToJsonString());
                responseContents.Add(returnDataModel.DataModel);

                if (pageIndex == 1)
                {
                    int totalPage = queryBetListResponse.Data.Total / queryBetListResponse.Data.PageSize;
                    int remainder = queryBetListResponse.Data.Total % queryBetListResponse.Data.PageSize;

                    if (remainder != 0)
                    {
                        totalPage++;
                    }

                    //校正頁數
                    totalPageCount = totalPage;
                }
            }

            return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.Success,
                new RequestAndResponse()
                {
                    RequestBody = requestBodys.ToJsonString(),
                    ResponseContent = responseContents.ToJsonString()
                });
        }

        /// <summary>
        /// 處理轉帳回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetTransferReturnModel(string apiResult)
        {
            ABTransferResponseModel transferModel = apiResult.Deserialize<ABTransferResponseModel>();

            if (transferModel.IsSuccess)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success);
            }

            return new BaseReturnDataModel<UserScore>(transferModel.Message);
        }

        /// <summary>
        /// 處理查詢訂單回傳結果
        /// </summary>
        public override BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            var responseModel = apiResult.Deserialize<ABCheckTransferResponseModel>();

            if (responseModel.IsSuccess &&
                responseModel.Data.TransferState == ABTransferStatus.Success.Value)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            if (responseModel.IsSuccess &&
                responseModel.Data != null &&
                responseModel.Data.TransferState != ABTransferStatus.Success.Value)
            {
                string transferStateMessage = ABTransferStatus.CombineMessage(responseModel.Data.TransferState);

                return new BaseReturnModel(transferStateMessage);
            }

            string errorMessage = ABResponseCode.CombineMessage(responseModel.ResultCode, responseModel.Message);

            return new BaseReturnModel(errorMessage);
        }

        /// <summary>
        /// 處理取得餘額回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            ABBalanceResponseModel userScoreModel = apiResult.Deserialize<ABBalanceResponseModel>();

            if (userScoreModel.IsSuccess)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success,
                    new UserScore() { AvailableScores = userScoreModel.Data.List[0].Amount.ToDecimal() });
            }

            return new BaseReturnDataModel<UserScore>(userScoreModel.Message);
        }

        /// <summary>
        /// 檢查以及創建帳號
        /// </summary>
        protected override BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam createRemoteAccountParam)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteCreateAccountApiResult(createRemoteAccountParam);

            if (returnModel.IsSuccess)
            {
                ABRegisterResponseModel registerModel = returnModel.DataModel.Deserialize<ABRegisterResponseModel>();

                //檢查帳號重複同一隻API
                if (registerModel.IsSuccess || registerModel.ResultCode == ABResponseCode.PlayerExist)
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                return new BaseReturnModel(registerModel.Message);
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
                return new BaseReturnDataModel<string>(returnModel.Message);
            }

            ABLunchGameResponseModel launchGameModel = returnModel.DataModel.Deserialize<ABLunchGameResponseModel>();

            if (launchGameModel.IsSuccess)
            {
                return new BaseReturnDataModel<string>(ReturnCode.Success, launchGameModel.Data.GameLoginUrl);
            }

            return new BaseReturnDataModel<string>(launchGameModel.Message);
        }

        protected override BaseReturnDataModel<string> GetRemoteLoginApiResult(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            var request = new ABLunchGameRequestModel
            {
                Player = tpGameRemoteLoginParam.CreateRemoteAccountParam.TPGameAccount,
            };

            string url = ABEBSharedAppSetting.LaunchGameUrl;

            BaseReturnDataModel<string> returnModel = DoPostRequest(url, request.ToJsonString(isCamelCaseNaming: true), out DetailRequestAndResponse detail);

            return returnModel;
        }

        protected override void DoKickUser(Model.Entity.ThirdPartyUserAccount thirdPartyUserAccount) => new NotImplementedException();

        /// <summary>
        /// 打第三方API，加密簽名方法
        /// </summary>
        private BaseReturnDataModel<string> DoPostRequest(string relativeUrl, string requestBody, out DetailRequestAndResponse detail)
        {
            string fullUrl = GetFullUrl(ABEBSharedAppSetting.ServiceUrl, relativeUrl);
            string contentMD5 = MD5Tool.Base64edMd5(requestBody);
            string requestTime = DateTime.Now.ToString("dd MMM yyyy HH:mm:ss z", new CultureInfo("en-US"));

            var method = System.Net.Http.HttpMethod.Post;
            string stringToSign = $"{method}\n{contentMD5}\n{HttpWebRequestContentType.Json}\n{requestTime}\n/{relativeUrl}";

            var hmacsha1 = new HMACSHA1
            {
                Key = Convert.FromBase64String(ABEBSharedAppSetting.ApiKey)
            };

            byte[] hashBytes = hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(stringToSign));
            string encrypted = Convert.ToBase64String(hashBytes);

            var httpRequestMessage = new HttpRequestMessage
            {
                Content = new StringContent(requestBody),
                Method = method,
                RequestUri = new Uri(fullUrl),
            };

            httpRequestMessage.Headers.TryAddWithoutValidation("Authorization", $"AB {ABEBSharedAppSetting.OperatorId}:{encrypted}");
            httpRequestMessage.Headers.TryAddWithoutValidation("Date", requestTime);

            httpRequestMessage.Content.Headers.TryAddWithoutValidation("Content-MD5", contentMD5);
            httpRequestMessage.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //Send the Http request
            var client = new HttpClient();
            HttpResponseMessage response = client.SendAsync(httpRequestMessage).Result;
            string apiResult = response.Content.ReadAsStringAsync().Result;

            detail = new DetailRequestAndResponse()
            {
                RequestUrl = fullUrl,
                RequestHeader = httpRequestMessage.Headers.ToJsonString(),
                RequestBody = requestBody,
                ResponseContent = apiResult
            };

            return ConvertToApiReturnDataModel(response.StatusCode, apiResult);
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            //無此API
            throw new NotSupportedException();
        }

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            var request = new ABRegisterRequestModel
            {
                Player = createRemoteAccountParam.TPGameAccount,
            };

            return DoPostRequest(ABEBSharedAppSetting.CreateAccountUrl, request.ToJsonString(isCamelCaseNaming: true), out DetailRequestAndResponse detail);
        }

        protected override string CreateTPGamePasswordByRule(int userId, string tpGameAccount) => null;
    }
}