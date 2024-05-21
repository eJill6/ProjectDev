namespace JxBackendService.Model.Common.IMOne
{
    public interface IIMOneReportApiSetting
    {
        string GetBetLogPath { get; }

        string ReportServiceUrl { get; }
    }

    public interface IIMOneAppSetting : IIMOneReportApiSetting
    {
        string MerchantCode { get; }

        string ServiceUrl { get; }

        string Language { get; }

        string Currency { get; }

        string ProductWallet { get; }

        string GameCode { get; }
    }

    public class IMOneReportApiSetting : SharedAppSettings, IIMOneReportApiSetting
    {
        public string GetBetLogPath => "Report/GetBetLog";

        public string ReportServiceUrl => Get("IMOne.ReportServiceUrl");
    }

    public abstract class BaseIMOneAppSetting : IMOneReportApiSetting, IIMOneAppSetting
    {
        /* API List */

        public static readonly string CreateAccountUrl = "Player/Register";

        public static readonly string CheckAccountExistUrl = "Player/CheckExists";

        public static readonly string TransferUrl = "Transaction/PerformTransfer";

        public static readonly string CheckTransferUrl = "Transaction/CheckTransferStatus";

        public static readonly string GetBalanceUrl = "Player/GetBalance";

        public static readonly string LaunchGameUrl = "Game/NewLaunchGame";

        public static readonly string LaunchMobileGameUrl = "Game/NewLaunchMobileGame";

        public BaseIMOneAppSetting()
        { }

        public string MerchantCodeConfigKey { get; set; }

        public string ServiceUrlConfigKey { get; set; }

        public string LanguageConfigKey { get; set; }

        public string CurrencyConfigKey { get; set; }

        public string MerchantCode => Get(MerchantCodeConfigKey);

        public string ServiceUrl => Get(ServiceUrlConfigKey);

        public string Language => Get(LanguageConfigKey);

        public string Currency => Get(CurrencyConfigKey);

        public abstract string ProductWallet { get; }

        public abstract string GameCode { get; }
    }
}