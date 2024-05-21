using JxBackendService.Common.Util;
using Newtonsoft.Json;
using System;

namespace JxBackendService.Model.Param.ThirdParty
{
    public class LastPlayInfo
    {
        [JsonProperty("PC")]
        public string ProductCode { get; set; }

        [JsonProperty("BTS")]
        public long BetTimeStamp { get; set; }

        [JsonProperty("ID")]
        public string PlayInfoID { get; set; }

        [JsonProperty("STS")]
        public long SearchTimeStamp { get; set; }

        [JsonIgnore]
        public DateTime BetTime => BetTimeStamp.ToDateTime();

        [JsonIgnore]
        public DateTime SearchTime => SearchTimeStamp.ToDateTime();
    }
}