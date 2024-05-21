using JxBackendService.Common;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.IM;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Net;
using System.Web;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameIMBGApiService : BaseTPGameApiService
    {
        private static readonly string _apiAgentHandle = "agentHandle";

        //private static readonly string _apiLogHandle = "logHandle";
        private static readonly string _language = "ZH-CN";

        private readonly Lazy<IGameAppSettingService> _gameAppSettingService;

        public TPGameIMBGApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _gameAppSettingService = DependencyUtil.ResolveKeyed<IGameAppSettingService>(envLoginUser.Application, SharedAppSettings.PlatformMerchant);
        }

        public override PlatformProduct Product => PlatformProduct.IMBG;

        protected override bool IsAllowTransferCompensation => true;

        protected override string CreateTPGamePasswordByRule(int userId, string tpGameAccount) => null;

        public override BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            if (apiResult.IsNullOrEmpty())
            {
                LogUtilService.ForcedDebug($"GetQueryOrderReturnModel fail, apiResult is null");

                return null;
            }

            IMBGResponse<IMBGOrderInfoData> orderInfoResponse = apiResult.Deserialize<IMBGResponse<IMBGOrderInfoData>>();

            if (!orderInfoResponse.Data.IsSuccess)
            {
                return new BaseReturnModel(orderInfoResponse.Data.ErrorLog);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        protected override DetailRequestAndResponse GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            IIMBGAppSetting imbgAppSetting = _gameAppSettingService.Value.GetIMBGAppSetting();
            string param = DESEncode(imbgAppSetting.DESKey, $"ac={(int)IMBGActionCodes.QueryOrder}&userCode={tpGameAccount}&orderId={tpGameMoneyInfo.OrderID}");

            DoRequest(imbgAppSetting, param, _apiAgentHandle, out DetailRequestAndResponse detail);

            return detail;
        }

        public override DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, CreateRemoteAccountParam createRemoteAccountParam, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            IIMBGAppSetting imbgAppSetting = _gameAppSettingService.Value.GetIMBGAppSetting();
            IMBGActionCodes actionCode;

            if (isMoneyIn)
            {
                actionCode = IMBGActionCodes.Recharge;
            }
            else
            {
                actionCode = IMBGActionCodes.Withdraw;
            }

            string param = DESEncode(imbgAppSetting.DESKey, $"ac={(int)actionCode}&userCode={createRemoteAccountParam.TPGameAccount}" +
                $"&money={tpGameMoneyInfo.Amount}&orderId={tpGameMoneyInfo.OrderID}");

            DoRequest(imbgAppSetting, param, _apiAgentHandle, out DetailRequestAndResponse detail);
            return detail;
        }

        public override string GetRemoteUserScoreApiResult(CreateRemoteAccountParam createRemoteAccountParam, IInvocationUserParam invocationUserParam)
        {
            BaseReturnDataModel<string> returnDataModel = GetBalanceApiResult(createRemoteAccountParam.TPGameAccount);

            return returnDataModel.DataModel;
        }

        public override BaseReturnDataModel<UserScore> GetTransferReturnModel(string apiResult)
        {
            IMBGResponse<IMBGTransferData> tranferResponse = apiResult.Deserialize<IMBGResponse<IMBGTransferData>>();

            if (!tranferResponse.Data.IsSuccess)
            {
                return new BaseReturnDataModel<UserScore>(tranferResponse.Data.ErrorLog);
            }

            return new BaseReturnDataModel<UserScore>(ReturnCode.Success,
                new UserScore() { AvailableScores = tranferResponse.Data.FreeMoney });
        }

        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            IMBGResponse<IMBGBalanceData> balanceResponse = apiResult.Deserialize<IMBGResponse<IMBGBalanceData>>();

            if (!balanceResponse.Data.IsSuccess)
            {
                return new BaseReturnDataModel<UserScore>(balanceResponse.Data.ErrorLog, null);
            }

            return new BaseReturnDataModel<UserScore>(
                ReturnCode.Success,
                new UserScore()
                {
                    AvailableScores = balanceResponse.Data.FreeMoney
                });
        }

        protected override BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam createRemoteAccountParam)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteCheckAccountExistApiResult(createRemoteAccountParam);

            if (!returnModel.IsSuccess)
            {
                return new BaseReturnModel(returnModel.Message);
            }

            IMBGResponse<IMBGBalanceData> balanceResponse = returnModel.DataModel.Deserialize<IMBGResponse<IMBGBalanceData>>();

            if (balanceResponse.Data.IsSuccess)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            //以GetBalance來判斷用戶是否存在, 若Code回傳1012表示用戶不存在
            if (balanceResponse.Data.Code != IMBGResponseCodeType.IMBG_1012)
            {
                return new BaseReturnModel(balanceResponse.Data.ErrorLog);
            }

            BaseReturnDataModel<string> createAccountResult = GetRemoteCreateAccountApiResult(createRemoteAccountParam);

            if (!createAccountResult.IsSuccess)
            {
                return new BaseReturnModel(createAccountResult.Message);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        protected override BaseReturnDataModel<string> GetRemoteForwardGameUrl(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteLoginApiResult(tpGameRemoteLoginParam);

            if (!returnModel.IsSuccess)
            {
                return new BaseReturnDataModel<string>(returnModel.Message, string.Empty);
            }

            IMBGResponse<IMBGLoginData> launchGameModel = returnModel.DataModel.Deserialize<IMBGResponse<IMBGLoginData>>();

            if (!launchGameModel.Data.IsSuccess)
            {
                return new BaseReturnDataModel<string>(launchGameModel.Data.ErrorLog, null);
            }

            return new BaseReturnDataModel<string>(ReturnCode.Success, launchGameModel.Data.FullUrl);
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            throw new NotImplementedException();
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(CreateRemoteAccountParam createRemoteAccountParam)
            => GetBalanceApiResult(createRemoteAccountParam.TPGameAccount);

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            var ipUtilService = DependencyUtil.ResolveService<IIpUtilService>().Value;

            //以GetForwardGameUrl來建立用戶, 若用戶不存在, 取得遊戲Url時會同步建立

            BaseReturnDataModel<string> gameUrlResult = GetRemoteForwardGameUrl(new TPGameRemoteLoginParam()
            {
                CreateRemoteAccountParam = createRemoteAccountParam,
                IpAddress = ipUtilService.GetIPAddress(),
                IsMobile = true,
                LoginInfo = null
            });

            return gameUrlResult;
        }

        protected override BaseReturnDataModel<string> GetRemoteLoginApiResult(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            IIMBGAppSetting imbgAppSetting = _gameAppSettingService.Value.GetIMBGAppSetting();

            string queryString = $"ac={(int)IMBGActionCodes.Login}&userCode={tpGameRemoteLoginParam.CreateRemoteAccountParam.TPGameAccount}" +
                $"&ip={tpGameRemoteLoginParam.IpAddress}&gameId=0&lang={_language}";

            string param = DESEncode(imbgAppSetting.DESKey, queryString);
            BaseReturnDataModel<string> returnModel = DoRequest(imbgAppSetting, param, _apiAgentHandle, out DetailRequestAndResponse detail);

            return returnModel;
        }

        protected override void DoKickUser(Model.Entity.ThirdPartyUserAccount thirdPartyUserAccount) => new NotImplementedException();

        protected override bool IsDoTransferCompensation(string apiResult)
        {
            IMBGResponse<IMBGTransferData> tranferResponse = apiResult.Deserialize<IMBGResponse<IMBGTransferData>>();

            // 用户还在游戏中，下分失败
            if (!tranferResponse.Data.IsSuccess &&
                tranferResponse.Data.Code == IMBGResponseCodeType.IMBG_1013.Value)
            {
                return true;
            }

            return false;
        }

        private BaseReturnDataModel<string> DoRequest(IIMBGAppSetting imbgAppSetting, string param, string apiHandle,
            out DetailRequestAndResponse detail)
        {
            long timestamp = DateTime.UtcNow.ToChinaDateTime().ToUnixOfTime();
            string sign = MD5Tool.MD5EncodingForIMBGGameProvider($"{imbgAppSetting.MerchantCode}{timestamp}{imbgAppSetting.MD5Key}");
            string url = $"{imbgAppSetting.ServiceUrl}{apiHandle}?agentId={imbgAppSetting.MerchantCode}&timestamp={timestamp}&param={param}&sign={sign}";

            var webRequestParam = new WebRequestParam()
            {
                Purpose = $"TPGService.{Product.Value}請求",
                Method = HttpMethod.Get,
                ContentType = HttpWebRequestContentType.WwwFormUrlencoded,
                Url = url,
                TimeOut = 20 * 1000,
                IsResponseValidJson = true
            };

            string apiResult = HttpWebRequestUtilService.GetResponse(webRequestParam, out HttpStatusCode httpStatusCode);
            detail = new DetailRequestAndResponse(webRequestParam, apiResult);

            return ConvertToApiReturnDataModel(httpStatusCode, apiResult);
        }

        private string DESEncode(string desKey, string content)
        {
            var desTool = new DESTool(desKey);

            return HttpUtility.UrlEncode(desTool.DESEnCode(content,
                System.Security.Cryptography.CipherMode.ECB,
                isReturnBase64String: true));
        }

        private BaseReturnDataModel<string> GetBalanceApiResult(string tpGameAccount)
        {
            IIMBGAppSetting imbgAppSetting = _gameAppSettingService.Value.GetIMBGAppSetting();
            string param = DESEncode(imbgAppSetting.DESKey, $"ac={(int)IMBGActionCodes.Balance}&userCode={tpGameAccount}");

            return DoRequest(imbgAppSetting, param, _apiAgentHandle, out DetailRequestAndResponse detail);
        }
    }
}