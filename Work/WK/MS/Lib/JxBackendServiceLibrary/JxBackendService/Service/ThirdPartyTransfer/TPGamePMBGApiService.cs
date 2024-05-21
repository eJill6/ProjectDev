using JxBackendService.Model.Common.PM;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGamePMBGApiService : TPGamePMApiService
    {
        public override PlatformProduct Product => PlatformProduct.PMBG;

        public override IPMAppSetting AppSetting => PMBGSharedAppSetting.Instance;

        public TPGamePMBGApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }
    }
}