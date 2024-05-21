using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Common.IMOne;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.IM;
using JxBackendService.Model.ThirdParty.IM.Lottery;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameIMKYApiService : TPGameIMOneApiService
    {
        public TPGameIMKYApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.IMKY;

        public override IIMOneAppSetting AppSetting => IMKYSharedAppSetting.Instance;

        protected override IMLaunchGameRequestModel GetIMLaunchGameRequestModel(string tpGameAccount, string ipAddress, string gameCode)
        {
            IMLaunchGameRequestModel model = base.GetIMLaunchGameRequestModel(tpGameAccount, ipAddress, gameCode);

            model.LobbyURL = DefaultReturnUrl;

            return model;
        }

        /// <summary>
        /// IMKY覆寫創帳號：因為第三方設計與其他家不同，創帳號後需登入一次遊戲，否則直接進行轉出會顯示賬號未激活
        /// </summary>
        protected override BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam createRemoteAccountParam)
        {
            BaseReturnDataModel<bool> checkAccountReturnModel = CheckAccountExist(createRemoteAccountParam);

            if (checkAccountReturnModel.IsSuccess)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            BaseReturnDataModel<string> returnModel = GetRemoteCreateAccountApiResult(createRemoteAccountParam);

            if (!returnModel.IsSuccess)
            {
                return new BaseReturnModel(returnModel.Message);
            }

            IMRegisterResponseModel registerModel = returnModel.DataModel.Deserialize<IMRegisterResponseModel>();

            if (registerModel.Code != IMOneResponseCode.Success)
            {
                return new BaseReturnModel(registerModel.Message);
            }

            var _ipUtilService = DependencyUtil.ResolveService<IIpUtilService>().Value;
            string ipAddress = _ipUtilService.GetIPAddress();

            var tpGameRemoteLoginParam = new TPGameRemoteLoginParam()
            {
                CreateRemoteAccountParam = createRemoteAccountParam,
                IpAddress = ipAddress,
                IsMobile = false,
                LoginInfo = new LoginInfo()                
            };

            BaseReturnDataModel<string> gameUrlResult = GetRemoteLoginApiResult(tpGameRemoteLoginParam);

            if (!gameUrlResult.IsSuccess)
            {
                return new BaseReturnDataModel<TPGameOpenParam>(gameUrlResult.Message);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }
    }
}