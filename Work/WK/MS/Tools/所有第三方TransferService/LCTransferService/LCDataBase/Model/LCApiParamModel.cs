using LCDataBase.Enums;
using JxBackendService.Model.ViewModel.ThirdParty.Old;

namespace LCDataBase.Model
{
    public class LCApiParamModel : IOldBetLogApiParam
    {
        /// <summary>
        /// API URL
        /// </summary>
        public string ServiceUrl { get; set; }

        /// <summary>
        /// 代理 ID
        /// </summary>
        public string AgentID { get; set; }

        /// <summary>
        /// 使用幣別
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 操作行為
        /// </summary>
        public ApiAction ActType { get; set; }

        /// <summary>
        /// 交易 ID
        /// </summary>
        public string TransferID { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string OrderID { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 使用者id
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 平台DES key
        /// </summary>
        public string DESKey { get; set; }

        /// <summary>
        /// 平台 MD5 key
        /// </summary>
        public string MD5Key { get; set; }

        /// <summary>
        /// 每次查詢的時間區間
        /// </summary>
        public int PerOnceQueryMinutes { get; set; }

        /// <summary>
        /// 平台 Linecode
        /// </summary>
        public string Linecode { get; set; }

        public string LastSearchToken { get; set; }
    }
}