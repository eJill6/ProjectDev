using Newtonsoft.Json;

namespace IMBGDataBase.Model
{
    public class IMBGBalanceResp : RespData
    {
        /// <summary>
        /// 总分数
        /// </summary>
        [JsonProperty("money")]
        public decimal Money { get; set; }

        /// <summary>
        /// 可下分数
        /// </summary>
        [JsonProperty("freeMoney")]
        public decimal FreeMoney { get; set; }

        /// <summary>
        /// 总分数，字符串类型
        /// </summary>
        [JsonProperty("moneyStr")]
        public string MoneyStr { get; set; }

        /// <summary>
        /// 可下分数，字符串类型
        /// </summary>
        [JsonProperty("freeMoneyStr")]
        public string FreeMoneyStr { get; set; }

        /// <summary>
        /// 状态码，用户是否在游戏房间内。（0：不在线，1：在线）
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
