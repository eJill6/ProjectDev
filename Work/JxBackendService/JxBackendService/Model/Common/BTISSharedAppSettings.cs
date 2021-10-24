namespace JxBackendService.Model.Common
{
    public class BTISSharedAppSettings : FtpSharedSettings
    {
        private BTISSharedAppSettings() { }

        /* API List */

        //Wallet Api
        public readonly string CreateAccountUrl = "/CreateUser";
        
        public readonly string GetCustomerAuthTokenUrl = "/GetCustomerAuthToken";

        /// <summary>用戶充值</summary>
        public readonly string TransferToWHLUrl = "/TransferToWHL";

        /// <summary>用戶提現</summary>
        public readonly string TransferFromWHLUrl = "/TransferFromWHL";

        /// <summary>確認訂單狀態</summary>
        public readonly string CheckTransactionUrl = "/CheckTransaction";

        /// <summary>取得用戶餘額</summary>
        public readonly string GetBalanceUrl = "/GetBalance";

        //Data Api
        public readonly string GetTokenUrl = "/GetToken";
        
        public readonly string BettingHistoryUrl = "/BettingHistory";

        public static BTISSharedAppSettings Instance = new BTISSharedAppSettings();

        public string Enabled => Get("TPGame.BTIS.Enabled");
        
        public string ENTRANCE_URL => Get("TPGame.BTIS.ENTRANCE_URL");

        public string WALLET_API_URL => Get("TPGame.BTIS.WALLET_API_URL");

        public string DATA_API_URL => Get("TPGame.BTIS.DATA_API_URL");

        public string API_AgentUserName => Get("TPGame.BTIS.API_AgentUserName");

        public string API_AgentPassword => Get("TPGame.BTIS.API_AgentPassword");

        public string WALLET_API_CurrencyCode => Get("TPGame.BTIS.WALLET_API_CurrencyCode");

        public string WALLET_API_CountryCode => Get("TPGame.BTIS.WALLET_API_CountryCode");

        public string WALLET_API_Group1ID => Get("TPGame.BTIS.WALLET_API_Group1ID");

        public string WALLET_API_CustomerDefaultLanguage => Get("TPGame.BTIS.WALLET_API_CustomerDefaultLanguage");
    }
}
