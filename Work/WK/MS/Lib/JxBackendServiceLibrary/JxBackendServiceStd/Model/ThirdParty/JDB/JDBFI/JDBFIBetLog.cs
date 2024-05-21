using JxBackendService.Common.Util;
using JxBackendService.Model.ThirdParty.Base;

namespace JxBackendService.Model.ThirdParty.JDB.JDBFI
{
    public class JDBFIBetLog : BaseRemoteBetLog
    {
        /// <summary>
        /// 游戏序号
        /// </summary>
        public string SeqNo { get; set; }

        /// <summary>
        /// 玩家账号
        /// </summary>
        public string PlayerId { get; set; }

        /// <summary>
        /// 游戏类型
        /// </summary>
        public string GType { get; set; }

        /// <summary>
        /// 机台类型
        /// </summary>
        public string MType { get; set; }

        /// <summary>
        /// 游戏时间（dd-MM-yyyy HH:mm:ss）
        /// </summary>
        public string GameDate { get; set; }

        /// <summary>
        /// 游戏区域
        /// </summary>
        public string RoomType { get; set; }

        /// <summary>
        /// 货币别
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 押注扣減金額
        /// </summary>
        public decimal Bet { get; set; }

        /// <summary>
        /// 用戶押注金額
        /// </summary>
        public decimal GetBetMoney() => -Bet;

        /// <summary>
        /// 游戏赢分
        /// </summary>
        public decimal Win { get; set; }

        /// <summary>
        /// 总输赢
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// 投注面值
        /// </summary>
        public decimal Denom { get; set; }

        /// <summary>
        /// 进场金额
        /// </summary>
        public decimal BeforeBalance { get; set; }

        /// <summary>
        /// 离场金额
        /// </summary>
        public decimal AfterBalance { get; set; }

        /// <summary>
        /// 最后修改时间（dd-MM-yyyy HH:mm:ss）
        /// </summary>
        public string LastModifyTime { get; set; }

        /// <summary>
        /// 玩家登入 IP
        /// </summary>
        public string PlayerIp { get; set; }

        /// <summary>
        /// 玩家从网页或行动装置登入
        /// </summary>
        public string ClientType { get; set; }

        /// <summary>
        /// 游戏序号
        /// </summary>
        public string HistoryId { get; set; }

        public override string KeyId
        {
            get
            {
                if (!HistoryId.IsNullOrEmpty())
                {
                    return HistoryId;
                }
                else
                {
                    return SeqNo;
                }
            }
        }

        public override string TPGameAccount => PlayerId;
    }
}