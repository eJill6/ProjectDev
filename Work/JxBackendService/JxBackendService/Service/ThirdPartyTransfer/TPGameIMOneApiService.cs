using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.Common.Util.ThirdParty;
using JxBackendService.Interface.Repository.Game;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity.Game;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.IM;
using JxBackendService.Model.ThirdParty.IM.Lottery;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Cache;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameIMOneApiService : BaseTPGameApiService
    {
        public static readonly int MaxSearchRangeMinutes = 10;

        public abstract IMLotterySharedAppSettings AppSettings { get; }

        public TPGameIMOneApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override string GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new IMCheckTransferRequestModel()
            {
                MerchantCode = AppSettings.MerchantCode,
                PlayerId = tpGameAccount,
                TransactionId = tpGameMoneyInfo.OrderID,
                ProductWallet = AppSettings.ProductWallet
            };

            BaseReturnDataModel<string> returnModel = DoPostRequest(AppSettings.CheckTransferUrl, request.ToJsonString());
            return returnModel.DataModel;
        }

        public override BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            BaseReturnModel returnModel = null;
            IMTransferResponseModel transferModel = apiResult.Deserialize<IMTransferResponseModel>();

            if (transferModel.Code == IMOneResponseCode.Success)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }
            else if (transferModel.Status == IMOneResponseStatus.Processed)
            {
                //處理中,不回傳結果, 讓後續流程不處理任何DB交易                
            }
            else
            {
                returnModel = new BaseReturnModel(transferModel.Message);
            }

            return returnModel;
        }

        protected override string GetRemoteTransferApiResult(bool isMoneyIn, string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new IMTransferRequestModel
            {
                MerchantCode = AppSettings.MerchantCode,
                PlayerId = tpGameAccount,
                Amount = Math.Round(isMoneyIn ? tpGameMoneyInfo.Amount : -tpGameMoneyInfo.Amount, 2),
                TransactionId = tpGameMoneyInfo.OrderID,
                ProductWallet = AppSettings.ProductWallet
            };
            var returnModel = DoPostRequest(AppSettings.TransferUrl, request.ToJsonString());
            return returnModel.DataModel;
        }

        public override BaseReturnDataModel<UserScore> GetTransferReturnModel(string apiResult)
        {
            IMTransferResponseModel transferModel = apiResult.Deserialize<IMTransferResponseModel>();

            if (transferModel.Code == IMOneResponseCode.Success)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success);
            }

            return new BaseReturnDataModel<UserScore>(transferModel.Message, null);
        }

        /// <summary>
        /// 取得使用者餘額
        /// </summary>
        protected override string GetRemoteUserScoreApiResult(string tpGameAccount)
        {
            var request = new IMGetBalanceRequestModel
            {
                MerchantCode = AppSettings.MerchantCode,
                ProductWallet = AppSettings.ProductWallet,
                PlayerId = tpGameAccount
            };

            var returnModel = DoPostRequest(AppSettings.GetBalanceUrl, request.ToJsonString());
            return returnModel.DataModel;
        }

        /// <summary>
        /// 取回使用者餘額
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            IMGetBalanceResponseModel userScoreModel = apiResult.Deserialize<IMGetBalanceResponseModel>();

            if (userScoreModel.Code == IMOneResponseCode.Success)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success, new UserScore() { AvailableScores = Convert.ToDecimal(userScoreModel.Balance) });
            }

            return new BaseReturnDataModel<UserScore>(userScoreModel.Message, null);
        }

        /// <summary>
        /// 創帳號
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
                IMRegisterResponseModel registerModel = returnModel.DataModel.Deserialize<IMRegisterResponseModel>();

                if (registerModel.Code == IMOneResponseCode.Success)
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                return new BaseReturnModel(registerModel.Message);
            }

            return new BaseReturnModel(returnModel.Message);
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(string tpGameAccount)
        {
            var request = new IMCheckExistRequestModel
            {
                MerchantCode = AppSettings.MerchantCode,
                ProductWallet = AppSettings.ProductWallet,
                PlayerId = tpGameAccount
            };

            BaseReturnDataModel<string> returnModel = DoPostRequest(AppSettings.CheckAccountExistUrl, request.ToJsonString());
            return returnModel;
        }

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam param)
        {
            var request = new IMRegisterRequestModel
            {
                MerchantCode = AppSettings.MerchantCode,
                PlayerId = param.TPGameAccount,
                Currency = AppSettings.Currency,
                Password = param.TPGameAccount.Replace("_", "") //不允許底線
            };

            return DoPostRequest(AppSettings.CreateAccountUrl, request.ToJsonString());
        }

        /// <summary>
        /// 檢查帳號
        /// </summary>
        private BaseReturnDataModel<bool> CheckAccountExist(string tpGameAccount)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteCheckAccountExistApiResult(tpGameAccount);

            if (returnModel.IsSuccess)
            {
                IMRegisterResponseModel checkAccountModel = returnModel.DataModel.Deserialize<IMRegisterResponseModel>();

                if (checkAccountModel.Code == IMOneResponseCode.Success)
                {
                    return new BaseReturnDataModel<bool>(ReturnCode.Success, true);
                }

                return new BaseReturnDataModel<bool>(checkAccountModel.Message, false);
            }

            return new BaseReturnDataModel<bool>(returnModel.Message, false);
        }

        /// <summary>
        /// 啟動遊戲
        /// </summary>
        protected override BaseReturnDataModel<string> GetRemoteForwardGameUrl(string tpGameAccount, string ip, bool isMobile)
        {
            var request = new IMLaunchGameRequestModel
            {
                MerchantCode = AppSettings.MerchantCode,
                ProductWallet = AppSettings.ProductWallet,
                PlayerId = tpGameAccount,
                IpAddress = ip,
                Language = AppSettings.Language,
                GameCode = AppSettings.GameCode
            };
            string url = AppSettings.LaunchGameUrl;
            if (isMobile)
            {
                url = AppSettings.LaunchMobileGameUrl;
            }

            var returnModel = DoPostRequest(url, request.ToJsonString());
            if (returnModel.IsSuccess)
            {
                IMLaunchGameResponseModel launchGameModel = returnModel.DataModel.Deserialize<IMLaunchGameResponseModel>();

                if (launchGameModel.Code == IMOneResponseCode.Success)
                {
                    return new BaseReturnDataModel<string>(ReturnCode.Success, launchGameModel.GameUrl);
                }

                return new BaseReturnDataModel<string>(launchGameModel.Message, string.Empty);
            }

            return new BaseReturnDataModel<string>(returnModel.Message, string.Empty);
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

            var request = new IMGetBetLogRequestModel
            {
                MerchantCode = AppSettings.MerchantCode,
                StartDate = GameIMUtil.ToBetRecordTimeFormatString(searchStartDate),
                EndDate = GameIMUtil.ToBetRecordTimeFormatString(searchEndDate),
                Page = 1,
                Currency = AppSettings.Currency,
                ProductWallet = AppSettings.ProductWallet
            };

            string requestBody = request.ToJsonString();

            BaseReturnDataModel<string> returnModel = DoPostRequest(AppSettings.GetBetLogUrl, requestBody);
            return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.GetSingle(returnModel.Code),
                new RequestAndResponse()
                {
                    RequestBody = requestBody,
                    ResponseContent = returnModel.DataModel
                });

            //            return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.Success, new RequestAndResponse()
            //            {
            //                RequestBody = request.ToJsonString(),
            //                ResponseContent = @"{""Code"": ""0"",""Message"": ""Successful."",""Result"": [{""Provider"":""VR_LOTTERY"",""GameId"":""imlotto20001"",""GameName"":""Venus 1.5 Lottery"",""ChineseGameName"":""VR 金星 1.5 分彩"",
            //""GameNo"":""20210112264"",""GameNoId"":""1"",""PlayerId"":""jxD_69778"",""ProviderPlayerId"":""IM0TSGOGSadmintest"",""Currency"":""CNY"",""Tray"":""1980"",
            //""BetId"":""" + DateTime.Now.ToString("yyyyMMddHHmmss") + @""",""BetOn"":""五星总和大小单双"",""BetType"":""Position=万,千,百,十,个;Number=总和大;"",
            //""BetDetails"":""WinningNumber=8,0,5,6,6;Award=五星总和大小单双;PrizeNumber=总和大;Unit=1.00000;Multiple=1;Count=1;"",
            //""Odds"":""1.97"",""BetAmount"":1,""ValidBet"":1,""WinLoss"":0.97,""PlayerWinLoss"":0.97,""LossPrize"":0,""Tips"":0,
            //""CommissionRate"":0,""Commission"":0,""Status"":""Settled"",""Platform"":""NA"",""BetDate"":""2021-01-12 15:34:36 +08:00"",
            //""ResultDate"":""2021-01-12 15:36:43 +08:00"",""SettlementDate"":""2021-01-12 15:36:43 +08:00"",""ReportingDate"":""2021-01-12 15:36:43 +08:00"",
            //""DateCreated"":""2021-01-12 15:36:15 +08:00"",""LastUpdatedDate"":""2021-01-12 15:42:10 +08:00""}]}"
            //            });
        }

        private BaseReturnDataModel<string> DoPostRequest(string relativeUrl, string requestBody)
        {
            return DoPostRequest(AppSettings.ServiceUrl, relativeUrl, requestBody);
        }

        private BaseReturnDataModel<string> DoPostRequest(string url, string relativeUrl, string requestBody)
        {
            url = GetFullUrl(url, relativeUrl);
            string result = HttpWebRequestUtil.GetResponse(
                new WebRequestParam()
                {
                    Purpose = $"TPGService.{Product.Value}請求",
                    Method = HttpMethod.Post,
                    Url = url,
                    Body = requestBody,
                    ContentType = HttpWebRequestContentType.Json,
                    IsResponseValidJson = true
                },
            out HttpStatusCode httpStatusCode);

            return new BaseReturnDataModel<string>()
            {
                IsSuccess = httpStatusCode == HttpStatusCode.OK,
                DataModel = result
            };
        }
    }

    public abstract class TPGameIMLotteryApiService : TPGameIMOneApiService
    {
        private readonly ILotteryInfoRep _lotteryInfoRep;
        private readonly IJxCacheService _jxCacheService;

        protected abstract int LotteryID { get; }

        public TPGameIMLotteryApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _lotteryInfoRep = ResolveJxBackendService<ILotteryInfoRep>();
            _jxCacheService = ResolveServiceForModel<IJxCacheService>(envLoginUser.Application);
        }

        public override BaseReturnModel GetAllowCreateTransferOrderResult()
        {
            Dictionary<int, LotteryInfo> lotteryInfoMap = GetAllDictionary();
            bool isActive = false;

            if (lotteryInfoMap.ContainsKey(LotteryID) && lotteryInfoMap[LotteryID].Status == 1)
            {
                isActive = true;
            }

            if (isActive)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }
            else
            {
                return new BaseReturnModel(ThirdPartyGameElement.GameMaintain);
            }
        }

        private Dictionary<int, LotteryInfo> GetAllDictionary()
        {
            return _jxCacheService.GetCache(CacheKey.LotteryInfo, false, () =>
            {
                List<LotteryInfo> lotteryInfos = _lotteryInfoRep.GetAll();
                return lotteryInfos.ToDictionary(d1 => d1.LotteryID, d2 => d2);
            });
        }
    }
}
