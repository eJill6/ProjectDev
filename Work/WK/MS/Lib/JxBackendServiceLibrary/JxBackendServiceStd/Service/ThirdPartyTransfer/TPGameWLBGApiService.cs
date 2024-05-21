using JxBackendService.Common;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.ThirdPartyTransfer.WLBG;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.WLBG;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameWLBGApiService : BaseTPGameApiService, ITPGameWLBGApiService
    {
        private static readonly int s_maxSearchRangeMinutes = 59;

        private static readonly int s_backSqliteSearchDateMinutes = 10;

        private static readonly string s_wlRequestDateTimeFormat = "yyyyMMddHHmmss";

        private static readonly string s_reasonGameLimit = "game_limit";

        protected override bool IsAllowTransferCompensation => true;

        public static string WLRequestDateTimeFormat => s_wlRequestDateTimeFormat;

        public override PlatformProduct Product => PlatformProduct.WLBG;

        public static int MaxSearchRangeMinutes => s_maxSearchRangeMinutes;

        public static int BackSqliteSearchDateMinutes => s_backSqliteSearchDateMinutes;

        public TPGameWLBGApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public Dictionary<string, string> GetApiGameListResult()
        {
            return JxCacheService.GetCache(new SearchCacheParam()
            {
                Key = CacheKey.ApiGameList(Product),
                CacheSeconds = 60 * 60 * 24,
                IsCloneInstance = false,
            }, () =>
            {
                long time = DateTimeUtil.ToUnixOfTime(DateTime.Now);
                string aid = WLBGSharedAppSetting.APIAccount;
                string sign = MD5Tool.ToMD5($"{aid}{time}{WLBGSharedAppSetting.RequestSignKey}", isToUpper: false);

                WLBGGetGameListRequestModel request = new WLBGGetGameListRequestModel()
                {
                    Aid = aid,
                    Sign = sign,
                    Time = time,
                };

                BaseReturnDataModel<string> returnModel = DoDataPostRequest(WLBGSharedAppSetting.GetGameList, request, isCamelCaseNaming: true);

                var model = returnModel.DataModel.Deserialize<WLBGBaseResponseWithDataModel<WLBGGameListDataResponseModel>>();
                var result = new Dictionary<string, string>();

                model.Data.Games.ForEach(o =>
                {
                    result[o.Game.ToString()] = o.Name;
                });

                return result;
            });
        }

        protected override string CreateTPGamePasswordByRule(int userId, string tpGameAccount) => null;

        /// <summary> 轉帳 </summary>
        public override DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, CreateRemoteAccountParam createRemoteAccountParam, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            decimal transferAmount = tpGameMoneyInfo.Amount;

            // 轉回主錢包
            if (!isMoneyIn)
            {
                transferAmount *= (-1);
            }

            var request = new WLBGTransferRequestModel
            {
                Uid = createRemoteAccountParam.TPGameAccount,
                OrderId = tpGameMoneyInfo.OrderID,
                Credit = transferAmount.ToAmountWithoutThousandthCommaString(),
            };

            DoGetRequest(WLBGSharedAppSetting.TransferV3, request, out DetailRequestAndResponse detail);

            return detail;
        }

        /// <summary> 查詢訂單 </summary>
        protected override DetailRequestAndResponse GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new WLBGCheckTransferRequestModel
            {
                OrderId = tpGameMoneyInfo.OrderID
            };

            DoGetRequest(WLBGSharedAppSetting.QueryOrderV3, request, out DetailRequestAndResponse detail);

            return detail;
        }

        /// <summary> 查詢餘額 </summary>
        public override string GetRemoteUserScoreApiResult(CreateRemoteAccountParam createRemoteAccountParam, IInvocationUserParam invocationUserParam)
        {
            var request = new WLBGUidRequestModel
            {
                Uid = createRemoteAccountParam.TPGameAccount
            };

            BaseReturnDataModel<string> returnModel = DoGetRequest(WLBGSharedAppSetting.GetBalance, request, out DetailRequestAndResponse detail);

            return returnModel.DataModel;
        }

        protected WLBGGetBetLogRequestModel GetWLBGBetLogRequest(string lastSearchToken)
        {
            //⽀持查询最近⼀个⽉内，不超过 1 ⼩时的数据
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

            var request = new WLBGGetBetLogRequestModel
            {
                From = searchStartDate.ToFormatDateTimeStringWithoutSymbol(),
                Until = searchEndDate.ToFormatDateTimeStringWithoutSymbol(),
            };

            return request;
        }

        /// <summary>
        /// 取得盈虧資料
        /// </summary>
        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            WLBGGetBetLogRequestModel request = GetWLBGBetLogRequest(lastSearchToken);
            BaseReturnDataModel<string> returnModel = DoGetRequest(WLBGSharedAppSetting.GetRecordV2, request, out DetailRequestAndResponse detail);

            var returnDataModel = returnModel.CastByCodeAndMessage<RequestAndResponse>();
            returnDataModel.DataModel = new RequestAndResponse()
            {
                RequestBody = request.ToJsonString(),
                ResponseContent = returnModel.DataModel
            };

            return returnDataModel;
        }

        /// <summary>
        /// 處理轉帳回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetTransferReturnModel(string apiResult)
        {
            WLBGTransferResponseModel transferModel = apiResult.Deserialize<WLBGTransferResponseModel>();

            if (transferModel.IsSuccess && transferModel.Data.Status == WLBGResponseStatus.Success)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success,
                    new UserScore() { AvailableScores = transferModel.Data.Balance.ToDecimal() });
            }

            string errorMessage = transferModel.Msg;

            if (errorMessage.IsNullOrEmpty())
            {
                errorMessage = WLResponseReasonType.CombineMessage(transferModel.Data.Reason);
            }

            return new BaseReturnDataModel<UserScore>(errorMessage);
        }

        /// <summary>
        /// 處理查詢訂單回傳結果
        /// </summary>
        public override BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            WLBGCheckTransferResponseModel responseModel = apiResult.Deserialize<WLBGCheckTransferResponseModel>();

            if (!responseModel.IsSuccess)
            {
                return null;
            }

            if (responseModel.Data.Status == WLBGResponseStatus.Success)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            if (responseModel.Data.Status == WLBGResponseStatus.Failed)
            {
                string errorMessage = WLResponseReasonType.CombineMessage(responseModel.Data.Reason);

                return new BaseReturnModel(errorMessage);
            }

            if (responseModel.Data.Status == WLBGResponseStatus.NotFound)
            {
                return new BaseReturnModel(responseModel.Data.Reason);
            }

            return null;
        }

        /// <summary>
        /// 處理取得餘額回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            WLBGBalanceResponseModel userScoreModel = apiResult.Deserialize<WLBGBalanceResponseModel>();

            //未知⽤户不会报错，会按余额为 0 返回
            if (userScoreModel.IsSuccess)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success,
                    new UserScore() { AvailableScores = userScoreModel.Data.Balance.ToDecimal() });
            }

            return new BaseReturnDataModel<UserScore>(userScoreModel.Msg ?? userScoreModel.Data.Status.ToString(), null);
        }

        /// <summary>
        /// 檢查以及創建帳號
        /// </summary>
        protected override BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam createRemoteAccountParam)
        {
            //創帳號同啟動遊戲
            BaseReturnDataModel<string> returnModel = GetRemoteCreateAccountApiResult(createRemoteAccountParam);

            if (returnModel.IsSuccess)
            {
                WLBGRegisterResponseModel registerModel = returnModel.DataModel.Deserialize<WLBGRegisterResponseModel>();

                //檢查帳號重複同一隻API
                if (registerModel.IsSuccess && registerModel.Data.Status == WLBGResponseStatus.Success)
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                return new BaseReturnModel(registerModel.Msg);
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
                return new BaseReturnDataModel<string>(returnModel.Message, string.Empty);
            }

            WLBGLaunchGameResponseModel launchGameModel = returnModel.DataModel.Deserialize<WLBGLaunchGameResponseModel>();

            if (launchGameModel.IsSuccess && launchGameModel.Data != null && !launchGameModel.Data.GameUrl.IsNullOrEmpty())
            {
                return new BaseReturnDataModel<string>(ReturnCode.Success, launchGameModel.Data.GameUrl);
            }

            if (!launchGameModel.Msg.IsNullOrEmpty())
            {
                return new BaseReturnDataModel<string>(launchGameModel.Msg);
            }

            if (launchGameModel.Data != null && !launchGameModel.Data.GameReason.IsNullOrEmpty())
            {
                return new BaseReturnDataModel<string>(launchGameModel.Data.GameReason);
            }

            return new BaseReturnDataModel<string>(ReturnCode.OperationFailed);
        }

        protected override BaseReturnDataModel<string> GetRemoteLoginApiResult(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            string subGameCode = GetThirdPartyRemoteCode(tpGameRemoteLoginParam.LoginInfo);

            var request = new WLBGLaunchGameRequestModel
            {
                Uid = tpGameRemoteLoginParam.CreateRemoteAccountParam.TPGameAccount,
                Game = subGameCode
            };

            BaseReturnDataModel<string> returnModel = DoGetRequest(WLBGSharedAppSetting.EnterGame, request, out DetailRequestAndResponse detail);

            return returnModel;
        }

        protected override void DoKickUser(ThirdPartyUserAccount thirdPartyUserAccount)
        {
            var request = new WLBGUidRequestModel
            {
                Uid = thirdPartyUserAccount.Account
            };

            BaseReturnDataModel<string> returnModel = DoGetRequest(WLBGSharedAppSetting.Kick, request, out DetailRequestAndResponse detail);
            LogUtilService.ForcedDebug($"{nameof(DoKickUser)}:{new { thirdPartyUserAccount.Account, returnModel.DataModel }.ToJsonString()}");
        }

        /// <summary>
        /// 打第三方API，加密簽名方法
        /// </summary>
        private BaseReturnDataModel<string> DoGetRequest(string relativeUrl, object request, out DetailRequestAndResponse detail)
        {
            string requestBody = request.ToKeyValueURL(isCamelCase: true);

            //p参数
            string param = AESTool.Encrypt(requestBody, WLBGSharedAppSetting.DataEncryptionKey);
            string unixTime = DateTime.Now.ToUnixOfTime(UnixOfTimeTypes.TotalSeconds).ToString();
            //k参数 将加密后参数、时间戳、signKey连接为⼀个字符
            string sign = MD5Tool.ToMD5($"{param}{unixTime}{WLBGSharedAppSetting.RequestSignKey}", isToUpper: false);
            string fullUrl = GenerateApiPath(GetFullUrl(WLBGSharedAppSetting.ServiceUrl, relativeUrl), WLBGSharedAppSetting.APIAccount, unixTime, param, sign);

            var httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>().Value;
            var webRequestParam = new WebRequestParam()
            {
                Purpose = $"TPGService.{Product.Value}請求",
                Method = HttpMethod.Get,
                Url = fullUrl,
                ContentType = HttpWebRequestContentType.Json,
                IsResponseValidJson = true
            };

            string apiResult = httpWebRequestUtilService.GetResponse(webRequestParam, out HttpStatusCode httpStatusCode);
            detail = new DetailRequestAndResponse(webRequestParam, apiResult);

            return ConvertToApiReturnDataModel(httpStatusCode, apiResult);
        }

        private BaseReturnDataModel<string> DoDataPostRequest(string relativeUrl, object request, bool isCamelCaseNaming = false)
        {
            string requestBody = request.ToJsonString(isCamelCaseNaming: isCamelCaseNaming);
            string fullUrl = GetFullUrl(WLBGSharedAppSetting.DataServiceUrl, relativeUrl);

            var httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>().Value;

            string apiResult = httpWebRequestUtilService.GetResponse(
                new WebRequestParam()
                {
                    Purpose = $"TPGService.{Product.Value}請求",
                    Method = HttpMethod.Post,
                    Url = fullUrl,
                    Body = requestBody,
                    ContentType = HttpWebRequestContentType.Json,
                    IsResponseValidJson = true
                },
            out HttpStatusCode httpStatusCode);

            return ConvertToApiReturnDataModel(httpStatusCode, apiResult);
        }

        private string GenerateApiPath(string host, string apiAccoint, string unixTime, string param, string sign)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(host);
            stringBuilder.Append("?a=").Append(apiAccoint);
            stringBuilder.Append("&t=").Append(unixTime);
            stringBuilder.Append("&p=").Append(System.Web.HttpUtility.UrlEncode(param));
            stringBuilder.Append("&k=").Append(sign);

            return stringBuilder.ToString();
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            //無此API
            throw new NotSupportedException();
        }

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            var request = new WLBGRegisterRequestModel
            {
                Uid = createRemoteAccountParam.TPGameAccount
            };

            return DoGetRequest(WLBGSharedAppSetting.Register, request, out DetailRequestAndResponse detail);
        }

        protected override bool IsDoTransferCompensation(string apiResult)
        {
            WLBGTransferResponseModel transferModel = apiResult.Deserialize<WLBGTransferResponseModel>();

            if (transferModel.Data != null &&
                transferModel.Data.Status == WLBGResponseStatus.Failed &&
                transferModel.Data.Reason == s_reasonGameLimit)
            {
                return true;
            }

            return false;
        }
    }
}