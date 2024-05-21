using JxBackendService.Model.Common;
using JxBackendService.Model.Common.IMOne;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameIMSGApiService : TPGameIMLotteryApiService
    {
        public override PlatformProduct Product => PlatformProduct.IMSG;

        public override IIMOneAppSetting AppSetting => IMSGAppSettings.Instance;

        protected override int LotteryID => 999019;

        public TPGameIMSGApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }
    }
}