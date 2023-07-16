using JxBackendService.Model.Common.PM;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGamePMSLApiService : TPGamePMApiService
    {
        public override PlatformProduct Product => PlatformProduct.PMSL;

        public override IPMAppSetting AppSetting => PMSLSharedAppSetting.Instance;

        public TPGamePMSLApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }
    }
}