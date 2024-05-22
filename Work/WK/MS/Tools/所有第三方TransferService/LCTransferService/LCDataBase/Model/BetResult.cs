using LCDataBase.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LCDataBase.Model
{
    public class BetResult
    {
        //public string last_version_key = string.Empty;

        /// <summary>
        /// 错误码
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; } = (int)APIErrorCode.SysDefault;

        /// <summary>
        /// 返回列表行数
        /// </summary>
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        [JsonProperty(PropertyName = "list")]
        public BetDetails BetDetails { get; set; }

        /// <summary>
        /// 数据拉取开始时间
        /// </summary>
        [JsonProperty(PropertyName = "start")]
        public string ServerStartTime { get; set; }

        /// <summary>
        /// 数据拉取结束时间
        /// </summary>
        [JsonProperty(PropertyName = "end")]
        public string ServerEndTime { get; set; }
    }
}
