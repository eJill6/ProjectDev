using JxBackendService.Model.Enums;

namespace JxBackendService.Interface.Service.Merchant
{
    public interface IMerchantSettingService
    {
        PlatformCulture PlatformCulture { get; }

        /// <summary>是否要計算有效投注額</summary>
        bool IsComputeAdmissionBetMoney { get; }
    }
}