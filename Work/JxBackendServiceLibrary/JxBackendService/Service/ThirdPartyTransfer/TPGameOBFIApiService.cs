using JxBackendService.Model.Common.PM;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameOBFIApiService : TPGamePMApiService
    {
        public override PlatformProduct Product => PlatformProduct.OBFI;

        public override IPMAppSetting AppSetting => OBFISharedAppSetting.Instance;

        public TPGameOBFIApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }
    }
}