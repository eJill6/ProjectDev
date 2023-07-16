using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Common
{
    public class PGSLSharedAppSettings : SharedAppSettings
    {
        private PGSLSharedAppSettings()
        { }

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

        //public readonly string LunchGameUrlPath = "/web-lobby/smartbot/"; //0422更新為 /web-lobby/tournament/open/
        public readonly string LunchGameUrlPath = "/web-lobby/tournament/open/";

        public readonly string LunchMobileGameUrlPath = "/lobby/index.html";

        public static PGSLSharedAppSettings Instance = new PGSLSharedAppSettings();

        public string ServiceUrl => Get("TPGame.PGSL.ServiceBaseUrl");

        public string GrabServiceUrl => Get("TPGame.PGSL.GrabServiceBaseUrl");

        public string OperatorToken => Get("TPGame.PGSL.OperatorToken");

        public string SecretKey => Get("TPGame.PGSL.SecretKey");

        public string Currncy => Get("TPGame.PGSL.Currncy");

        public string LunchGameUrl => Get("TPGame.PGSL.LunchGameUrl");

        public string LunchMobileGameUrl => Get("TPGame.PGSL.LunchMobileGameUrl");
    }
}