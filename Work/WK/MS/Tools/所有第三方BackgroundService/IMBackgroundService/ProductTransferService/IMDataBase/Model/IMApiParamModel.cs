using IMDataBase.Enums;
using JxBackendService.Model.Common.IMOne;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.ThirdParty.Old;

namespace IMDataBase.Model
{
    public class IMApiParamModel : IMOneReportApiSetting, IOldBetLogApiParam, IIMOneReportApiSetting
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
        /// 使用幣別
        /// </summary>
        public string Currency { get; set; }

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
        /// 廠商產品錢包代號
        /// </summary>
        public string ProductWallet { get; set; }

        /// <summary>
        /// 每次查詢的時間區間
        /// </summary>
        public int PerOnceQueryMinutes { get; set; }

        /// <summary>
        /// 產品類別代號(預設為电竞/体育电竞)
        /// </summary>
        public int ProductCode { get; set; } = IMProductCode.ESportsBulls.Value;

        public string LastSearchToken { get; set; }
    }

    /// <summary>
    /// 產品類別代號列舉
    /// </summary>
    public class IMProductCode : BaseIntValueModel<IMProductCode>
    {
        private IMProductCode()
        { }

        /// <summary>
        /// 电竞 / 体育电竞
        /// </summary>
        public static IMProductCode ESportsBulls = new IMProductCode()
        {
            Value = 1
        };

        /// <summary>
        /// 虚拟电竞
        /// </summary>
        public static IMProductCode EsportsVirtual = new IMProductCode()
        {
            Value = 2
        };

        /// <summary>
        /// PK 拾 快乐荣耀
        /// </summary>
        public static IMProductCode PKTenLegends = new IMProductCode()
        {
            Value = 3
        };
    }
}