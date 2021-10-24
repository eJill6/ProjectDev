using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.OB.OBFI;
using JxBackendService.Model.ThirdParty.OB.OBSP;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Web;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public class TPGameOBSPApiService : BaseTPGameApiService
    {
        public static readonly int MaxSearchRangeMinutes = 60;
        private readonly int _searchBetLogPageSize = 100;

        public override PlatformProduct Product => PlatformProduct.OBSP;

        public TPGameOBSPApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {

        }

        /// <summary>
        /// 轉帳
        /// </summary>
        protected override string GetRemoteTransferApiResult(bool isMoneyIn, string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            OBSPTransferType transferType;

            if (isMoneyIn)
            {
                transferType = OBSPTransferType.Deposit;
            }
            else
            {
                transferType = OBSPTransferType.Withdraw;
            }

            var request = new OBSPCreateTransferRequest()
            {
                MerchantCode = OBSPSharedAppSettings.MerchantCode,
                Key = OBSPSharedAppSettings.Key,
                TransferId = tpGameMoneyInfo.OrderID,
                UserName = tpGameAccount,
                TransferType = transferType,
                Amount = tpGameMoneyInfo.Amount.ToString()
            };

            string requestBody = request.ToRequestBody();
            BaseReturnDataModel<string> returnModel = DoPostRequest(OBSPSharedAppSettings.FundTransferActionUrl, requestBody);
            return returnModel.DataModel;
        }

        /// <summary>
        /// 查詢訂單
        /// </summary>
        protected override string GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new OBSPGetTransferRecordRequest()
            {
                MerchantCode = OBSPSharedAppSettings.MerchantCode,
                Key = OBSPSharedAppSettings.Key,
                TransferId = tpGameMoneyInfo.OrderID,
                UserName = tpGameAccount
            };

            string requestBody = request.ToRequestBody();
            BaseReturnDataModel<string> returnModel = DoPostRequest(OBSPSharedAppSettings.FundGetTransferRecordActionUrl, requestBody);
            return returnModel.DataModel;
        }

        /// <summary>
        /// 查詢餘額
        /// </summary>
        protected override string GetRemoteUserScoreApiResult(string tpGameAccount)
        {
            var request = new OBSPUserBasicRequest()
            {
                MerchantCode = OBSPSharedAppSettings.MerchantCode,
                Key = OBSPSharedAppSettings.Key,
                UserName = tpGameAccount,
            };

            string requestBody = request.ToRequestBody();
            BaseReturnDataModel<string> returnModel = DoPostRequest(OBSPSharedAppSettings.FundCheckBalanceActionUrl, requestBody);
            return returnModel.DataModel;
        }

        /// <summary>
        /// 取得盈虧資料
        /// </summary>
        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            //有分頁機制
            DateTime maxSearchEndDate = DateTime.Now;
            DateTime searchStartDate = DateTime.Now.AddMinutes(-MaxSearchRangeMinutes);

            if (!lastSearchToken.IsNullOrEmpty())
            {
                searchStartDate = DateTime.Parse(lastSearchToken);
            }

            DateTime searchEndDate = searchStartDate.AddMinutes(MaxSearchRangeMinutes);

            if (searchEndDate > maxSearchEndDate)
            {
                searchEndDate = maxSearchEndDate;
            }

            if (searchEndDate < searchStartDate)
            {
                searchEndDate = searchStartDate;
            }

            int totalPageCount = 1; //不知道有多少頁 先預設1
            var requestBodys = new List<string>();
            var responseContents = new List<string>();

            for (int i = 1; i <= totalPageCount; i++)
            {
                if (i > 1)
                {
                    Thread.Sleep(3000);
                }

                //取得Data Api Token
                var request = new OBSPGQueryBetListRequest()
                {
                    MerchantCode = OBSPSharedAppSettings.MerchantCode,
                    Key = OBSPSharedAppSettings.Key,
                    StartTime = searchStartDate.ToUnixOfTime().ToString(),
                    EndTime = searchEndDate.ToUnixOfTime().ToString(),
                    PageNum = i,
                    PageSize = _searchBetLogPageSize
                };

                string requestBody = request.ToRequestBody();
                BaseReturnDataModel<string> returnDataModel = DoPostRequest(OBSPSharedAppSettings.BetQueryBetListActionUrl, requestBody);
                var queryBetListResponse = returnDataModel.DataModel.Deserialize<OBSPApiResponse<OBSPQueryBetListData>>();

                //確保全部分頁的request都要成功
                if (!queryBetListResponse.Status || queryBetListResponse.Code != OBSPReturnCode.Success)
                {
                    return new BaseReturnDataModel<RequestAndResponse>(queryBetListResponse.Msg, new RequestAndResponse()
                    {
                        RequestBody = requestBody,
                        ResponseContent = returnDataModel.DataModel
                    });
                }

                requestBodys.Add(requestBody);
                responseContents.Add(returnDataModel.DataModel);

                if (i == 1)
                {
                    //校正頁數
                    totalPageCount = (int)Math.Ceiling((double)queryBetListResponse.Data.TotalCount / request.PageSize);
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
            var apiResponseModel = apiResult.Deserialize<OBSPApiResponse>();

            if (apiResponseModel.IsSuccess)
            {
                return new BaseReturnDataModel<UserScore>();
            }
            else
            {
                return new BaseReturnDataModel<UserScore>(apiResponseModel.Msg, null);
            }
        }

        /// <summary>
        /// 處理查詢訂單回傳結果
        /// </summary>
        public override BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            var apiResponseModel = apiResult.Deserialize<OBSPApiResponse<GetTransferRecordData>>();

            if (apiResponseModel.IsSuccess)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success);
            }

            string errorMsg = apiResponseModel.Msg;

            if (apiResponseModel.Data != null)
            {
                OBSPOrderStatus orderStatus = OBSPOrderStatus.GetSingle(apiResponseModel.Data.Status);


                if (orderStatus == null)
                {
                    errorMsg = $"orderStatus={apiResponseModel.Data.Status}";
                }
                else
                {
                    errorMsg = orderStatus.Name;
                }
            }

            return new BaseReturnModel(errorMsg);
        }

        /// <summary>
        /// 處理取得餘額回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            var apiResponseModel = apiResult.Deserialize<OBSPApiResponse<CheckBalanceData>>();

            if (apiResponseModel.IsSuccess)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success,
                    new UserScore() { AvailableScores = apiResponseModel.Data.Balance });
            }

            return new BaseReturnDataModel<UserScore>(apiResponseModel.Msg, null);
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
                OBSPApiResponse<CreateUserData> apiResponseModel = returnModel.DataModel.Deserialize<OBSPApiResponse<CreateUserData>>();

                if (apiResponseModel.IsSuccess)
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                return new BaseReturnModel(apiResponseModel.Msg);
            }

            return new BaseReturnModel(returnModel.Message);
        }

        /// <summary>
        /// 取得遊戲入口網址
        /// </summary>
        protected override BaseReturnDataModel<string> GetRemoteForwardGameUrl(string tpGameAccount, string ip, bool isMobile)
        {
            OBSPTerminalType terminalType = (isMobile) ? OBSPTerminalType.Mobile : OBSPTerminalType.PC;

            var request = new OBSPLoginUserRequest()
            {
                MerchantCode = OBSPSharedAppSettings.MerchantCode,
                Key = OBSPSharedAppSettings.Key,
                Currency = OBSPSharedAppSettings.Currency,
                UserName = tpGameAccount,
                Terminal = terminalType.Value
            };

            string requestBody = request.ToRequestBody();
            BaseReturnDataModel<string> returnModel = DoPostRequest(OBSPSharedAppSettings.UserLoginActionUrl, requestBody);

            if (returnModel.IsSuccess)
            {
                OBSPApiResponse<UserLoginData> apiResponseModel = returnModel.DataModel.Deserialize<OBSPApiResponse<UserLoginData>>();

                //檢查帳號重複同一隻API
                if (apiResponseModel.IsSuccess)
                {
                    //組網址
                    string url = string.Format("{0}?token={1}",
                                               apiResponseModel.Data.Domain,
                                               HttpUtility.UrlEncode(apiResponseModel.Data.Token));

                    return new BaseReturnDataModel<string>(ReturnCode.Success, url);
                }

                return new BaseReturnDataModel<string>(apiResponseModel.Msg, string.Empty);
            }

            return new BaseReturnDataModel<string>(returnModel.Message, string.Empty);
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(string tpGameAccount)
        {
            var request = new OBSPUserBasicRequest()
            {
                MerchantCode = OBSPSharedAppSettings.MerchantCode,
                Key = OBSPSharedAppSettings.Key,
                UserName = tpGameAccount,
            };

            string requestBody = request.ToRequestBody();
            return DoPostRequest(OBSPSharedAppSettings.FundCheckBalanceActionUrl, requestBody);
        }

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam param)
        {
            var request = new OBSPCreateUserRequest()
            {
                MerchantCode = OBSPSharedAppSettings.MerchantCode,
                Key = OBSPSharedAppSettings.Key,
                Currency = OBSPSharedAppSettings.Currency,
                UserName = param.TPGameAccount,
            };

            string requestBody = request.ToRequestBody();
            return DoPostRequest(OBSPSharedAppSettings.UserCreateActionUrl, requestBody);
        }

        /// <summary>
        /// 檢查帳號
        /// </summary>
        private BaseReturnDataModel<bool> CheckRemoteAccountExist(string tpGameAccount)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteCheckAccountExistApiResult(tpGameAccount);

            if (returnModel.IsSuccess)
            {
                OBSPApiResponse<CheckBalanceData> apiResponseModel = returnModel.DataModel.Deserialize<OBSPApiResponse<CheckBalanceData>>();

                //檢查帳號重複同一隻API
                if (apiResponseModel.IsSuccess)
                {
                    return new BaseReturnDataModel<bool>(ReturnCode.Success, true);
                }
                else if (apiResponseModel.Code == OBSPReturnCode.UserIsNotExist)
                {
                    return new BaseReturnDataModel<bool>(apiResponseModel.Msg, false);
                }

                throw new Exception(apiResponseModel.Msg);
            }

            throw new Exception(returnModel.Message);
        }

        private BaseReturnDataModel<string> DoPostRequest(string actionUrl, string requestBody)
        {
            string fullUrl = GetCombineUrl(OBSPSharedAppSettings.ApiRootUrl, actionUrl);

            string result = HttpWebRequestUtil.GetResponse(
                new WebRequestParam()
                {
                    Purpose = $"TPGService {Product} API",
                    Method = HttpMethod.Post,
                    Url = fullUrl,
                    Body = requestBody,
                    ContentType = HttpWebRequestContentType.WwwFormUrlencoded,
                    IsResponseValidJson = true,
                },
            out HttpStatusCode httpStatusCode);

            return new BaseReturnDataModel<string>()
            {
                IsSuccess = httpStatusCode == HttpStatusCode.OK,
                DataModel = result
            };
        }
    }
}
