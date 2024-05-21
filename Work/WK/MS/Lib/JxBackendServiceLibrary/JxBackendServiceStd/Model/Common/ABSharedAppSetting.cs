namespace JxBackendService.Model.Common
{
    public class ABEBSharedAppSetting : SharedAppSettings
    {
        private ABEBSharedAppSetting()
        { }

        /* API List */

        public static readonly string CreateAccountUrl = "CheckOrCreate";

        public static readonly string CheckAccountExistUrl = "CheckOrCreate";

        public static readonly string TransferUrl = "Transfer";

        public static readonly string CheckTransferUrl = "GetTransferState";

        public static readonly string GetBalanceUrl = "GetBalances";

        public static readonly string GetBetLogUrl = "PagingQueryBetRecords";

        public static readonly string LaunchGameUrl = "Login";

        public static string ServiceUrl => Get("TPGame.ABEB.ServiceBaseUrl");

        public static string AgentUserName => Get("TPGame.ABEB.AgentUserName");

        public static string OperatorId => Get("TPGame.ABEB.OperatorId");

        public static string Suffix => Get("TPGame.ABEB.Suffix");

        public static string ApiKey => Get("TPGame.ABEB.ApiKey");
    }
}