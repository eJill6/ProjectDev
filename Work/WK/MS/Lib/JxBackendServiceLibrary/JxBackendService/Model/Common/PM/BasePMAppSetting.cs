namespace JxBackendService.Model.Common.PM
{
    public interface IPMAppSetting
    {
        string MerchantCode { get; }

        string ServiceUrl { get; }

        string GrabServiceUrl { get; }

        string SecretKey { get; }

        string IV { get; }

        int GameId { get; }

        string LaunchGameUrl { get; }
    }

    public abstract class BasePMAppSetting : SharedAppSettings, IPMAppSetting
    {
        /* API List */

        public static readonly string CreateAccountUrl = "launchGameById";

        public static readonly string TransferInUrl = "transferIn";

        public static readonly string TransferOutUrl = "transferOut";

        public static readonly string CheckTransferUrl = "queryOrderStatus";

        public static readonly string GetBalanceUrl = "queryBalance";

        public static readonly string GetBetLogUrl = "queryGameOrders";

        public abstract string LaunchGameUrl { get; }

        public BasePMAppSetting()
        { }

        public string ServiceUrlConfigKey { get; set; }

        public string GrabServiceUrlConfigKey { get; set; }

        public string MerchantCodeConfigKey { get; set; }

        public string SecretKeyConfigKey { get; set; }

        public string IVConfigKey { get; set; }

        public string ServiceUrl => Get(ServiceUrlConfigKey);

        public string GrabServiceUrl => Get(GrabServiceUrlConfigKey);

        public string MerchantCode => Get(MerchantCodeConfigKey);

        public string SecretKey => Get(SecretKeyConfigKey);

        public string IV => Get(IVConfigKey);

        public abstract int GameId { get; }
    }
}