using JxBackendService.Interface.Service.Merchant;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.Merchant.Base
{
    public abstract class BaseMerchantSettingService : BaseService, IMerchantSettingService
    {
        public BaseMerchantSettingService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public abstract PlatformCulture PlatformCulture { get; }

        public abstract bool IsComputeAdmissionBetMoney { get; }

        public abstract string GoogleAuthenticatorIssuer { get; }
    }
}