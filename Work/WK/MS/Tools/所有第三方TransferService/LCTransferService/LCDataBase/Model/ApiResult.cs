using System;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using Newtonsoft.Json;

namespace LCDataBase.Model
{
    public class ApiResult<T> : IOldBetLogModel
    {
        /// <summary>
        /// 主操作类型
        /// </summary>
        [JsonProperty(PropertyName = "m")]
        public string MainType { get; set; }

        /// <summary>
        /// 子操作类型
        /// </summary>
        [JsonProperty(PropertyName = "s")]
        public string SubType { get; set; }

        /// <summary>
        /// 数据结果
        /// </summary>
        [JsonProperty(PropertyName = "d")]
        public T Data { get; set; }

        public string RemoteFileSeq { get; set; }

        public Action WriteRemoteContentToOtherMerchant { get; set; }
    }
}