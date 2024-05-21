namespace JxBackendService.Model.Common
{
    public class PGSLSharedAppSetting : SharedAppSettings
    {
        private PGSLSharedAppSetting()
        { }

        /// <summary>資料加密Key</summary>
        public readonly string EncryptKey = "!@#$2wsx";

        /// <summary>
        /// URL = ServiceUrl + UrlPathPrefix + CreateAccountUrl
        /// </summary>
        public readonly string UrlPathPrefix = "/external";

        /// <summary>
        /// URL = GrabServiceUrl + GrabUrlPathPrefix + GetBetLogUrl
        /// </summary>
        public readonly string GrabUrlPathPrefix = "/external-datagrabber";

        /* API List */

        public readonly string CreateAccountUrl = "/v1/Player/Create";

        public readonly string TransferInUrl = "/v3/Cash/TransferIn";

        public readonly string TransferOutUrl = "/v3/Cash/TransferOut";

        public readonly string CheckTransferUrl = "/v3/Cash/GetSingleTransaction";

        public readonly string GetBalanceUrl = "/v3/Cash/GetPlayerWallet";

        public readonly string GetBetLogUrl = "/Bet/v4/GetHistory";

        public readonly string LaunchGameUrlPath = "/external-game-launcher/api/v1/GetLaunchURLHTML";

        public readonly string WebLobbyPath = "/web-lobby/";

        public readonly string WebLobbyUrlType = "web-lobby";

        public readonly string WebGameUrlType = "game-entry";

        private readonly string _launchGameUrlPath = "/{0}/index.html";

        public string FormLaunchGameUrlPath(string remoteGameCode) => string.Format(_launchGameUrlPath, remoteGameCode);

        public static PGSLSharedAppSetting Instance = new PGSLSharedAppSetting();

        public string ServiceUrl => Get("TPGame.PGSL.ServiceBaseUrl");

        public string GrabServiceUrl => Get("TPGame.PGSL.GrabServiceBaseUrl");

        public string OperatorToken => Get("TPGame.PGSL.OperatorToken");

        public string SecretKey => Get("TPGame.PGSL.SecretKey");

        public string Currncy => Get("TPGame.PGSL.Currncy");

        public string LaunchGameUrl => Get("TPGame.PGSL.LunchGameUrl");

        public string LaunchMobileGameUrl => Get("TPGame.PGSL.LunchMobileGameUrl");
    }
}