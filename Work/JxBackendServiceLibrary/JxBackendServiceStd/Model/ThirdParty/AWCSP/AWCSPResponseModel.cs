using System.Collections.Generic;

namespace JxBackendService.Model.ThirdParty.AWCSP
{
    /// <summary>
    /// 注册接口返回数据
    /// </summary>
    public class AWCSPRegisterResponseModel : AWCSPBaseResponseModel
    {
    }

    /// <summary>
    /// 玩家余额
    /// </summary>
    public class AWCSPUserBalanceResponseModel : AWCSPBaseResponseModel
    {
        /// <summary>
        /// 查询时间 (使用 ISO 8601 格式)
        /// </summary>
        public string Querytime { get; set; }

        /// <summary>
        /// 用户数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 回传的数据组
        /// </summary>
        public List<AWCSPUserBalanceItem> Results { get; set; }
    }

    /// <summary>
    /// 查询玩家余额回传的数据组
    /// </summary>
    public class AWCSPUserBalanceItem
    {
        /// <summary>
        /// 玩家账号
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 如果状态是成功，则传回「balance」数据
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 当前余额上次修改时间 (ISO 8601格式)
        /// </summary>
        public string LastModified { get; set; }
    }

    /// <summary>
    /// 取得 Token URL
    /// </summary>
    public class AWCSPLaunchGameResponseModel : AWCSPBaseResponseModel
    {
        /// <summary>
        /// 登入URL
        /// </summary>
        public string Url { get; set; }
    }

    public class AWCSPTransferResponseModel : AWCSPBaseResponseModel
    {
        /// <summary>
        /// 玩家总余额
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// UUID in database资料库识别码
        /// </summary>
        public string DatabaseId { get; set; }

        /// <summary>
        /// 存款后余额
        /// </summary>
        public decimal CurrentBalance { get; set; }

        /// <summary>
        /// 当前余额上次修改时间(ISO 8601格式)
        /// </summary>
        public string LastModified { get; set; }

        /// <summary>
        ///  唯一个交易识别码
        /// </summary>
        public string TxCode { get; set; }

        /// <summary>
        /// 存款总额
        /// </summary>
        public decimal Amount { get; set; }
    }

    /// <summary>
    /// 转帐紀錄
    /// </summary>
    public class AWCSPCheckTransferResponseModel : AWCSPBaseResponseModel
    {
        /// <summary>
        /// 交易类型
        /// 0:transfer fail 转帐失败
        /// 1:transfer success转帐成功
        /// </summary>
        public string TxStatus { get; set; }

        /// <summary>
        /// 交易完成后余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 转帐金额
        /// </summary>
        public decimal TransferAmount { get; set; }

        /// <summary>
        /// 存款 /取款
        /// </summary>
        public string TransferType { get; set; }

        /// <summary>
        /// 交易识别码
        /// </summary>
        public string TxCode { get; set; }
    }

    public class AWCSPBetLogResponseModel : AWCSPBaseResponseModel
    {
        public List<AWCSPBetLog> Transactions { get; set; }
    }
}