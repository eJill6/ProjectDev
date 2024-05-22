using Newtonsoft.Json;

namespace LCDataBase.Model
{
    public class BetDetails
    {
        /// <summary>
        /// 使用者id       
        /// </summary>
        public int[] UserID { get; set; }

        /// <summary>
        /// 游戏局号列表
        /// </summary>
        [JsonProperty(PropertyName = "GameID")]
        public string[] GameID { get; set; }

        /// <summary>
        /// 玩家帐号列表        
        /// </summary>
        [JsonProperty(PropertyName = "Accounts")]
        public string[] Accounts { get; set; }

        /// <summary>
        /// 房间 ID 列表
        /// </summary>
        [JsonProperty(PropertyName = "ServerID")]
        public int[] ServerID { get; set; }

        /// <summary>
        /// 游戏 ID 列表
        /// </summary>
        [JsonProperty(PropertyName = "KindID")]
        public int[] KindID { get; set; }

        /// <summary>
        /// 桌子号列表
        /// </summary>
        [JsonProperty(PropertyName = "TableID")]
        public long[] TableID { get; set; }

        /// <summary>
        /// 椅子号列表
        /// </summary>
        [JsonProperty(PropertyName = "ChairID")]
        public int[] ChairID { get; set; }

        /// <summary>
        /// 玩家数量列表
        /// </summary>
        [JsonProperty(PropertyName = "UserCount")]
        public int[] UserCount { get; set; }

        /// <summary>
        /// 有效下注
        /// </summary>
        [JsonProperty(PropertyName = "CellScore")]
        public string[] CellScore { get; set; }

        /// <summary>
        /// 总下注列表
        /// </summary>
        [JsonProperty(PropertyName = "AllBet")]
        public string[] AllBet { get; set; }

        /// <summary>
        /// 盈利列表
        /// </summary>
        [JsonProperty(PropertyName = "Profit")]
        public string[] Profit { get; set; }

        /// <summary>
        /// 抽水列表
        /// </summary>
        [JsonProperty(PropertyName = "Revenue")]
        public string[] Revenue { get; set; }

        /// <summary>
        /// 游戏开始时间列表
        /// </summary>
        [JsonProperty(PropertyName = "GameStartTime")]
        public string[] GameStartTime { get; set; }

        /// <summary>
        /// 游戏结束时间列表
        /// </summary>
        [JsonProperty(PropertyName = "GameEndTime")]
        public string[] GameEndTime { get; set; }

        /// <summary>
        /// 手牌公共牌
        /// </summary>
        [JsonProperty(PropertyName = "CardValue")]
        public string[] CardValue { get; set; }

        /// <summary>
        /// 渠道 ID 列表
        /// </summary>
        [JsonProperty(PropertyName = "ChannelID")]
        public int[] ChannelID { get; set; }

        /// <summary>
        /// 游戏结果对应玩家所属站点
        /// </summary>
        [JsonProperty(PropertyName = "LineCode")]
        public string[] LineCode { get; set; }
    }
}
