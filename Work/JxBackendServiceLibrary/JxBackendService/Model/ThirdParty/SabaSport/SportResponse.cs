using Newtonsoft.Json;
using System;

namespace JxBackendService.Model.ThirdParty.SabaSport
{
    public class BaseSportData
    {
        [JsonProperty(PropertyName = "error_code")]
        public int? Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

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

    public class SportResponse<T> : BaseSportData
    {
        /// <summary>
        /// 数据结果
        /// </summary>
        [JsonProperty(PropertyName = "Data")]
        public T Data { get; set; }
    }

    public class SportBalanceData : BaseSportData
    {
        [JsonProperty(PropertyName = "vendor_member_id")]
        public string TPGameAccount { get; set; }

        [JsonProperty(PropertyName = "balance")]
        public decimal? Balance { get; set; }

        [JsonProperty(PropertyName = "bonus_balance")]
        public decimal? BonusBalance { get; set; }

        [JsonProperty(PropertyName = "outstanding")]
        public decimal? Outstanding { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public int? Currency { get; set; }
    }

    public class SportTransferData : BaseSportData
    {
        [JsonProperty(PropertyName = "trans_id")]
        public int? TransId { get; set; }

        [JsonProperty(PropertyName = "before_amount")]
        public decimal? BeforeAmount { get; set; }

        [JsonProperty(PropertyName = "after_amount")]
        public decimal? AfterAmount { get; set; }

        [JsonProperty(PropertyName = "bonus_before_amount")]
        public decimal? BonusBeforeAmount { get; set; }

        [JsonProperty(PropertyName = "bonus_after_amount")]
        public decimal? BonusAfterAmount { get; set; }

        [JsonProperty(PropertyName = "system_id")]
        public string SystemId { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int? Status { get; set; }

        [JsonProperty(PropertyName = "transfer_date")]
        public DateTime? TransferDate { get; set; }

        public decimal? Amount { get; set; }
    }

    public class SportBetSettingItem
    {
        public string Sport_type { get; set; }

        public int Min_bet { get; set; }

        public int Max_bet { get; set; }

        public int Max_bet_per_match { get; set; }
    }
}
