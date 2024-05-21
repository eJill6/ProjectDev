using JxBackendService.Model.ThirdParty.Base;
using System.Collections.Generic;

namespace JxBackendService.Model.ThirdParty.JDB.JDBFI
{
    /// <summary>
    /// 注册接口返回数据
    /// </summary>
    public class JDBFIRegisterResponseModel : JDBFIBaseResponseModel
    {
    }

    public class JDBFIUserInfoResponseModel : JDBFIBaseResponseModel
    {
        /// <summary>
        /// 回传的数据组
        /// </summary>
        public List<JDBFIUserInfoItem> data { get; set; }
    }

    /// <summary>
    /// 查询玩家数据回传的数据组
    /// </summary>
    public class JDBFIUserInfoItem
    {
        /// <summary>
        /// 玩家账号
        /// </summary>
        public string uid { get; set; }

        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal balance { get; set; }

        /// <summary>
        /// 代理账号
        /// </summary>
        public string parent { get; set; }

        /// <summary>
        /// 玩家名称
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 货币别
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        /// 账户型态
        /// </summary>
        public int lvl { get; set; }

        /// <summary>
        /// 账户状态
        /// </summary>
        public int locked { get; set; }

        /// <summary>
        /// 账户启用关闭
        /// </summary>
        public int closed { get; set; }

        /// <summary>
        /// 是否使用 Jackpot（有设定 Jackpot 的支线才有影响）
        /// </summary>
        public int jackpotFlag { get; set; }
    }

    /// <summary>
    /// 取得 Token URL
    /// </summary>
    public class JDBFILaunchGameResponseModel : JDBFIBaseResponseModel
    {
        /// <summary>
        /// 登入URL
        /// </summary>
        public string Path { get; set; }
    }

    public class JDBFITransferResponseModel : JDBFIBaseResponseModel
    {
        /// <summary>
        /// 玩家总余额
        /// </summary>
        public decimal UserBalance { get; set; }

        /// <summary>
        /// 代理现金余额
        /// </summary>
        public decimal AgentCashBalance { get; set; }

        /// <summary>
        /// 提领现金
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 输入的交易序号
        /// </summary>
        public string SerialNo { get; set; }

        /// <summary>
        /// 交易号码：每次交易都会有一个专属的号码
        /// </summary>
        public string Pid { get; set; }

        /// <summary>
        /// 交易日期（dd-MM-yyyy HH:mm:ss）
        /// </summary>
        public string PayDate { get; set; }
    }

    public class JDBFIBetLogResponseModel : JDBFIBaseResponseModel
    {
        public List<JDBFIBetLog> Data { get; set; }
    }
}