namespace JxBackendService.Model.Common
{
    public class RGSharedAppSetting : SharedAppSettings
    {
        public static readonly string RegisterMethod = "account.register";
        public static readonly string DepositMethod = "account.deposit";
        public static readonly string WithdrawMethod = "account.withdraw";
        public static readonly string BalanceMethod = "account.balance";
        public static readonly string TradeQueryMethod = "account.trade.query";
        public static readonly string OrderListMethod = "order.list";
        public static readonly string Currency = "CNY";

        private RGSharedAppSetting() { }

        public static string ServiceUrl => Get("RGServiceUrl");
        
        public static string LoginHost => Get("RGHostLUrl");
        
        public static string AppKey => Get("RGAppKey");
        
        public static string AppSecret => Get("RGAppSecret");
    }
}
