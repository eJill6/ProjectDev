using Newtonsoft.Json;

namespace IMBGDataBase.Model
{
    public class IMBGBetLogResp<T>
    {
        /// <summary>
        /// 子操作类型，登录游戏传 1
        /// </summary>
        [JsonProperty("ac")]
        public string Ac { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        [JsonProperty("me")]
        public string Me { get; set; }

        /// <summary>
        /// 数据结构返回值
        /// </summary>
        [JsonProperty("list")]
        public T List { get; set; }

        /// <summary>
        /// JX使用, 非IM棋牌回傳資料
        /// </summary>
        public string Message { get; set; }
    }
}