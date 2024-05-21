using JxBackendService.Common.Extensions;
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

        protected override string ProcessLaunchGameUrl(string launchGameUrl)
        {
            return launchGameUrl
                .ExtendQueryParam("backurl", DefaultReturnUrl)
                .ExtendQueryParam("jumpType", "1"); // 1:可以外跳
        }
    }
}