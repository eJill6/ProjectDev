using MS.Core.Attributes;
using MS.Core.MM.Models.Booking.Enums;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models;

namespace MS.Core.MM.Models.Entities.Post
{
    public class MMBooking : BaseDBModel, IBookingPaymentStatus
    {
        /// <summary>
        /// 預約 Id
        /// </summary>
        [PrimaryKey]
        public string BookingId { get; set; }

        /// <summary>
        /// 贴子 Id
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 預約用戶 Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 預約狀態。0：確認中、1：服務中、2：服務完成、3：取消接單、4：超時未接單
        /// </summary>
        public BookingStatus Status { get; set; }

        /// <summary>
        /// 套餐名稱
        /// </summary>
        public string ComboName { get; set; }

        /// <summary>
        /// 套餐價格
        /// </summary>
        public decimal ComboPrice { get; set; }

        /// <summary>
        /// 服務時間、次數或包含項目
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// 支付方式。1：預約支付、2：全額支付
        /// </summary>
        public BookingPaymentType PaymentType { get; set; }

        /// <summary>
        /// 支付金額(鑽石)
        /// </summary>
        public decimal PaymentMoney { get; set; }

        /// <summary>
        /// 預約金(鑽石)
        /// </summary>
        public decimal BookingAmount { get; set; }

        /// <summary>
        /// 聯繫方式
        /// </summary>
        public string? Contact { get; set; }

        /// <summary>
        /// 預約時間
        /// </summary>
        public DateTime BookingTime { get; set; }

        /// <summary>
        /// 接單時間
        /// </summary>
        public DateTime? AcceptTime { get; set; }

        /// <summary>
        /// 取消時間
        /// </summary>
        public DateTime? CancelTime { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime? FinishTime { get; set; }

        /// <summary>
        /// 計畫時間時間(預計完成時間48H、用戶評價時間48H)
        /// </summary>
        public DateTime? ScheduledTime { get; set; }

        /// <summary>
        /// 前端用戶是否刪除。0：未刪除、1：刪除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 發贴人ID(商戶UserId)
        /// </summary>
        public int PostUserId { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// 收益單ID
        /// </summary>
        public string? IncomeId { get; set; }

        /// <summary>
        /// 预定时发帖人身份
        /// </summary>
        public IdentityType? CurrentIdentity { get; set; }

        /// <summary>
        /// 超觅老板拆占比
        /// </summary>
        public decimal? PlatformSharing { get; set; }
    }
}