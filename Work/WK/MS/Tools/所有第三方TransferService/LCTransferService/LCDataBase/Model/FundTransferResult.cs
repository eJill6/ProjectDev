using LCDataBase.Enums;
using Newtonsoft.Json;

namespace LCDataBase.Model
{
    public class FundTransferResult
    {
        /// <summary>
        /// 错误码
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; } = (int)TransferStatus.SysDefault;

        /// <summary>
        /// 上分后可下分金额
        /// </summary>
        [JsonProperty(PropertyName = "money")]
        public decimal Money { get; set; }

        /// <summary>
        /// 状态码（-1：不存在、0：成功、2: 失败、3:正在处理中）
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public decimal Status { get; set; } = (decimal)TransferStatus.SysDefault;
    }
}
