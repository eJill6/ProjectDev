using MS.Core.MMModel.Attributes;
using System.ComponentModel;

namespace MS.Core.MMModel.Models.Booking.Enums
{
    public enum MyBookingStatusForClient
    {
        /// <summary>
        /// 服務中
        /// </summary>
        [ManageDescription("服务中")]
        [Description("服务中")]
        InService = 1,
        /// <summary>
        /// 交易完成
        /// </summary>
        [ManageDescription("服务完成")]
        [Description("交易完成")]
        TransactionCompleted = 2,
        /// <summary>
        /// 申請退款中
        /// </summary>
        [ManageDescription("申请退款中")]
        [Description("申请退款中")]
        RefundInProgress = 3,

        /// <summary>
        /// 退款成功
        /// </summary>
        [ManageDescription("退款成功")]
        [Description("退款成功")]
        RefundSuccessful = 4,
    }
}
