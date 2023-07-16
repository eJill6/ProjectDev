namespace JxBackendService.Model.Common
{
    public class AWCSPSharedAppSetting : SharedAppSettings
    {
        private AWCSPSharedAppSetting()
        { }

        /* API  */

        /// <summary>
        /// 创建玩家
        /// </summary>
        public static string CreateMember => "wallet/createMember";

        /// <summary>
        /// 进入游戏
        /// </summary>
        public static string Login => "wallet/login";

        /// <summary>
        /// 登入并进入游戏
        /// </summary>
        public static string DoLoginAndLaunchGame => "wallet/doLoginAndLaunchGame";

        /// <summary>
        /// 取得玩家余额
        /// </summary>
        public static string GetBalance => "wallet/getBalance";

        /// <summary>
        /// 查詢转帐紀錄
        /// </summary>
        public static string CheckTransferOperation => "wallet/checkTransferOperation";

        /// <summary>
        /// 取款
        /// </summary>
        public static string Withdraw => "wallet/withdraw";

        /// <summary>
        /// 存款
        /// </summary>
        public static string Deposit => "wallet/deposit";

        /// <summary>
        /// 取得指定区间交易资料
        /// </summary>
        public static string GetTransactionByTxTime => "fetch/gzip/getTransactionByTxTime";

        /// <summary>
        /// 依注单更新时间取得交易资料
        /// </summary>
        public static string GetTransactionByUpdateDate => "fetch/gzip/getTransactionByUpdateDate";

        public static string Enabled => Get("TPGame.AWCSP.Enabled");

        public static string ServiceUrl => Get("TPGame.AWCSP.ServiceBaseUrl");

        public static string Cert => Get("TPGame.AWCSP.Cert");

        public static string AgentId => Get("TPGame.AWCSP.AgentId");
    }
}