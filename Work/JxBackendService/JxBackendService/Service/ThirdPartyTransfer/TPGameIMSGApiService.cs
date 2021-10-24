using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public class TPGameIMSGApiService : TPGameIMLotteryApiService
    {
        public override PlatformProduct Product => PlatformProduct.IMSG;

        public override IMLotterySharedAppSettings AppSettings => IMSGAppSettings.Instance;

        protected override int LotteryID => 999019;

        public TPGameIMSGApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {

        }
    }    
}
