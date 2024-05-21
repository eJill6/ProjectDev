namespace JxBackendService.Model.Common
{
    public class ABEBSharedAppSettings : SharedAppSettings
    {
        private ABEBSharedAppSettings()
        { }

        /* API List */

        public readonly string CreateAccountUrl = "check_or_create";

        public readonly string CheckAccountExistUrl = "check_or_create";

        public readonly string TransferUrl = "agent_client_transfer";

        public readonly string CheckTransferUrl = "query_transfer_state";

        public readonly string GetBalanceUrl = "get_balance";

        public readonly string GetBetLogUrl = "betlog_pieceof_histories_in30days";

        public readonly string LaunchGameUrl = "forward_game";

        public readonly string LaunchMobileGameUrl = "forward_game";

        public static ABEBSharedAppSettings Instance = new ABEBSharedAppSettings();

        public string ServiceUrl => Get("TPGame.ABEB.ServiceBaseUrl");

        public string AgentUserName => Get("TPGame.ABEB.AgentUserName");

        public string PropertyId => Get("TPGame.ABEB.PropertyId");

        public string DesKey => Get("TPGame.ABEB.DesKey");

        public string MD5Key => Get("TPGame.ABEB.MD5Key");
    }
}