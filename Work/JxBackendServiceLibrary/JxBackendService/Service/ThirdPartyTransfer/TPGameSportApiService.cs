using Flurl;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.Enums.ThirdParty.Handicap;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.SabaSport;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameSportApiService : BaseTPGameApiService
    {
        private readonly ISportAppSetting _sportAppSetting;

        private static SportServiceUrl _sportServiceUrl;

        public TPGameSportApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            var gameAppSettingService = DependencyUtil.ResolveKeyed<IGameAppSettingService>(envLoginUser.Application, SharedAppSettings.PlatformMerchant);
            _sportAppSetting = gameAppSettingService.GetSportAppSetting();
        }

        private SportServiceUrl GetSportServiceUrl()
        {
            _sportServiceUrl = AssignValueOnceUtil.GetAssignValueOnce(_sportServiceUrl, () =>
            {
                return new SportServiceUrl()
                {
                    CreateMember = Url.Combine(_sportAppSetting.ServiceUrl, "CreateMember"),
                    CheckUserBalance = Url.Combine(_sportAppSetting.ServiceUrl, "CheckUserBalance"),
                    GetSabaUrl = Url.Combine(_sportAppSetting.ServiceUrl, "GetSabaUrl"),
                    CheckFundTransfer = Url.Combine(_sportAppSetting.ServiceUrl, "CheckFundTransfer"),
                    FundTransfer = Url.Combine(_sportAppSetting.ServiceUrl, "FundTransfer"),
                    GetMemberBetSetting = Url.Combine(_sportAppSetting.ServiceUrl, "GetMemberBetSetting"),
                    SetMemberBetSetting = Url.Combine(_sportAppSetting.ServiceUrl, "SetMemberBetSetting"),
                };
            });

            return _sportServiceUrl;
        }

        public override PlatformProduct Product => PlatformProduct.Sport;

        public override BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            SportResponse<SportTransferData> tranferResponse = apiResult.Deserialize<SportResponse<SportTransferData>>();

            if (tranferResponse.IsSuccess && tranferResponse.Data.Status == 0)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            LogUtil.ForcedDebug($"远程查询订单状态失败 apiResult = {apiResult}");

            if (tranferResponse.IsSuccess && tranferResponse.Data.Status == 1) //交易失败
            {
                return new BaseReturnModel($"Status={tranferResponse.Data.Status}|{tranferResponse.Message}");
            }
            else if (tranferResponse.Code == 2) //交易纪录不存在
            {
                return new BaseReturnModel($"Code={tranferResponse.Code}|{tranferResponse.Message}");
            }

            // 非成功狀態皆進行重查訂單
            return null;
        }

        protected override DetailRequestAndResponse GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            string url = GetSportServiceUrl().CheckFundTransfer;
            string param = $"vendor_trans_id={tpGameMoneyInfo.OrderID}&wallet_id={_sportAppSetting.WalletId}";

            DoPostRequest(url, param, out DetailRequestAndResponse detail);

            return detail;
        }

        protected override DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            SportApiTypes actionCode;

            if (isMoneyIn)
            {
                actionCode = SportApiTypes.TransferIn;
            }
            else
            {
                actionCode = SportApiTypes.TransferOut;
            }

            string url = GetSportServiceUrl().FundTransfer;
            string param = $"vendor_member_id={tpGameAccount}&vendor_trans_id={tpGameMoneyInfo.OrderID}" +
                $"&amount={tpGameMoneyInfo.Amount}&currency={_sportAppSetting.Currency}" +
                $"&direction={(int)actionCode}&wallet_id={_sportAppSetting.WalletId}";

            DoPostRequest(url, param, out DetailRequestAndResponse detail);

            //live上壞很久了
            //JxTask.Run(EnvLoginUser, () =>
            //{
            //    Thread.Sleep(1000);
            //    SetUserBetAmountLimit(tpGameAccount);
            //});

            return detail;
        }

        protected override string GetRemoteUserScoreApiResult(string tpGameAccount)
        {
            BaseReturnDataModel<string> returnDataModel = GetBalanceApiResult(tpGameAccount);

            return returnDataModel.DataModel;
        }

        public override BaseReturnDataModel<UserScore> GetTransferReturnModel(string apiResult)
        {
            SportResponse<SportTransferData> tranferResponse = apiResult.Deserialize<SportResponse<SportTransferData>>();

            if (!tranferResponse.IsSuccess)
            {
                return new BaseReturnDataModel<UserScore>(tranferResponse.Message);
            }

            return new BaseReturnDataModel<UserScore>(ReturnCode.Success,
                new UserScore()
                {
                    AvailableScores = tranferResponse.Data.AfterAmount.GetValueOrDefault(),
                });
        }

        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            SportResponse<List<SportBalanceData>> balanceResponse = apiResult.Deserialize<SportResponse<List<SportBalanceData>>>();

            // 取不到第三方用戶餘額資訊
            SportBalanceData failBalanceData = balanceResponse.Data.Where(w => !w.IsSuccess).FirstOrDefault();

            string errorMessage = balanceResponse.ErrorLog;

            if (!balanceResponse.IsSuccess || failBalanceData != null)
            {
                if (failBalanceData != null)
                {
                    errorMessage = failBalanceData.ErrorLog;
                }

                return new BaseReturnDataModel<UserScore>(errorMessage);
            }

            // 取第三方用戶餘額資訊
            SportBalanceData balanceData = balanceResponse.Data.Where(w => w.IsSuccess).First();

            return new BaseReturnDataModel<UserScore>(
                ReturnCode.Success,
                new UserScore()
                {
                    AvailableScores = balanceData.Balance.GetValueOrDefault(),
                    FreezeScores = balanceData.Outstanding.GetValueOrDefault()
                });
        }

        protected override BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam param)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteCheckAccountExistApiResult(param.TPGameAccount);

            if (!returnModel.IsSuccess)
            {
                return new BaseReturnModel(returnModel.Message);
            }

            SportResponse<List<SportBalanceData>> balanceResponse = returnModel.DataModel.Deserialize<SportResponse<List<SportBalanceData>>>();

            if (balanceResponse.IsSuccess && balanceResponse.Data.Where(w => w.IsSuccess && w.TPGameAccount == param.TPGameAccount).Any())
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            BaseReturnDataModel<string> createAccountResult = GetRemoteCreateAccountApiResult(param);

            if (!createAccountResult.IsSuccess)
            {
                return new BaseReturnModel(createAccountResult.Message);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        protected override BaseReturnDataModel<string> GetRemoteForwardGameUrl(string tpGameAccount, string ip, bool isMobile, LoginInfo loginInfo)
        {
            BaseReturnDataModel<string> returnDataModel = GetRemoteLoginApiResult(tpGameAccount, ip, isMobile, loginInfo);

            if (!returnDataModel.IsSuccess)
            {
                return new BaseReturnDataModel<string>(returnDataModel.Message, string.Empty);
            }

            SportResponse<string> launchGameModel = returnDataModel.DataModel.Deserialize<SportResponse<string>>();

            if (!launchGameModel.IsSuccess)
            {
                return new BaseReturnDataModel<string>(launchGameModel.Message, null);
            }

            string fullLoginGameUrl = Url.Combine(launchGameModel.Data, "OType={oddsType}&lang=cs");

            return new BaseReturnDataModel<string>(ReturnCode.Success, fullLoginGameUrl);
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            throw new NotImplementedException();
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(string tpGameAccount)
            => GetBalanceApiResult(tpGameAccount);

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam param)
        {
            string url = GetSportServiceUrl().CreateMember;
            var registerData = new SportRegisterData
            {
                Vendor_Member_ID = param.TPGameAccount,
                OperatorId = param.TPGameAccount,
                UserName = param.TPGameAccount,
                OddsType = _sportAppSetting.OddsType,
                Currency = _sportAppSetting.Currency,
            };

            BaseReturnDataModel<string> returnModel = DoPostRequest(url, registerData.ToKeyValueURL(), out DetailRequestAndResponse detail);

            return returnModel;
        }

        protected override BaseReturnDataModel<string> GetRemoteLoginApiResult(string tpGameAccount, string ipAddress, bool isMobile, LoginInfo loginInfo)
        {
            string url = GetSportServiceUrl().GetSabaUrl;
            string platform = SabaSportPlatformTypes.PC.Value;

            if (isMobile)
            {
                platform = SabaSportPlatformTypes.MobileH5.Value;
            }

            string param = $@"vendor_member_id={tpGameAccount}&platform={platform}";

            BaseReturnDataModel<string> returnModel = DoPostRequest(url, param, out DetailRequestAndResponse detail);

            return returnModel;
        }

        /// <summary> 查询游戏总余额 </summary>
        private BaseReturnDataModel<string> GetBalanceApiResult(string tpGameAccount)
        {
            string url = GetSportServiceUrl().CheckUserBalance;
            string param = $@"vendor_member_ids={tpGameAccount}&wallet_id={_sportAppSetting.WalletId}";

            BaseReturnDataModel<string> returnModel = DoPostRequest(url, param, out DetailRequestAndResponse detail);

            return returnModel;
        }

        //private void SetUserBetAmountLimit(string tpGameAccount)
        //{
        //    string url = GetSportServiceUrl().GetMemberBetSetting;
        //    string param = $"vendor_member_id={tpGameAccount}";

        //    BaseReturnDataModel<string> returnModel = DoPostRequest(url, param);

        //    if (!returnModel.IsSuccess)
        //    {
        //        return;
        //    }

        //    var sportResponse = returnModel.DataModel.Deserialize<SportResponse<List<SportBetSettingItem>>>();

        //    if (!sportResponse.IsSuccess || !sportResponse.Data.Any())
        //    {
        //        return;
        //    }

        //    foreach (SportBetSettingItem data in sportResponse.Data)
        //    {
        //        data.Min_bet = 20;
        //        data.Max_bet = 20000;
        //        data.Max_bet_per_match = 80000;
        //    }

        //    url = GetSportServiceUrl().SetMemberBetSetting;
        //    param = $"vendor_member_id={tpGameAccount}&bet_setting={sportResponse.Data.ToJsonString()}";
        //    DoPostRequest(url, param);
        //}

        private BaseReturnDataModel<string> DoPostRequest(string url, string param, out DetailRequestAndResponse detail)
        {
            if (param.IsNullOrEmpty())
            {
                detail = new DetailRequestAndResponse();

                return new BaseReturnDataModel<string>(ReturnCode.ParameterIsInvalid);
            }

            string queryString = $"vendor_id={_sportAppSetting.MerchantCode}&{param}";

            var webRequestParam = new WebRequestParam()
            {
                Purpose = $"TPGService.{Product.Value}請求",
                Method = HttpMethod.Post,
                Url = url,
                Body = queryString,
                ContentType = HttpWebRequestContentType.WwwFormUrlencoded,
                IsResponseValidJson = true
            };

            string apiResult = HttpWebRequestUtil.GetResponse(webRequestParam, out HttpStatusCode httpStatusCode);
            detail = new DetailRequestAndResponse(webRequestParam, apiResult);

            return ConverToApiReturnDataModel(httpStatusCode, apiResult);
        }

        protected override PlatformHandicap ConvertToPlatformHandicap(string handicap)
        {
            SabaSportHandicap sabaSportHandicap = SabaSportHandicap.GetSingle(handicap);

            if (sabaSportHandicap == null)
            {
                return null;
            }

            return sabaSportHandicap.PlatformHandicap;
        }
    }
}