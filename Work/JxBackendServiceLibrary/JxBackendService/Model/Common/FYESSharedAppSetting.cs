namespace JxBackendService.Model.Common
{
    public class FYESSharedAppSetting : SharedAppSettings
    {
        private FYESSharedAppSetting()
        { }

        public static readonly string RegisterUrl = "/api/user/register";

        public static readonly string LoginUrl = "/api/user/login";

        public static readonly string GetBalanceUrl = "/api/user/balance";

        public static readonly string TransferUrl = "/api/user/transfer";

        public static readonly string GetTransferInfoUrl = "/api/user/transferinfo";

        public static readonly string GetBetLogUrl = "/api/log/get";

        public static FYESSharedAppSetting Instance = new FYESSharedAppSetting();

        public static string Enabled => Get("TPGame.FYES.Enabled");

        public static string ServiceUrl => Get("TPGame.FYES.ServiceBaseUrl");

        public static string AuthorizationKey => Get("TPGame.FYES.AuthorizationKey");

        public static string Currency => Get("TPGame.FYES.Currency");

        public static string Language => Get("TPGame.FYES.Language");

        public static int BetLogPageSize => 50;
    }
}