namespace JxBackendService.Model.ThirdParty.JDB.JDBFI
{
    public class JDBFIGetBalanceRequestModel : JDBFIBaseRequestModel
    {
        public string PlayerId { get; set; }
    }

    /// <summary>
    /// 创建玩家
    /// </summary>
    public class JDBFIRegisterRequestModel : JDBFIBaseParentRequestModel
    {
        /// <summary>
        /// 玩家账号，只限a-z与0-9。
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// 玩家名称，不允许的特殊字符：大於小於#"'%-+=*/| and符號
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 账户初始额度（预设为 0）
        /// </summary>
        public string Credit_allocated { get; set; }
    }

    /// <summary>
    /// 查询玩家数据
    /// </summary>
    public class JDBFIUserInfoRequestModel : JDBFIBaseParentRequestModel
    {
        /// <summary>
        /// 玩家账号，只限a-z与0-9。
        /// </summary>
        public string Uid { get; set; }
    }

    public class JDBFICheckTransferRequestModel : JDBFIBaseParentRequestModel
    {
        /// <summary>
        /// 交易序号，限英文或数字
        /// </summary>
        public string SerialNo { get; set; }
    }

    public class JDBFITransferRequestModel : JDBFICheckTransferRequestModel
    {
        /// <summary>
        /// 玩家账号
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// 0：不全部提领（默认值）
        /// 1：全部提领（包含所有小数字金额）
        /// </summary>
        public string AllCashOutFlag => "0";

        /// <summary>
        /// 提款或存款金额
        /// 正数：存款
        /// 负数：提款
        /// 当 allCashOutFlag 为「0」时，此字段为必填
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    public class JDBFIGetBetLogRequestModel : JDBFIBaseParentRequestModel
    {
        public string Starttime { get; set; }

        public string Endtime { get; set; }

        public int[] GTypes { get; set; }
    }

    /// <summary>
    /// 获取游戏链接（Token）请求参数
    /// </summary>
    public class JDBFILaunchGameRequestModel : JDBFIGetBalanceRequestModel
    {
        /// <summary>
        /// 玩家账号
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// 语系
        /// </summary>
        public string Lang => "cn";

        /// <summary>
        /// 游戏类型
        /// </summary>
        public string GType { get; set; }

        /// <summary>
        /// 机台类型
        /// </summary>
        public string MType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark => string.Empty;

        /// <summary>
        /// 1：使用JDB游戏大厅（默认值）2：不使用JDB游戏大厅
        /// </summary>
        public int WindowMode => 2;

        /// <summary>
        /// 是否为手机 APP 进入游戏
        /// </summary>
        public bool IsAPP { get; set; }

        /// <summary>
        /// 游戏大厅网址
        /// </summary>
        public string LobbyURL => string.Empty;

        /// <summary>
        /// 默认音效开关
        /// </summary>
        public int Mute => 0;

        /// <summary>
        /// 棋牌游戏群组
        /// </summary>
        public string CardGameGroup => string.Empty;

        /// <summary>
        /// 是否显示币别符号
        /// </summary>
        public string IsShowDollarSign => "true";
    }
}