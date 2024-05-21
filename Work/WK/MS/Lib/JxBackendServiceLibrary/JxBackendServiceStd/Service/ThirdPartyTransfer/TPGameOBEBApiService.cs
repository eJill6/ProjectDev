using JxBackendService.Common;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.ThirdPartyTransfer.OBEB;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.OB.OBEB;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Collections.Generic;
using System.Net;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameOBEBApiService : BaseTPGameApiService, ITPGameOBEBApiService
    {
        public static readonly int MaxSearchRangeMinutes = 30;

        public static readonly int BackSqliteSearchDateMinutes = 20;

        public override PlatformProduct Product => PlatformProduct.OBEB;

        public TPGameOBEBApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public OBEBBaseResponseWtihDataModel<List<OBEBAnchor>> GetAnchorsResult()
        {
            BaseReturnDataModel<string> returnDataModel = GetRemoteAnchorListApiResult();

            if (!returnDataModel.IsSuccess)
            {
                //臨時記錄日志排查問題
                LogUtilService.ForcedDebug($"{Product} GetAnchorsResult 請求結果：{returnDataModel.ToJsonString()}");

                return null;
            }

            var anchorsResult = returnDataModel.DataModel.Deserialize<OBEBBaseResponseWtihDataModel<List<OBEBAnchor>>>();

            if (!anchorsResult.data.AnyAndNotNull())
            {
                //臨時記錄日志排查問題
                LogUtilService.ForcedDebug($"{Product} GetAnchorsResult 請求結果：{returnDataModel.ToJsonString()}");
            }

            return anchorsResult;
        }

        public OBEBBaseResponseModel ForeLeaveTable(string tpGameAccount)
        {
            BaseReturnDataModel<string> returnDataModel = GetRemoteForeLeaveTableApiResult(tpGameAccount);

            if (!returnDataModel.IsSuccess)
            {
                return null;
            }

            return returnDataModel.DataModel.Deserialize<OBEBBaseResponseModel>();
        }

        /// <summary> 会员离桌 </summary>
        private BaseReturnDataModel<string> GetRemoteForeLeaveTableApiResult(string tpGameAccount)
        {
            var request = new OBEBBaseRequestModel
            {
                loginName = tpGameAccount
            };

            return DoPostRequest(OBEBSharedAppSetting.ForeLeaveTable, request, out DetailRequestAndResponse detail);
        }

        private BaseReturnDataModel<string> GetRemoteAnchorListApiResult()
        {
            var request = new OBEBAnchorRequestModel
            {
                Timestamp = DateTime.Now.ToUnixOfTime()
            };

            return DoDataPostRequest(OBEBSharedAppSetting.LivesUrl, request, request.PageIndex);
        }

        /// <summary> 轉帳(充值/提現) </summary>
        public override DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, CreateRemoteAccountParam createRemoteAccountParam, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var requestData = new OBEBTransferRequestModel
            {
                loginName = createRemoteAccountParam.TPGameAccount,
                amount = tpGameMoneyInfo.Amount.Floor(2),
                transferNo = tpGameMoneyInfo.OrderID
            };

            string postUrl = string.Empty;

            if (isMoneyIn)
            {
                postUrl = OBEBSharedAppSetting.DepositUrl;
            }
            else
            {
                postUrl = OBEBSharedAppSetting.WithdrawUrl;
            }

            DoPostRequest(postUrl, requestData, out DetailRequestAndResponse detail);

            return detail;
        }

        /// <summary> 查詢訂單 </summary>
        protected override DetailRequestAndResponse GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var requestData = new OBEBCheckTransferRequestModel
            {
                transferNo = tpGameMoneyInfo.OrderID,
                loginName = tpGameAccount
            };

            DoPostRequest(OBEBSharedAppSetting.CheckTransferUrl, requestData, out DetailRequestAndResponse detail);

            return detail;
        }

        /// <summary> 查詢餘額 </summary>
        public override string GetRemoteUserScoreApiResult(CreateRemoteAccountParam createRemoteAccountParam, IInvocationUserParam invocationUserParam)
        {
            var requestData = new OBEBBaseRequestModel
            {
                loginName = createRemoteAccountParam.TPGameAccount
            };

            BaseReturnDataModel<string> returnModel = DoPostRequest(OBEBSharedAppSetting.GetBalanceUrl, requestData, out DetailRequestAndResponse detail);

            return returnModel.DataModel;
        }

        /// <summary> 取得盈虧資料 </summary>
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

            for (int pageIndex = 1; pageIndex <= totalPageCount; pageIndex++)
            {
                // 10秒/4次
                if (pageIndex > 1)
                {
                    TaskUtil.DelayAndWait(400);
                }

                var requestData = new OBEBDataBaseRequestModel
                {
                    startTime = searchStartDate.ToFormatDateTimeString(),
                    endTime = searchEndDate.ToFormatDateTimeString(),
                    pageIndex = pageIndex,
                };

                BaseReturnDataModel<string> returnDataModel = DoDataPostRequest(OBEBSharedAppSetting.BetHistoryRecordUrl, requestData, pageIndex);
                var queryBetListResponse = returnDataModel.DataModel.Deserialize<OBEBBetLogResponseModel>();

                //確保全部分頁的request都要成功
                if (!queryBetListResponse.IsSuccess || queryBetListResponse.data == null)
                {
                    return new BaseReturnDataModel<RequestAndResponse>(queryBetListResponse.message, new RequestAndResponse()
                    {
                        RequestBody = requestData.ToJsonString(),
                        ResponseContent = returnDataModel.DataModel
                    });
                }

                requestBodys.Add(requestData.ToJsonString());
                responseContents.Add(returnDataModel.DataModel);

                if (pageIndex == 1)
                {
                    //校正頁數
                    totalPageCount = queryBetListResponse.data.totalPage;
                }
            }

            return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.Success,
                new RequestAndResponse()
                {
                    RequestBody = requestBodys.ToJsonString(),
                    ResponseContent = responseContents.ToJsonString()
                });
        }

        /// <summary> 處理轉帳回傳結果 </summary>
        public override BaseReturnDataModel<UserScore> GetTransferReturnModel(string apiResult)
        {
            OBEBTransferResponseModel transferModel = apiResult.Deserialize<OBEBTransferResponseModel>();

            if (transferModel.IsSuccess && transferModel.data != null)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success);
            }

            return new BaseReturnDataModel<UserScore>(transferModel.message, null);
        }

        /// <summary> 處理查詢訂單回傳結果 </summary>
        public override BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            OBEBCheckTransferResponseModel transferModel = apiResult.Deserialize<OBEBCheckTransferResponseModel>();

            if (transferModel.IsSuccess && transferModel.data != null)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            return new BaseReturnModel($"error:{transferModel.message}");
        }

        /// <summary> 處理取得餘額回傳結果 </summary>
        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            OBEBBalanceResponseModel balanceModel = apiResult.Deserialize<OBEBBalanceResponseModel>();

            if (balanceModel.IsSuccess && balanceModel.data != null)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success, new UserScore() { AvailableScores = balanceModel.data.balance.ToDecimal() });
            }

            return new BaseReturnDataModel<UserScore>(balanceModel.message, null);
        }

        /// <summary> 檢查以及創建帳號 </summary>
        protected override BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam createRemoteAccountParam)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteCreateAccountApiResult(createRemoteAccountParam);

            if (returnModel.IsSuccess)
            {
                OBEBCreateUserResponseModel createUserModel = returnModel.DataModel.Deserialize<OBEBCreateUserResponseModel>();

                //檢查帳號重複同一隻API
                if ((createUserModel.IsSuccess && createUserModel.data != null) ||
                    createUserModel.code == OBEBResponseCode.AccountExist)
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                return new BaseReturnModel(createUserModel.message);
            }

            return new BaseReturnModel(returnModel.Message);
        }

        protected override BaseReturnDataModel<string> GetRemoteLoginApiResult(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            try
            {
                // 会员离桌
                OBEBBaseResponseModel responseModel = ForeLeaveTable(tpGameRemoteLoginParam.CreateRemoteAccountParam.TPGameAccount);
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"{Product} ForeLeaveTable 会员离桌 請求失敗：{ex}");
            }

            //IPV6格式會不符合
            if (tpGameRemoteLoginParam.IpAddress.Contains(":"))
            {
                tpGameRemoteLoginParam.IpAddress = "127.0.0.1";
            }

            // 主播ID
            string anchorId = tpGameRemoteLoginParam.LoginInfo.RemoteCode;

            if (anchorId.IsNullOrEmpty())
            {
                // 如果給空字串第三方JSON解析會失敗
                anchorId = null;
            }

            var request = new OBEBForwardGameRequestModel
            {
                loginName = tpGameRemoteLoginParam.CreateRemoteAccountParam.TPGameAccount,
                loginPassword = tpGameRemoteLoginParam.CreateRemoteAccountParam.TPGamePassword,
                deviceType = tpGameRemoteLoginParam.IsMobile ? 2 : 1,
                ip = tpGameRemoteLoginParam.IpAddress,
                backurl = GetCombineUrl(SharedAppSettings.FrontSideWebUrl, "ReconnectTips"),
                anchorId = anchorId,
            };

            BaseReturnDataModel<string> returnModel = DoPostRequest(OBEBSharedAppSetting.ForwardGameUrl, request, out DetailRequestAndResponse detail);

            return returnModel;
        }

        protected override void DoKickUser(Model.Entity.ThirdPartyUserAccount thirdPartyUserAccount) => new NotImplementedException();

        protected override string CreateTPGamePasswordByRule(int userId, string tpGameAccount) => tpGameAccount;

        /// <summary> 取得遊戲入口網址 </summary>
        protected override BaseReturnDataModel<string> GetRemoteForwardGameUrl(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteLoginApiResult(tpGameRemoteLoginParam);

            if (!returnModel.IsSuccess)
            {
                return new BaseReturnDataModel<string>(returnModel.Message, string.Empty);
            }

            OBEBForwardGameResponseModel launchGameModel = returnModel.DataModel.Deserialize<OBEBForwardGameResponseModel>();

            if (launchGameModel.IsSuccess)
            {
                return new BaseReturnDataModel<string>(ReturnCode.Success, launchGameModel.data.url);
            }

            return new BaseReturnDataModel<string>(launchGameModel.message, string.Empty);
        }

        /// <summary> 無此API </summary>
        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            throw new System.NotImplementedException();
        }

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            var createUser = new OBEBUserRequestModel
            {
                loginName = createRemoteAccountParam.TPGameAccount,
                loginPassword = createRemoteAccountParam.TPGamePassword,
            };

            return DoPostRequest(OBEBSharedAppSetting.CreateAccountUrl, createUser, out DetailRequestAndResponse detail);
        }

        private BaseReturnDataModel<string> DoPostRequest<T>(string relativeUrl, T requestData, out DetailRequestAndResponse detail)
        {
            string fullUrl = GetCombineUrl(OBEBSharedAppSetting.ServiceBaseUrl, relativeUrl);

            return DoPostRequest(fullUrl, requestData, null, out detail);
        }

        private BaseReturnDataModel<string> DoDataPostRequest<T>(string relativeUrl, T requestData, int pageIndex)
        {
            string fullUrl = GetCombineUrl(OBEBSharedAppSetting.DataServiceBaseUrl, relativeUrl);

            var headers = new Dictionary<string, string>
            {
                { "merchantCode", OBEBSharedAppSetting.MerchantCode },
                { "pageIndex", pageIndex.ToString() }
            };

            return DoPostRequest(fullUrl, requestData, headers, out DetailRequestAndResponse detail);
        }

        private BaseReturnDataModel<string> DoPostRequest<T>(string fullUrl, T requestData,
            Dictionary<string, string> headers, out DetailRequestAndResponse detail)
        {
            string requestDataJson = requestData.ToJsonString(isCamelCaseNaming: true);
            string parameters = AESTool.Encrypt(requestDataJson, OBEBSharedAppSetting.AESKey);
            string signature = MD5Tool.MD5EncodingForOBGameProvider(requestDataJson + OBEBSharedAppSetting.MD5Key).ToUpper();

            var requestBoby = new Dictionary<string, string>
            {
                { "merchantCode", OBEBSharedAppSetting.MerchantCode },
                { "params", parameters },
                { "signature", signature },
            };

            string requestBobyJson = requestBoby.ToJsonString();

            var httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>().Value;

            var webRequestParam = new WebRequestParam()
            {
                Purpose = $"TPGService.{Product.Value}請求",
                Method = HttpMethod.Post,
                Url = fullUrl,
                Body = requestBobyJson,
                ContentType = HttpWebRequestContentType.Json,
                Headers = headers,
                IsResponseValidJson = true,
            };

            string apiResult = httpWebRequestUtilService.GetResponse(webRequestParam, out HttpStatusCode httpStatusCode);
            detail = new DetailRequestAndResponse(webRequestParam, apiResult);

            return ConvertToApiReturnDataModel(httpStatusCode, apiResult);
        }
    }
}