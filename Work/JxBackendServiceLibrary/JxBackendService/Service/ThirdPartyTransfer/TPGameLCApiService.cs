using JxBackendService.Common;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.LC;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Net;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameLCApiService : BaseTPGameApiService
    {
        private readonly ILCAppSetting _lcAppSetting;

        private static readonly int s_updateUserScoreFail = 33;

        protected override bool IsAllowTransferCompensation => true;

        public TPGameLCApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            var gameAppSettingService = DependencyUtil.ResolveKeyed<IGameAppSettingService>(envLoginUser.Application, SharedAppSettings.PlatformMerchant);
            _lcAppSetting = gameAppSettingService.GetLCAppSetting();
        }

        public override PlatformProduct Product => PlatformProduct.LC;

        public override BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            LCResponse<LCTransferData> tranferResponse = apiResult.Deserialize<LCResponse<LCTransferData>>();

            if (!tranferResponse.Data.IsSuccess)
            {
                return null;
            }

            if (!tranferResponse.Data.Status.HasValue)
            {
                return null;
            }

            //判斷訂單狀態
            LCOrderStatus lcOrderStatus = LCOrderStatus.GetSingle(tranferResponse.Data.Status.Value);

            if (lcOrderStatus == null || lcOrderStatus == LCOrderStatus.Processing)
            {
                return null;
            }

            if (lcOrderStatus != LCOrderStatus.Success)
            {
                return new BaseReturnModel(lcOrderStatus.Name);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        protected override DetailRequestAndResponse GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            string orderId = CreateRemoteOrderID(tpGameAccount, tpGameMoneyInfo);
            string param = $@"s={(int)LCApiTypes.OrderStatus}&orderid={orderId}";
            DoRequest(param, out DetailRequestAndResponse detail);

            return detail;
        }

        protected override DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            LCApiTypes actionCode;

            if (isMoneyIn)
            {
                actionCode = LCApiTypes.Recharge;
            }
            else
            {
                actionCode = LCApiTypes.Withdraw;
            }

            string orderId = CreateRemoteOrderID(tpGameAccount, tpGameMoneyInfo);
            string param = $@"s={(int)actionCode}&account={tpGameAccount}&orderid={orderId}&money={tpGameMoneyInfo.Amount}";
            DoRequest(param, out DetailRequestAndResponse detail);

            return detail;
        }

        protected override string GetRemoteUserScoreApiResult(string tpGameAccount)
        {
            BaseReturnDataModel<string> returnDataModel = GetBalanceApiResult(tpGameAccount);

            return returnDataModel.DataModel;
        }

        public override BaseReturnDataModel<UserScore> GetTransferReturnModel(string apiResult)
        {
            LCResponse<LCTransferData> tranferResponse = apiResult.Deserialize<LCResponse<LCTransferData>>();

            if (!tranferResponse.Data.IsSuccess)
            {
                return new BaseReturnDataModel<UserScore>(tranferResponse.Data.ErrorLog);
            }

            return new BaseReturnDataModel<UserScore>(ReturnCode.Success, new UserScore() { AvailableScores = tranferResponse.Data.Money.Value });
        }

        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            LCResponse<LCBalanceData> balanceResponse = apiResult.Deserialize<LCResponse<LCBalanceData>>();

            if (!balanceResponse.Data.IsSuccess)
            {
                return new BaseReturnDataModel<UserScore>(balanceResponse.Data.ErrorLog, null);
            }

            return new BaseReturnDataModel<UserScore>(
                ReturnCode.Success,
                new UserScore()
                {
                    AvailableScores = balanceResponse.Data.TotalMoney.Value,
                    FreezeScores = balanceResponse.Data.TotalMoney.Value - balanceResponse.Data.FreeMoney.Value
                });
        }

        protected override BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam param)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteCheckAccountExistApiResult(param.TPGameAccount);

            if (!returnModel.IsSuccess)
            {
                return new BaseReturnModel(returnModel.Message);
            }

            LCResponse<LCBalanceData> balanceResponse = returnModel.DataModel.Deserialize<LCResponse<LCBalanceData>>();

            if (balanceResponse.Data.IsSuccess)
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

            LCResponse<LCLoginData> launchGameModel = returnDataModel.DataModel.Deserialize<LCResponse<LCLoginData>>();

            if (!launchGameModel.Data.IsSuccess)
            {
                return new BaseReturnDataModel<string>(launchGameModel.Data.ErrorLog, null);
            }

            return new BaseReturnDataModel<string>(ReturnCode.Success, launchGameModel.Data.Url);
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            throw new NotImplementedException();
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(string tpGameAccount)
            => GetBalanceApiResult(tpGameAccount);

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam param)
        {
            var ipUtilService = DependencyUtil.ResolveService<IIpUtilService>();

            BaseReturnDataModel<string> gameUrlResult = GetRemoteForwardGameUrl(
                param.TPGameAccount,
                ipUtilService.GetIPAddress(),
                isMobile: true,
                loginInfo: null);

            return gameUrlResult;
        }

        protected override BaseReturnDataModel<string> GetRemoteLoginApiResult(string tpGameAccount, string ipAddress, bool isMobile, LoginInfo loginInfo)
        {
            DateTime time = DateTime.UtcNow.ToChinaDateTime();
            string orderId = $"{_lcAppSetting.MerchantCode}{time.ToFormatDateTimeStringWithoutSymbol()}{tpGameAccount}";

            string remoteGameCode = "0";

            if (!loginInfo.RemoteCode.IsNullOrEmpty())
            {
                remoteGameCode = loginInfo.RemoteCode;
            }

            string param = $@"s={(int)LCApiTypes.Login}&account={tpGameAccount}&money=0&lineCode={_lcAppSetting.Linecode}&ip={ipAddress}&orderid={orderId}&KindID={remoteGameCode}&lang=zh-CN";

            BaseReturnDataModel<string> returnModel = DoRequest(param, out DetailRequestAndResponse detail);

            return returnModel;
        }

        protected override bool IsDoTransferCompensation(string apiResult)
        {
            var tranferResponse = apiResult.Deserialize<LCResponse<LCTransferData>>();

            if (!tranferResponse.Data.IsSuccess &&
                tranferResponse.Data.Code.HasValue &&
                tranferResponse.Data.Code.Value == s_updateUserScoreFail)
            {
                return true;
            }

            return false;
        }

        /// <summary> 查询游戏总余额 </summary>
        private BaseReturnDataModel<string> GetBalanceApiResult(string tpGameAccount)
        {
            string param = $@"s={(int)LCApiTypes.TotalBalance}&account={tpGameAccount}";

            BaseReturnDataModel<string> returnModel = DoRequest(param, out DetailRequestAndResponse detail);

            return returnModel;
        }

        private string CreateRemoteOrderID(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            return $"{_lcAppSetting.MerchantCode}{tpGameMoneyInfo.OrderID}{tpGameAccount}";
        }

        private BaseReturnDataModel<string> DoRequest(string param, out DetailRequestAndResponse detail)
        {
            long timestamp = DateTime.UtcNow.ToChinaDateTime().ToUnixOfTime();

            if (param.IsNullOrEmpty())
            {
                detail = new DetailRequestAndResponse();

                return new BaseReturnDataModel<string>(ReturnCode.ParameterIsInvalid);
            }

            var desParam = AESTool.Encrypt(param, _lcAppSetting.DESKey, isEncodeUrl: true);
            var desMd5 = MD5Tool.MD5EncodingForLCGameProvider($"{_lcAppSetting.MerchantCode}{timestamp}{_lcAppSetting.MD5Key}");
            string url = $"{_lcAppSetting.ServiceUrl}?agent={_lcAppSetting.MerchantCode}&timestamp={timestamp}&param={desParam}&key={desMd5}";

            var webRequestParam = new WebRequestParam()
            {
                Purpose = $"TPGService.{Product.Value}請求",
                Method = HttpMethod.Get,
                ContentType = HttpWebRequestContentType.WwwFormUrlencoded,
                Url = url,
                TimeOut = 20 * 1000,
                IsResponseValidJson = true
            };

            string apiResult = HttpWebRequestUtil.GetResponse(webRequestParam, out HttpStatusCode httpStatusCode);
            detail = new DetailRequestAndResponse(webRequestParam, apiResult);

            return ConverToApiReturnDataModel(httpStatusCode, apiResult);
        }
    }
}