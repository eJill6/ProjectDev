namespace JxBackendService.Model.Common
{
    public class OBEBSharedAppSetting : SharedAppSettings
    {
        #region API 基礎接口

        /// <summary>创建游戏账号</summary>
        public static readonly string CreateAccountUrl = "/api/merchant/create/v2";

        /// <summary>转账上分</summary>
        public static readonly string DepositUrl = "/api/merchant/deposit/v1";

        /// <summary>转账下分</summary>
        public static readonly string WithdrawUrl = "/api/merchant/withdraw/v1";

        /// <summary>转账结果查询</summary>
        public static readonly string CheckTransferUrl = "/api/merchant/transfer/v1";

        /// <summary>进入游戏</summary>
        public static readonly string ForwardGameUrl = "/api/merchant/forwardGame/v2";

        /// <summary>快速進入遊戲(建帳號、上分、取得網址)</summary>
        public static readonly string FastGameUrl = "/api/merchant/fastGame/v2";

        /// <summary>取得用戶餘額</summary>
        public static readonly string GetBalanceUrl = "/api/merchant/balance/v1";

        #endregion API 基礎接口

        #region API 資料接口

        /// <summary>游戏记录</summary>
        public static readonly string BetHistoryRecordUrl = "/data/merchant/betHistoryRecord/v1";

        /// <summary>大赛游戏记录(时间区间)</summary>
        public static readonly string MatchAccountChangeUrl = "/data/merchant/matchAccountChange/v1";

        /// <summary>主播列表</summary>
        public static readonly string LivesUrl = "/data/merchant/lives/v2";

        /// <summary>活动彩金数据接口</summary>
        public static readonly string ActivityRecordUrl = "/data/merchant/activityRecord/v1";

        /// <summary>主播列表</summary>
        public static readonly string GetLivesUrl = "/data/merchant/lives/v2";

        /// <summary>会员离桌接口</summary>
        public static readonly string ForeLeaveTable = "/api/merchant/foreLeaveTable/v1";

        #endregion API 資料接口

        public static string ServiceBaseUrl => Get("TPGame.OBEB.ServiceBaseUrl");

        public static string DataServiceBaseUrl => Get("TPGame.OBEB.DataServiceBaseUrl");

        public static string MerchantName => Get("TPGame.OBEB.MerchantName");

        public static string MerchantCode => Get("TPGame.OBEB.MerchantCode");

        public static string AESKey => Get("TPGame.OBEB.AESKey");

        public static string MD5Key => Get("TPGame.OBEB.MD5Key");

        public static string NewDescription => Get("TPGame.OBEB.NewDescription");

        public static string NewAvatar => Get("TPGame.OBEB.NewAvatar");
    }
}