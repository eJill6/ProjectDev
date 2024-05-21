namespace JxBackendService.Model.Common
{
    public class OBSPSharedAppSetting : SharedAppSettings
    {
        private OBSPSharedAppSetting()
        { }

        public static string ApiRootUrl => Get("TPGame.OBSP.ApiRootUrl");

        public static string MerchantCode => Get("TPGame.OBSP.merchantCode");

        public static string Key => Get("TPGame.OBSP.Key");

        public static string Currency => Get("TPGame.OBSP.currency");

        public static readonly string UserCreateActionUrl = "/api/user/create";

        public static readonly string UserLoginActionUrl = "/api/user/login";

        public static readonly string FundCheckBalanceActionUrl = "/api/fund/checkBalance";

        public static readonly string FundTransferActionUrl = "/api/fund/transfer";

        public static readonly string FundGetTransferRecordActionUrl = "/api/fund/getTransferRecord";

        public static readonly string BetQueryBetListActionUrl = "/api/bet/queryBetList";
    }
}