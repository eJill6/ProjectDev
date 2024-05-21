using JxBackendService.Model.Common.IMOne;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameIMPPApiService : TPGameIMOneApiService
    {
        
        public TPGameIMPPApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public override PlatformProduct Product => PlatformProduct.IMPP;
        
        public override IIMOneAppSetting AppSetting => _gameAppSettingService.GetIMPPAppSetting();
    }
}
