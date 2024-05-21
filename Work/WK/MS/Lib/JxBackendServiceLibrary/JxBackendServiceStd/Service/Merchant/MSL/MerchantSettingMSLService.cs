using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Merchant.Base;

namespace JxBackendService.Service.Merchant.MSL
{
    public class MerchantSettingMSLService : BaseMerchantSettingService
    {
        public MerchantSettingMSLService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformCulture PlatformCulture => PlatformCulture.China;

        public override bool IsComputeAdmissionBetMoney => true;

        public override string GoogleAuthenticatorIssuer => "MSL";
    }
}