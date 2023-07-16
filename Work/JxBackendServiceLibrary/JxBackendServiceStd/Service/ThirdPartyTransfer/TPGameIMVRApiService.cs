using JxBackendService.Model.Common;
using JxBackendService.Model.Common.IMOne;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameIMVRApiService : TPGameIMLotteryApiService
    {
        public override PlatformProduct Product => PlatformProduct.IMVR;

        public override IIMOneAppSetting AppSetting => IMVRAppSettings.Instance;

        protected override int LotteryID => 999020;

        public override BaseReturnModel GetAllowCreateTransferOrderResult() => new BaseReturnModel(ReturnCode.Success);

        public TPGameIMVRApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            
        }
    }
}
