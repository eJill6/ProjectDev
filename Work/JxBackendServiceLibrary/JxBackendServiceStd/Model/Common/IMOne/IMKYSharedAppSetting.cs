namespace JxBackendService.Model.Common.IMOne
{
    public class IMKYSharedAppSetting : BaseIMOneAppSetting
    {
        private IMKYSharedAppSetting()
        {
            MerchantCodeConfigKey = "TPGame.IMKY.MerchantCode";
            ServiceUrlConfigKey = "TPGame.IMKY.ServiceBaseUrl";
            LanguageConfigKey = "TPGame.IMKY.Language";
            CurrencyConfigKey = "TPGame.IMKY.Currency";
        }

        public static IMKYSharedAppSetting Instance = new IMKYSharedAppSetting();

        public override string ProductWallet => "603";

        public override string GameCode => "IMBG20001";
    }
}