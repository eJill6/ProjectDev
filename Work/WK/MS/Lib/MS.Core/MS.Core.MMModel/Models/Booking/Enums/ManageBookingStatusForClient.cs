using MS.Core.MMModel.Attributes;
using System.ComponentModel;

namespace MS.Core.MMModel.Models.Booking.Enums
{
    public enum ManageBookingStatusForClient
    {
        //[ManageDescription("全部订单")]
        //All = 0,
        ///// <summary>
        ///// 待接單
        ///// </summary>
        //[ManageDescription("待接单")]
        //Pending = 1,

        ///// <summary>
        ///// 服務中
        ///// </summary>
        //[ManageDescription("服务中")]
        //InService = 2,

        ///// <summary>
        ///// 待評價
        ///// </summary>
        //[ManageDescription("服务完成")]
        //Finish = 3,

        ///// <summary>
        ///// 取消接單
        ///// </summary>
        //[ManageDescription("取消接单")]
        //OrderCancelled = 4,

        ///// <summary>
        ///// 超時未接單
        ///// </summary>
        //[Description("超时未接单")]
        //TimeoutNoAcceptance = 5,

        ///// <summary>
        ///// 申請退款中
        ///// </summary>
        //[ManageDescription("申请退款中")]
        //RefundInProgress = 6,

        ///// <summary>
        ///// 已退款
        ///// </summary>
        //[ManageDescription("已退款")]
        //RefundSuccessful = 7,
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

        /// <summary>
        /// 拒絕退款
        /// </summary>
        [ManageDescription("服务完成")]
        [Description("拒绝退款")]
        RefundRejected = 5,
    }
}
