namespace JxBackendService.Model.ThirdParty.WLBG
{
    /// <summary>
    /// register ⽤户注册
    /// </summary>
    public class WLBGRegisterRequestModel
    {
        /// <summary>
        /// ⽤户ID
        /// </summary>
        public string Uid { get; set; }
    }

    /// <summary>
    /// 查询玩家余额
    /// </summary>
    public class WLBGGetBalanceRequestModel
    {
        /// <summary>
        /// ⽤户ID
        /// </summary>
        public string Uid { get; set; }
    }

    public class WLBGCheckTransferRequestModel
    {
        /// <summary>
        ///划拨单号
        /// </summary>
        public string OrderId { get; set; }
    }

    public class WLBGTransferRequestModel : WLBGCheckTransferRequestModel
    {
        /// <summary>
        /// 玩家账号
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// 划拨⾦额
        /// </summary>
        public string Credit { get; set; }
    }

    public class WLBGGetBetLogRequestModel
    {
        public string From { get; set; }

        public string Until { get; set; }
    }

    /// <summary>
    /// enterGame 进游戏请求参数
    /// </summary>
    public class WLBGLaunchGameRequestModel
    {
        /// <summary>
        /// ⽤户 ID
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// （可选）进哪个游戏。游戏编号详见附表。不传则进⼊⽡⼒⼤厅。
        /// </summary>
        public string Game { get; set; }

        ///// <summary>
        ///// （可选）⽤户当前 IP，⽀持 ipV4、ipV6 格式。
        ///// </summary>
        //public string ip { get; set; }

        ///// <summary>
        ///// （可选）同时发起划拨：划拨单号。
        ///// </summary>
        //public string orderId { get; set; }

        ///// <summary>
        ///// （可选）同时发起划拨：划拨⾦额。
        ///// </summary>
        //public string credit { get; set; }
    }

    /// <summary>
    ///  查询游戏列表
    /// </summary>
    public class WLBGGetGameListRequestModel
    {
        /// <summary>
        /// API账号。可查看后台：api接⼊信息->API账号
        /// </summary>
        public string Aid { get; set; }

        /// <summary>
        /// 当前时间戳(单位:秒)
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// 签名。md5(aid+time+md5Key)。签名密钥md5Key可在查看后台：api接⼊信息->请求签名秘钥
        /// </summary>
        public string Sign { get; set; }
    }
}