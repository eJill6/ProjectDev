using MS.Core.MMModel.Attributes;
using System.ComponentModel;

namespace MS.Core.MM.Models.Booking.Enums
{
    public enum BookingStatus
    {
        /// <summary>
        /// 服务中
        /// </summary>
        [ManageDescription("服务中")]
        [Description("服务中")]
        InService = 1,
        /// <summary>
        /// 交易完成
        /// </summary>
        [ManageDescription("交易完成")]
        [Description("交易完成")]
        TransactionCompleted = 2,
        /// <summary>
        /// 申请退款中
        /// </summary>
        [ManageDescription("申请退款中")]
        [Description("申请退款中")]
        RefundInProgress=3,
        /// <summary>
        /// 退款成功
        /// </summary>
        [ManageDescription("退款成功")]
        [Description("退款成功")]
        RefundSuccessful=4,

        //[ManageDescription("待接单")]
        //[Description("待接单")]
        //Pending = 0,

        //[ManageDescription("服务中")]
        //[Description("服务中")]
        //InService = 1,

        //[ManageDescription("服务完成")]
        //[Description("待评价")]
        //PendingEvaluation = 2,

        //[ManageDescription("服务完成")]
        //[Description("评价审核中")]
        //EvaluationReview = 3,

        //[ManageDescription("服务完成")]
        //[Description("交易完成")]
        //TransactionCompleted = 4,

        //[ManageDescription("服务完成")]
        //[Description("评价审核未通过")]
        //EvaluationRejected = 5,

        //[ManageDescription("订单已取消")]
        //[Description("订单已取消")]
        //OrderCancelled = 6,

        //[ManageDescription("超时未接单")]
        //[Description("超时未接单")]
        //TimeoutNoAcceptance = 7,

        //[ManageDescription("申请退款中")]
        //[Description("申请退款中")]
        //RefundInProgress = 8,

        //[ManageDescription("退款成功")]
        //[Description("退款成功")]
        //RefundSuccessful = 9,

        //[ManageDescription("服务完成")]
        //[Description("拒绝退款")]
        //RefundRejected = 10,

        //[ManageDescription("超时未接单")]
        //[Description("超时未接单")]
        //TimeoutNoAcceptanceProgress = 11,
    }
}
