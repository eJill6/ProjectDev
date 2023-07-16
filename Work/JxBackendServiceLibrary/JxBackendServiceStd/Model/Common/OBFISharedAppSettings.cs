namespace JxBackendService.Model.Common
{
    public class OBFISharedAppSettings : SharedAppSettings
    {
        private OBFISharedAppSettings()
        { }

        /* API List */

        public readonly string CreateAccountUrl = "launchGameById";

        public readonly string TransferInUrl = "transferIn";

        public readonly string TransferOutUrl = "transferOut";

        public readonly string CheckTransferUrl = "queryOrderStatus";

        public readonly string GetBalanceUrl = "queryBalance";

        public readonly string GetBetLogUrl = "queryGameOrders";

        public readonly string LaunchGameUrl = "launchGameById";

        public static OBFISharedAppSettings Instance = new OBFISharedAppSettings();

        public string ServiceUrl => Get("TPGame.OBFI.ServiceBaseUrl");

        public string GrabServiceUrl => Get("TPGame.OBFI.GrabServiceBaseUrl");

        public string MerchantCode => Get("TPGame.OBFI.MerchantCode");

        public string SecretKey => Get("TPGame.OBFI.SecretKey");

        public string IV => Get("TPGame.OBFI.IV");
    }
}