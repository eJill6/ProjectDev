using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMBGDataBase.Model
{
    public class IMBGTransferResp : RespData
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

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
        /// 订单类型 1:上分, 2:下分
        /// </summary>
        [JsonProperty("tradeType")]
        public string TradeType { get; set; }

        /// <summary>
        /// 订单金额 
        /// </summary>
        [JsonProperty("orderMoney")]
        public decimal OrderMoney { get; set; }

        /// <summary>
        /// 订单金额，字符串类型 
        /// </summary>
        [JsonProperty("orderMoneyStr")]
        public string OrderMoneyStr { get; set; }
    }
}
