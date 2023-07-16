using JxBackendService.Service.Enums.MSL;
using JxBackendService.Service.Merchant.MSL;
using System;
using System.Collections.Generic;

namespace JxBackendService.Model.Enums
{
    public class PlatformMerchant : BaseStringValueModel<PlatformMerchant>
    {
        public Type TPGameAccountServiceType { get; private set; }

        public Type MerchantSettingServiceType { get; private set; }

        public Type ProfitLossTypeNameServiceType { get; private set; }

        private PlatformMerchant()
        { }

        public static readonly PlatformMerchant MiseLiveStream = new PlatformMerchant()
        {
            Value = "MSL",
            TPGameAccountServiceType = typeof(TPGameAccountMSLService),
            MerchantSettingServiceType = typeof(MerchantSettingMSLService),
            ProfitLossTypeNameServiceType = typeof(ProfitLossTypeNameMSLService)
        };
    }
}