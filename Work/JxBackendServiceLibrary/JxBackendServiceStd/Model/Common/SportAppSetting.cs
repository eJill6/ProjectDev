namespace JxBackendService.Model.Common
{
    public interface ISportAppSetting
    {
        string MerchantCode { get; }

        string ServiceUrl { get; }

        string LoginBaseUrl { get; }

        string Currency { get; }

        string OddsType { get; }

        int WalletId { get; }
    }

    public class SportAppSetting : SharedAppSettings, ISportAppSetting
    {
        public string MerchantCodeConfigKey { get; set; }

        public string ServiceUrlConfigKey { get; set; }

        public string LoginBaseUrlConfigKey { get; set; }

        public string CurrencyConfigKey { get; set; }

        public string OddsTypeConfigKey { get; set; }

        public string MerchantCode => Get(MerchantCodeConfigKey);

        public string ServiceUrl => Get(ServiceUrlConfigKey);

        public string LoginBaseUrl => Get(LoginBaseUrlConfigKey);

        public string Currency => Get(CurrencyConfigKey);

        public string OddsType => Get(OddsTypeConfigKey);

        /// <summary> 产品代碼 1 : Sportsbook、5 : AG </summary>
        public int WalletId => 1;
    }

    public class SportServiceUrl
    {
        public string CreateMember { get; set; }

        public string CheckUserBalance { get; set; }

        public string GetSabaUrl { get; set; }

        public string CheckFundTransfer { get; set; }

        public string FundTransfer { get; set; }

        public string GetMemberBetSetting { get; set; }

        public string SetMemberBetSetting { get; set; }
    }
}
