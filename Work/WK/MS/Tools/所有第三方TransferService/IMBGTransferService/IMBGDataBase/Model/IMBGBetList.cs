using Newtonsoft.Json;
using System.Collections.Generic;

namespace IMBGDataBase.Model
{
    public class IMBGBetList<U> : RespData
    {
        /// <summary>
        /// 数据结构返回值
        /// </summary>
        [JsonProperty("list")]
        public List<U> List { get; set; }
    }

    public class IMBGBetLog
    {
        /// <summary>
        /// 注单号（玩家下注记录标识，全平台唯一值，不会重复）
        /// 20210506，出現 Could not convert string to integer: 2147484456 ，已超過 int最大值，改為long型態
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// 代理商编号
        /// </summary>
        [JsonProperty("agentId")]
        public string AgentId { get; set; }

        /// <summary>
        /// 代理商玩家标示
        /// </summary>
        [JsonProperty("userCode")]
        public string UserCode { get; set; }

        /// <summary>
        /// 游戏 ID
        /// </summary>
        [JsonProperty("gameId")]
        public int GameId { get; set; }

        /// <summary>
        /// 房间 ID
        /// </summary>
        [JsonProperty("roomId")]
        public int RoomId { get; set; }

        /// <summary>
        /// 牌局号
        /// </summary>
        [JsonProperty("dealId")]
        public string DealId { get; set; }

        /// <summary>
        /// 桌子号
        /// </summary>
        [JsonProperty("deskId")]
        public int DeskId { get; set; }

        /// <summary>
        /// 座位号
        /// </summary>
        [JsonProperty("seatId")]
        public int SeatId { get; set; }

        /// <summary>
        /// 初始分数
        /// </summary>
        [JsonProperty("initMoney")]
        public string InitMoney { get; set; }

        /// <summary>
        /// 结算后分数
        /// </summary>
        [JsonProperty("money")]
        public string Money { get; set; }

        /// <summary>
        /// 总下注
        /// </summary>
        [JsonProperty("totalBet")]
        public string TotalBet { get; set; }

        /// <summary>
        /// 有效下注—保留参数
        /// </summary>
        [JsonProperty("effectBet")]
        public string EffectBet { get; set; }

        /// <summary>
        /// 输赢分数
        /// </summary>
        [JsonProperty("winLost")]
        public string WinLost { get; set; }

        /// <summary>
        /// 输赢分数的绝对值
        /// </summary>
        [JsonProperty("winLostAbs")]
        public string WinLostAbs { get; set; }

        /// <summary>
        /// 抽水额
        /// </summary>
        [JsonProperty("fee")]
        public string Fee { get; set; }

        /// <summary>
        /// 派彩额
        /// </summary>
        [JsonProperty("payAmount")]
        public string PayAmount { get; set; }

        /// <summary>
        /// 有效下注=总输+总赢，棋牌可以根据这个值进行返水
        /// </summary>
        [JsonProperty("allBills")]
        public string AllBills { get; set; }

        /// <summary>
        /// 总输=所有亏损的下注位亏损额度总和的绝对值
        /// </summary>
        [JsonProperty("allLost")]
        public string AllLost { get; set; }

        /// <summary>
        /// 总赢=所有赢的下注位盈利额度总和
        /// </summary>
        [JsonProperty("allWin")]
        public string AllWin { get; set; }

        /// <summary>
        /// 开局时间
        /// </summary>
        [JsonProperty("openTime")]
        public string OpenTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [JsonProperty("endTime")]
        public string EndTime { get; set; }
    }
}