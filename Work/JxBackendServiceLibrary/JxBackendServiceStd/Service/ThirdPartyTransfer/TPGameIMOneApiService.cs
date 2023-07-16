using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.Common.Util.ThirdParty;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository.Game.Lottery;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Common;
using JxBackendService.Model.Common.IMOne;
using JxBackendService.Model.Entity.Game.Lottery;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.IM;
using JxBackendService.Model.ThirdParty.IM.Lottery;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
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

        protected readonly IGameAppSettingService _gameAppSettingService;

        public abstract IIMOneAppSetting AppSetting { get; }

        protected override int? TransferAmountFloorDigit => 2;

        public TPGameIMOneApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _gameAppSettingService = DependencyUtil.ResolveKeyed<IGameAppSettingService>(envLoginUser.Application, SharedAppSettings.PlatformMerchant);
        }

        protected override DetailRequestAndResponse GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new IMCheckTransferRequestModel()
            {
                MerchantCode = AppSetting.MerchantCode,
                PlayerId = tpGameAccount,
                TransactionId = tpGameMoneyInfo.OrderID,
                ProductWallet = AppSetting.ProductWallet
            };

            DoPostRequest(BaseIMOneAppSetting.CheckTransferUrl, request.ToJsonString(), out DetailRequestAndResponse detail);

            return detail;
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
                returnModel = new BaseReturnModel($"error:{transferModel.Message}");
            }

            return returnModel;
        }

        protected override DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, CreateRemoteAccountParam createRemoteAccountParam, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            string amountText = tpGameMoneyInfo.Amount.ToString("0.####");

            if (!isMoneyIn)
            {
                amountText = $"-{amountText}";
            }

            var request = new IMTransferRequestModel
            {
                MerchantCode = AppSetting.MerchantCode,
                PlayerId = createRemoteAccountParam.TPGameAccount,
                Amount = amountText,
                TransactionId = tpGameMoneyInfo.OrderID,
                ProductWallet = AppSetting.ProductWallet
            };

            DoPostRequest(BaseIMOneAppSetting.TransferUrl, request.ToJsonString(), out DetailRequestAndResponse detail);

            return detail;
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
        protected override string GetRemoteUserScoreApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            var request = new IMGetBalanceRequestModel
            {
                MerchantCode = AppSetting.MerchantCode,
                ProductWallet = AppSetting.ProductWallet,
                PlayerId = createRemoteAccountParam.TPGameAccount
            };

            BaseReturnDataModel<string> returnModel = DoPostRequest(BaseIMOneAppSetting.GetBalanceUrl, request.ToJsonString(), out DetailRequestAndResponse detail);

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
                IMRegisterResponseModel registerModel = returnModel.DataModel.Deserialize<IMRegisterResponseModel>();

                if (registerModel.Code == IMOneResponseCode.Success)
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                return new BaseReturnModel(registerModel.Message);
            }

            return new BaseReturnModel(returnModel.Message);
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            var request = new IMCheckExistRequestModel
            {
                MerchantCode = AppSetting.MerchantCode,
                ProductWallet = AppSetting.ProductWallet,
                PlayerId = createRemoteAccountParam.TPGameAccount
            };

            BaseReturnDataModel<string> returnModel = DoPostRequest(BaseIMOneAppSetting.CheckAccountExistUrl, request.ToJsonString(), out DetailRequestAndResponse detail);

            return returnModel;
        }

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            var request = new IMRegisterRequestModel
            {
                MerchantCode = AppSetting.MerchantCode,
                PlayerId = createRemoteAccountParam.TPGameAccount,
                Currency = AppSetting.Currency,
                Password = createRemoteAccountParam.TPGamePassword
            };

            return DoPostRequest(BaseIMOneAppSetting.CreateAccountUrl, request.ToJsonString(), out DetailRequestAndResponse detail);
        }

        protected override string CreateTPGamePasswordByRule(int userId, string tpGameAccount)
        {
            return tpGameAccount.Replace("_", string.Empty); //不允許底線
        }

        /// <summary>
        /// 檢查帳號
        /// </summary>
        protected BaseReturnDataModel<bool> CheckAccountExist(CreateRemoteAccountParam createRemoteAccountParam)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteCheckAccountExistApiResult(createRemoteAccountParam);

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
        protected override BaseReturnDataModel<string> GetRemoteForwardGameUrl(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteLoginApiResult(tpGameRemoteLoginParam);

            if (!returnModel.IsSuccess)
            {
                return new BaseReturnDataModel<string>(returnModel.Message, string.Empty);
            }

            IMLaunchGameResponseModel launchGameModel = returnModel.DataModel.Deserialize<IMLaunchGameResponseModel>();

            if (launchGameModel.Code == IMOneResponseCode.Success)
            {
                return new BaseReturnDataModel<string>(ReturnCode.Success, launchGameModel.GameUrl);
            }

            return new BaseReturnDataModel<string>(launchGameModel.Message);
        }

        protected override BaseReturnDataModel<string> GetRemoteLoginApiResult(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            string remoteGameCode = GetThirdPartyRemoteCode(tpGameRemoteLoginParam.LoginInfo);

            if (remoteGameCode.IsNullOrEmpty())
            {
                remoteGameCode = AppSetting.GameCode;
            }

            IMLaunchGameRequestModel request = GetIMLaunchGameRequestModel(
                tpGameRemoteLoginParam.CreateRemoteAccountParam.TPGameAccount,
                tpGameRemoteLoginParam.IpAddress,
                remoteGameCode);

            string url = BaseIMOneAppSetting.LaunchGameUrl;

            if (tpGameRemoteLoginParam.IsMobile)
            {
                url = BaseIMOneAppSetting.LaunchMobileGameUrl;
            }

            BaseReturnDataModel<string> returnModel = DoPostRequest(url, request.ToJsonString(), out DetailRequestAndResponse detail);

            return returnModel;
        }

        protected virtual IMLaunchGameRequestModel GetIMLaunchGameRequestModel(string tpGameAccount, string ipAddress, string gameCode)
        {
            return new IMLaunchGameRequestModel
            {
                MerchantCode = AppSetting.MerchantCode,
                ProductWallet = AppSetting.ProductWallet,
                PlayerId = tpGameAccount,
                IpAddress = ipAddress,
                Language = AppSetting.Language,
                GameCode = gameCode
            };
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
                MerchantCode = AppSetting.MerchantCode,
                StartDate = GameIMUtil.ToBetRecordTimeFormatString(searchStartDate),
                EndDate = GameIMUtil.ToBetRecordTimeFormatString(searchEndDate),
                Page = 1,
                Currency = AppSetting.Currency,
                ProductWallet = AppSetting.ProductWallet
            };

            string requestBody = request.ToJsonString();

            BaseReturnDataModel<string> returnModel = DoPostRequest(AppSetting.ReportServiceUrl, AppSetting.GetBetLogPath, requestBody, out DetailRequestAndResponse detail);
            var returnDataModel = returnModel.CastByCodeAndMessage<RequestAndResponse>();

            returnDataModel.DataModel = new RequestAndResponse()
            {
                RequestBody = requestBody,
                ResponseContent = returnModel.DataModel
            };

            return returnDataModel;
        }

        private BaseReturnDataModel<string> DoPostRequest(string relativeUrl, string requestBody, out DetailRequestAndResponse detail)
            => DoPostRequest(AppSetting.ServiceUrl, relativeUrl, requestBody, out detail);

        private BaseReturnDataModel<string> DoPostRequest(string apiServiceUrl, string relativeUrl, string requestBody, out DetailRequestAndResponse detail)
        {
            string url = GetFullUrl(apiServiceUrl, relativeUrl);

            var httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>();

            var webRequestParam = new WebRequestParam()
            {
                Purpose = $"TPGService.{Product.Value}請求",
                Method = HttpMethod.Post,
                Url = url,
                Body = requestBody,
                ContentType = HttpWebRequestContentType.Json,
                IsResponseValidJson = true
            };

            string apiResult = httpWebRequestUtilService.GetResponse(webRequestParam, out HttpStatusCode httpStatusCode);
            detail = new DetailRequestAndResponse(webRequestParam, apiResult);

            return ConverToApiReturnDataModel(httpStatusCode, apiResult);
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