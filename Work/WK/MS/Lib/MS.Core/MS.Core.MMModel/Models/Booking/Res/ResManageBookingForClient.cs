using MS.Core.MMModel.Models.Booking.Enums;

namespace MS.Core.MM.Models.Booking.Res
{
    public class ResManageBookingForClient
    {
        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AvatarUrl { get; set; }
        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 狀態
        /// </summary>
        public BookingStatusForClient Status { get; set; }
        /// <summary>
        /// 聯絡資訊
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 預約時間
        /// </summary>
        public string BookingTime { get; set; }
        /// <summary>
        /// 接單時間
        /// </summary>
        public string AcceptTime { get; set; }
        /// <summary>
        /// 取消時間
        /// </summary>
        public string CancelTime { get; set; }
        /// <summary>
        /// 完成時間
        /// </summary>
        public string FinishTime { get; set; }
        /// <summary>
        /// 支付類型
        /// </summary>
        public BookingPaymentTypeForClient PaymentType { get; set; }
        /// <summary>
        /// 預約ID
        /// </summary>
        public string BookingId { get; set; }
        /// <summary>
        /// 狀態
        /// </summary>
        public string StatusText { get; set; }
        /// <summary>
        /// 支付金額
        /// </summary>
        public string PaymentMoney { get; set; }
    }
}