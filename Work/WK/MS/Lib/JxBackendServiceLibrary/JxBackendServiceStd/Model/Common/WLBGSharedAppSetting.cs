namespace JxBackendService.Model.Common
{
    public class WLBGSharedAppSetting : SharedAppSettings
    {
        #region API 接口

        /// <summary> 用户注册 </summary>
        public static readonly string Register = "api/register";

        /// <summary> 进游戏 </summary>
        public static readonly string EnterGame = "api/enterGame";

        /// <summary> 强制登出玩家 </summary>
        public static readonly string Kick = "api/kick";

        /// <summary> 导出游戏记录 </summary>
        public static readonly string GetRecordV2 = "api/getRecordV2";

        /// <summary> 划拨 </summary>
        public static readonly string TransferV3 = "api/transferV3";

        /// <summary> 查询⽤户余额 </summary>
        public static readonly string GetBalance = "api/getBalance";

        /// <summary> 划拨查询 </summary>
        public static readonly string QueryOrderV3 = "api/queryOrderV3";

        /// <summary> 查询商户额度 </summary>
        public static readonly string GetAgentBalance = "api/getAgentBalance";

        #endregion API 接口

        #region 資料接口

        /// <summary> 查询游戏列表 </summary>
        public static readonly string GetGameList = "api/game/list";

        #endregion 資料接口

        public static string ServiceUrl => Get("TPGame.WLBG.ServiceBaseUrl");

        public static string DataServiceUrl => Get("TPGame.WLBG.DataServiceBaseUrl");

        public static string AgentID => Get("TPGame.WLBG.AgentID");

        public static string APIAccount => Get("TPGame.WLBG.APIAccount");

        public static string DataEncryptionKey => Get("TPGame.WLBG.DataEncryptionKey");

        public static string RequestSignKey => Get("TPGame.WLBG.RequestSignKey");
    }
}