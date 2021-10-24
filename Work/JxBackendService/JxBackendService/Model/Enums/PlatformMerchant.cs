using JxBackendService.Model.Common;
using JxBackendService.Service.Enums.CTL;
using JxBackendService.Service.Enums.CTS;
using JxBackendService.Service.Merchant.CTL;
using JxBackendService.Service.Merchant.CTS;
using System;

namespace JxBackendService.Model.Enums
{
    public class PlatformMerchant : BaseStringValueModel<PlatformMerchant>
    {
        public CustomerTypes CustomerType { get; private set; }

        public string MerchantFolder { get; private set; }

        public Lazy<bool> IsUseMinifiedJavascriptFile { get; private set; }

        public Type TPGameAccountServiceType { get; private set; }

        public Type MerchantSettingServiceType { get; private set; }

        public Type UserRegisterServiceType { get; private set; }
        
        public Type ProfitLossTypeNameServiceType { get; private set; }

        public Type WalletTypeServiceType { get; private set; }


        private PlatformMerchant() { }

        public static PlatformMerchant CheerTechLottery = new PlatformMerchant()
        {
            Value = "CTL",
            MerchantFolder = string.Empty, //為了相容目前平台的資料夾
            CustomerType = CustomerTypes.Agent,
            IsUseMinifiedJavascriptFile = new Lazy<bool>(() => false),
            TPGameAccountServiceType = typeof(TPGameAccountService),
            MerchantSettingServiceType = typeof(MerchantSettingCTLService),
            ProfitLossTypeNameServiceType = typeof(ProfitLossTypeNameCTLService),
            WalletTypeServiceType = typeof(WalletTypeCTLService),
        };

        public static PlatformMerchant CheerTechSport = new PlatformMerchant()
        {
            Value = "CTS",
            MerchantFolder = "CTS",
            CustomerType = CustomerTypes.Direct,
            IsUseMinifiedJavascriptFile = new Lazy<bool>(() => PlatformMerchantHelper.GetUseMinifiedFileByEnvironment()),
            TPGameAccountServiceType = typeof(TPGameAccountCTSService),
            MerchantSettingServiceType = typeof(MerchantSettingCTSService),
            ProfitLossTypeNameServiceType = typeof(ProfitLossTypeNameCTSService),
            WalletTypeServiceType = typeof(WalletTypeCTSService),
        };
    }

    public static class PlatformMerchantHelper
    {
        public static bool GetUseMinifiedFileByEnvironment()
        {
            if (SharedAppSettings.GetEnvironmentCode(JxApplication.FrontSideWeb) == EnvironmentCode.Development)
            {
                return false;
            }

            return true;
        }
    }

    /// <summary> 搭配DB使用勿更動編號 </summary>
    public enum CustomerTypes
    {
        /// <summary>代理</summary>
        Agent = 0,
        /// <summary>直客</summary>
        Direct = 1,
    }
}

