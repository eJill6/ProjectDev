namespace JxBackendService.Model.Common
{
    public class EVEBSharedAppSetting : SharedAppSettings
    {
        private EVEBSharedAppSetting()
        { }

        /* API List */

        public readonly string GetTokenUrl = "/V1/Account/Token";

        public readonly string CreateAccountUrl = "/V1/Member/CreateMember";

        public readonly string CheckAccountExistUrl = "/V1/Member/GetMember";

        public readonly string TransferUrl = "/V1/Member/Transfer";

        public readonly string CheckTransferUrl = "/V1/Report/CheckTransfer";

        public readonly string GetBalanceUrl = "/V1/Member/GetBalance";

        public readonly string GetBetLogUrl = "/V1/Report/GetBetRecord";

        public readonly string LaunchGameUrl = "/V1/Game/Login";

        public static EVEBSharedAppSetting Instance = new EVEBSharedAppSetting();

        public string ServiceUrl => Get("TPGame.EVEB.ServiceBaseUrl");

        public string MerchantCode => Get("TPGame.EVEB.MerchantCode");

        public string SecretKey => Get("TPGame.EVEB.SecretKey");
    }
}