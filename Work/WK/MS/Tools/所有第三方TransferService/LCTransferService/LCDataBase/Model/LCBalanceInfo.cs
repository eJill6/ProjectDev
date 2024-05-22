using LCDataBase.Enums;
using Newtonsoft.Json;

namespace LCDataBase.Model
{
    public class LCBalanceInfo
    {
        /// <summary>
        /// 总余额
        /// </summary>
        [JsonProperty(PropertyName = "totalMoney")]
        public decimal TotalMoney { get; set; }

        /// <summary>
        /// 可下分余额
        /// </summary>
        [JsonProperty(PropertyName = "freeMoney")]
        public decimal FreeMoney { get; set; }

        /// <summary>
        /// 状态码（-1、不存在，0、不在 线,1、在线）
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; } = (int)TransferStatus.SysDefault;

        /// <summary>
        /// 错误码
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; } = (int)APIErrorCode.SysDefault;
    }
}