using JxBackendService.Model.Common.IMOne;

namespace JxBackendService.Model.Common
{
    public abstract class IMLotterySharedAppSettings : BaseIMOneAppSetting
    {
        public IMLotterySharedAppSettings()
        {
            MerchantCodeConfigKey = GetMerchantCodeConfigKey();
            ServiceUrlConfigKey = GetServiceUrlConfigKey();
            LanguageConfigKey = GetLanguageConfigKey();
            CurrencyConfigKey = GetCurrencyConfigKey();
        }

        //因為實際上改用table開關沒用到, 故先關註解
        //public abstract string Enabled { get; }
        protected abstract string GetMerchantCodeConfigKey();

        protected abstract string GetServiceUrlConfigKey();

        protected abstract string GetLanguageConfigKey();

        protected abstract string GetCurrencyConfigKey();
    }

    public class IMVRAppSettings : IMLotterySharedAppSettings
    {
        private IMVRAppSettings()
        { }

        public static IMVRAppSettings Instance = new IMVRAppSettings();

        protected override string GetMerchantCodeConfigKey() => "TPGame.IMVR.MerchantCode";

        protected override string GetServiceUrlConfigKey() => "TPGame.IMVR.ServiceBaseUrl";

        protected override string GetLanguageConfigKey() => "TPGame.IMVR.Language";

        protected override string GetCurrencyConfigKey() => "TPGame.IMVR.Currency";

        public override string ProductWallet => "503";

        public override string GameCode => "imlotto20001";
    }

    public class IMSGAppSettings : IMLotterySharedAppSettings
    {
        private IMSGAppSettings()
        { }

        public static IMSGAppSettings Instance = new IMSGAppSettings();

        protected override string GetMerchantCodeConfigKey() => "TPGame.IMSG.MerchantCode";

        protected override string GetServiceUrlConfigKey() => "TPGame.IMSG.ServiceBaseUrl";

        protected override string GetLanguageConfigKey() => "TPGame.IMSG.Language";

        protected override string GetCurrencyConfigKey() => "TPGame.IMSG.Currency";

        public override string ProductWallet => "504";

        public override string GameCode => "imlotto30001";
    }
}