using JxBackendService.Model.ViewModel.ThirdParty.Old;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMBGDataBase.Model
{
    /// <summary>
    /// IM棋牌API回應基底類別
    /// </summary>
    public class IMBGResp<T> : IOldBetLogModel
    {
        /// <summary>
        /// 子操作类型，登录游戏传 1
        /// </summary>
        [JsonProperty("ac")]
        public int Ac { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        [JsonProperty("me")]
        public string Me { get; set; }

        /// <summary>
        /// 数据结构返回值
        /// </summary>
        [JsonProperty("data")]
        public T Data { get; set; }

        /// <summary>
        /// JX使用, 非IM棋牌回傳資料
        /// </summary>
        public string Message { get; set; }

        public string RemoteFileSeq { get; set; }

        public Action WriteRemoteContentToOtherMerchant { get; set; }
    }

    public class RespData
    {
        /// <summary>
        /// 错误码
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; } = 0;
    }
}