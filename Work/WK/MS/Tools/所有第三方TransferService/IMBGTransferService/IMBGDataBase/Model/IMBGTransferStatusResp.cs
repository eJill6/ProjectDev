using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMBGDataBase.Model
{
    public class IMBGTransferStatusResp : RespData
    {
        /// <summary>
        /// 订单状态（1：处理中，2：成功，3：失败）
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// 订单分数
        /// </summary>
        [JsonProperty("money")]
        public decimal Money { get; set; }

        /// <summary>
        /// 订单分数，字符串类型
        /// </summary>
        [JsonProperty("moneyStr")]
        public string MoneyStr { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        /// <summary>
        /// 订单类型 1 上分 2 下分
        /// </summary>
        [JsonProperty("tradeType")]
        public string TradeType { get; set; }
    }
}
