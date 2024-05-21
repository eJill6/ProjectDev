using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.AWCSP;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameAWCSPApiService : BaseTPGameApiService
    {
        private static readonly int s_maxSearchRangeMinutes = 60;

        private static readonly int s_backSqliteSearchDateMinutes = 15;

        private static readonly int s_betLogApiDelaySeconds = 20;

        //24小時制
        private static readonly string s_awcspRequestDateTimeFormat = "yyyy-MM-ddTHH:mm:sszzz";

        /// <summary> 賽馬投注上下限，目前異動不大先不放到設定檔 </summary>
        private static readonly HORSEBOOKRegisterBetLimit s_horsebookRegisterBetLimit = new HORSEBOOKRegisterBetLimit
        {
            LIVE = new HORSEBOOKLIVE
            {
                Minbet = 10,
                Maxbet = 3000,
                MaxBetSumPerHorse = 10000,
                MinorMinbet = 10,
                MinorMaxbet = 1000,
                MinorMaxBetSumPerHorse = 10000
            }
        };

        /// <summary> 鬥雞投注上下限，目前異動不大先不放到設定檔 </summary>
        private static readonly SV388RegisterBetLimit s_sv388RegisterBetLimit = new SV388RegisterBetLimit
        {
            LIVE = new SV388LIVE
            {
                Minbet = 1,
                Maxbet = 2400,
                Mindraw = 1,
                Maxdraw = 1200,
                Matchlimit = 4800,
            }
        };

        private static readonly Dictionary<string, object> s_registerLimitMapping = new Dictionary<string, object>
        {
            { AWCSPPlatform.HORSEBOOK.Value, s_horsebookRegisterBetLimit },
            { AWCSPPlatform.SV388.Value, s_sv388RegisterBetLimit },
        };

        public static int MaxSearchRangeMinutes => s_maxSearchRangeMinutes;

        public static int BackSqliteSearchDateMinutes => s_backSqliteSearchDateMinutes;

        public static Dictionary<string, object> RegisterBetLimitMapping => s_registerLimitMapping;

        public override PlatformProduct Product => PlatformProduct.AWCSP;

        public static string AWCSPRequestDateTimeFormat => s_awcspRequestDateTimeFormat;

        public TPGameAWCSPApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override string CreateTPGamePasswordByRule(int userId, string tpGameAccount) => null;

        /// <summary>
        /// 轉帳
        /// </summary>
        public override DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, CreateRemoteAccountParam createRemoteAccountParam, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new AWCSPBaseRequestModel();
            string postUrl = string.Empty;

            if (isMoneyIn)
            {
                postUrl = AWCSPSharedAppSetting.Deposit;
                request = new AWCSPTransferRequestModel
                {
                    UserId = createRemoteAccountParam.TPGameAccount,
                    TxCode = tpGameMoneyInfo.OrderID,
                    TransferAmount = tpGameMoneyInfo.Amount.ToAmountWithoutThousandthCommaString()
                };
            }
            else
            {
                postUrl = AWCSPSharedAppSetting.Withdraw;
                request = new AWCSPWithdrawRequestModel
                {
                    UserId = createRemoteAccountParam.TPGameAccount,
                    TxCode = tpGameMoneyInfo.OrderID,
                    TransferAmount = tpGameMoneyInfo.Amount.ToAmountWithoutThousandthCommaString(),
                    WithdrawType = 0
                };
            }

            DoPostRequest(postUrl, request, out DetailRequestAndResponse detail);

            return detail;
        }

        /// <summary>
        /// 查詢訂單
        /// </summary>
        protected override DetailRequestAndResponse GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new AWCSPCheckTransferRequestModel
            {
                TxCode = tpGameMoneyInfo.OrderID
            };

            DoPostRequest(AWCSPSharedAppSetting.CheckTransferOperation, request, out DetailRequestAndResponse detail);

            return detail;
        }

        /// <summary>
        /// 查詢餘額
        /// </summary>
        public override string GetRemoteUserScoreApiResult(CreateRemoteAccountParam createRemoteAccountParam, IInvocationUserParam invocationUserParam)
        {
            var request = new AWCSPUserInfoRequestModel
            {
                UserIds = createRemoteAccountParam.TPGameAccount
            };

            BaseReturnDataModel<string> returnModel = DoPostRequest(AWCSPSharedAppSetting.GetBalance, request, out DetailRequestAndResponse detail);

            return returnModel.DataModel;
        }

        /// <summary>
        /// 取得盈虧資料
        /// </summary>
        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            var mergedBetLogResponse = new AWCSPBetLogResponseModel()
            {
                Status = AWCSPResponseCode.Success.Value,
                Transactions = new List<AWCSPBetLog>()
            };

            AWCSPGetBetLogRequestModel lastRequest = null;
            List<AWCSPPlatform> awcspPlatforms = AWCSPPlatform.GetAll();

            for (int i = 0; i < awcspPlatforms.Count; i++)
            {
                if (i > 0)
                {
                    //API 最快支持每 20 秒进行一次拉帐
                    Task.Delay(s_betLogApiDelaySeconds * 1000).Wait();
                }

                string awcspPlatform = awcspPlatforms[i].Value;
                lastRequest = GetAWCSPBetLogRequest(lastSearchToken, awcspPlatform);
                BaseReturnDataModel<string> returnModel = DoPostRequest(AWCSPSharedAppSetting.GetTransactionByTxTime, lastRequest, out DetailRequestAndResponse detail);

                AWCSPBetLogResponseModel betLogResponse = returnModel.DataModel.Deserialize<AWCSPBetLogResponseModel>();

                if (!betLogResponse.IsSuccess)
                {
                    return new BaseReturnDataModel<RequestAndResponse>(
                        betLogResponse.Desc,
                        new RequestAndResponse()
                        {
                            RequestBody = lastRequest.ToJsonString(),
                            ResponseContent = returnModel.DataModel
                        });
                }

                mergedBetLogResponse.Transactions.AddRange(betLogResponse.Transactions);
            }

            //非成功狀態都在之前return出去了,這裡直接當做成功處理
            return new BaseReturnDataModel<RequestAndResponse>(
                ReturnCode.Success,
                new RequestAndResponse()
                {
                    RequestBody = lastRequest.ToJsonString(),
                    ResponseContent = mergedBetLogResponse.ToJsonString()
                });
        }

        protected AWCSPGetBetLogRequestModel GetAWCSPBetLogRequest(string lastSearchToken, string platform)
        {
            //⽀持查询最近 7 天内，不超过 1 ⼩时的数据
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

            var request = new AWCSPGetBetLogRequestModel
            {
                StartTime = HttpUtility.UrlEncode(searchStartDate.ToString(AWCSPRequestDateTimeFormat)),
                EndTime = HttpUtility.UrlEncode(searchEndDate.ToString(AWCSPRequestDateTimeFormat)),
                Platform = platform
            };

            return request;
        }

        /// <summary>
        /// 處理轉帳回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetTransferReturnModel(string apiResult)
        {
            AWCSPTransferResponseModel transferModel = apiResult.Deserialize<AWCSPTransferResponseModel>();

            if (transferModel.IsSuccess)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success, new UserScore() { AvailableScores = transferModel.CurrentBalance });
            }

            return new BaseReturnDataModel<UserScore>(transferModel.Desc);
        }

        /// <summary>
        /// 處理查詢訂單回傳結果
        /// </summary>
        public override BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            AWCSPCheckTransferResponseModel responseModel = apiResult.Deserialize<AWCSPCheckTransferResponseModel>();

            if (responseModel.IsSuccess && responseModel.TxStatus == AWCSPResponseCode.TransferSuccess)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success);
            }
            else if (responseModel.Status == AWCSPResponseCode.DataDoesNotExist)
            {
                return new BaseReturnDataModel<UserScore>(responseModel.Desc);
            }

            return null;
        }

        /// <summary>
        /// 處理取得餘額回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            AWCSPUserBalanceResponseModel userScoreModel = apiResult.Deserialize<AWCSPUserBalanceResponseModel>();

            if (userScoreModel.IsSuccess)
            {
                //單位 元
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success, new UserScore() { AvailableScores = userScoreModel.Results[0].Balance });
            }

            return new BaseReturnDataModel<UserScore>(userScoreModel.Desc, null);
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
                AWCSPRegisterResponseModel registerModel = returnModel.DataModel.Deserialize<AWCSPRegisterResponseModel>();

                //檢查帳號重複同一隻API
                if (registerModel.IsSuccess ||
                    registerModel.Status == AWCSPResponseCode.AccountAlreadyExist)
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                return new BaseReturnModel(registerModel.Desc);
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

            AWCSPLaunchGameResponseModel launchGameModel = returnModel.DataModel.Deserialize<AWCSPLaunchGameResponseModel>();

            if (launchGameModel.IsSuccess)
            {
                return new BaseReturnDataModel<string>(ReturnCode.Success, launchGameModel.Url);
            }

            return new BaseReturnDataModel<string>(launchGameModel.Desc);
        }

        protected override BaseReturnDataModel<string> GetRemoteLoginApiResult(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            string platformValue = ThirdPartySubGameCodes.GetSingle(tpGameRemoteLoginParam.LoginInfo.GameCode).RemoteGameCode;
            AWCSPPlatform platform = AWCSPPlatform.GetSingle(platformValue);

            var request = new AWCSPLaunchGameRequestModel
            {
                UserId = tpGameRemoteLoginParam.CreateRemoteAccountParam.TPGameAccount,
                IsMobileLogin = tpGameRemoteLoginParam.IsMobile,
                //ExternalURL = GetCombineUrl(SharedAppSettings.FrontSideWebUrl, "ReconnectTips"),
                Platform = platform.Value,
                GameCode = platform.GameCode
            };

            BaseReturnDataModel<string> returnModel = DoPostRequest(AWCSPSharedAppSetting.DoLoginAndLaunchGame, request, out DetailRequestAndResponse detail);

            return returnModel;
        }

        protected override void DoKickUser(Model.Entity.ThirdPartyUserAccount thirdPartyUserAccount) => new NotImplementedException();

        /// <summary>
        /// 打第三方API，加密簽名方法
        /// </summary>
        private BaseReturnDataModel<string> DoPostRequest(string relativeUrl, AWCSPBaseRequestModel request, out DetailRequestAndResponse detail)
        {
            string fullUrl = GetFullUrl(AWCSPSharedAppSetting.ServiceUrl, relativeUrl);

            var httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>().Value;

            var webRequestParam = new WebRequestParam()
            {
                Purpose = $"TPGService.{Product.Value}請求",
                Method = HttpMethod.Post,
                Url = fullUrl,
                Body = request.ToKeyValueURL(isCamelCase: true),
                ContentType = HttpWebRequestContentType.WwwFormUrlencoded,
                IsResponseValidJson = false,
            };

            string apiResult = httpWebRequestUtilService.GetResponse(webRequestParam, out HttpStatusCode httpStatusCode);
            detail = new DetailRequestAndResponse(webRequestParam, apiResult);

            return ConvertToApiReturnDataModel(httpStatusCode, apiResult);
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            //無此API
            throw new NotSupportedException();
        }

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            var request = new AWCSPRegisterRequestModel
            {
                UserId = createRemoteAccountParam.TPGameAccount,
                BetLimit = RegisterBetLimitMapping.ToJsonString()
            };

            return DoPostRequest(AWCSPSharedAppSetting.CreateMember, request, out DetailRequestAndResponse detail);
        }
    }
}