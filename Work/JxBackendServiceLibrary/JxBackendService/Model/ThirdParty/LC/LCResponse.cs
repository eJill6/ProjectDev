using JxBackendService.Model.Enums;
using JxBackendService.Resource.Element;
using Newtonsoft.Json;

namespace JxBackendService.Model.ThirdParty.LC
{
    public class LCResponse<T>
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

        /// <summary>
        /// 回传讯息
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }

    public class BaseLCData
    {
        /// <summary>
        /// 错误码
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public int? Code { get; set; }

        public bool IsSuccess => Code == 0;

        public string ErrorLog
        {
            get
            {
                if (IsSuccess)
                {
                    return null;
                }

                return $"Error Code = {Code}";
            }
        }
    }

    public class LCLoginData : BaseLCData
    {
        /// <summary>
        /// 游戏 URL 
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        /// <summary>
        /// 1=中国风 2=Q 版主题 3（设计中）
        /// </summary>
        [JsonProperty(PropertyName = "skin")]
        public int? Skin { get; set; }
    }

    public class LCBalanceData : BaseLCData
    {
        /// <summary>
        /// 总余额
        /// </summary>
        [JsonProperty(PropertyName = "totalMoney")]
        public decimal? TotalMoney { get; set; }

        /// <summary>
        /// 可下分余额
        /// </summary>
        [JsonProperty(PropertyName = "freeMoney")]
        public decimal? FreeMoney { get; set; }

        /// <summary>
        /// 状态码（-1、不存在，0、不在 线,1、在线）
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public int? Status { get; set; }
    }

    public class LCTransferData : BaseLCData
    {
        /// <summary>
        /// 上分后可下分金额
        /// </summary>
        [JsonProperty(PropertyName = "money")]
        public decimal? Money { get; set; }

        /// <summary>
        /// 状态码（-1：不存在、0：成功、2: 失败、3:正在处理中）
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public int? Status { get; set; }
    }

    public class LCOrderStatus : BaseIntValueModel<LCOrderStatus>
    {
        private LCOrderStatus()
        {
            ResourceType = typeof(SelectItemElement);
        }

        public static readonly LCOrderStatus NotExists = new LCOrderStatus()
        {
            Value = -1,
            ResourcePropertyName = nameof(SelectItemElement.LCOrderStatus_NotExists)
        };

        public static readonly LCOrderStatus Success = new LCOrderStatus()
        {
            Value = 0,
            ResourcePropertyName = nameof(SelectItemElement.LCOrderStatus_Success)
        };

        public static readonly LCOrderStatus Fail = new LCOrderStatus()
        {
            Value = 2,
            ResourcePropertyName = nameof(SelectItemElement.LCOrderStatus_NotExists)
        };

        public static readonly LCOrderStatus Processing = new LCOrderStatus()
        {
            Value = 3,
            ResourcePropertyName = nameof(SelectItemElement.LCOrderStatus_Processing)
        };
    }
}
