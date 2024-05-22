using IMBGDataBase.Enums;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using System;

namespace IMBGDataBase.Model
{
    public class IMBGApiParamModel : IOldBetLogApiParam
    {
        /// <summary>
        /// API URL
        /// </summary>
        public string ServiceUrl { get; set; }

        /// <summary>
        /// 代理 ID
        /// </summary>
        public string MerchantCode { get; set; }

        /// <summary>
        /// 語系
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 操作行為
        /// </summary>
        public ApiAction ActType { get; set; }

        /// <summary>
        /// MoneyInfo.MoneyInID
        /// </summary>
        public string MoneyID { get; set; }

        /// <summary>
        /// Money(In/Out)Info.Status
        /// </summary>
        public int MoneyInfoStatus { get; set; }

        /// <summary>
        /// UserInfo.UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// UserInfo.UserID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 訂單編號(唯一)
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        public string PlayerId { get; set; }

        /// <summary>
        /// 金額(最多为2 位小数)
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 頁
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每次查詢的時間區間
        /// </summary>
        public int PerOnceQueryMinutes { get; set; }

        /// <summary>
        /// MD5 key (request data 加密用)
        /// </summary>
        public string MD5Key { get; set; }

        /// <summary>
        /// Des ECB key (request data 加密用)
        /// </summary>
        public string DesKey { get; set; }

        public string LastSearchToken { get; set; }
    }
}