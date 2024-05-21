using JxBackendService.Interface.Service.Web.BackSideWeb;
using JxBackendService.Model.Attributes;
using JxBackendService.Resource.Element;
using MS.Core.MMModel.Models.User.Enums;

namespace BackSideWeb.Models.Param
{
    public class QueryAdminBookingViewModel:IDataKey
    {
        public string KeyContent => BookingId.ToString();
        /// <summary>
        /// 预约单ID
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminBookingId), ResourceType = typeof(DisplayElement), Sort = 1)]
        public string BookingId { get; set; }


        /// <summary>
        /// 预约会员ID
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminBookingUserId), ResourceType = typeof(DisplayElement), Sort = 2)]
        public int UserId { get; set; }

        /// <summary>
        /// 用户身份
        /// </summary>
        public IdentityType CurrentIdentity { get; set; }
        /// <summary>
        /// 预约会员ID
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminBookingUserId), ResourceType = typeof(DisplayElement), Sort = 3)]
        public string UserIdentityText { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminBookingContact), ResourceType = typeof(DisplayElement), Sort = 4)]
        public string Contact { get; set; }

        /// <summary>
        /// 帖子ID
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminBookingPostId), ResourceType = typeof(DisplayElement), Sort = 5)]
        public string PostId { get; set; }





        /// <summary>
        /// 下单时间
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminBookingBookingTimeText), ResourceType = typeof(DisplayElement), Sort = 6)]
        public string BookingTimeText { get; set; }

        /// <summary>
        /// 接单时间
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminBookingAcceptTimeText), ResourceType = typeof(DisplayElement), Sort = 7)]
        public string AcceptTimeText { get; set; }

        /// <summary>
        /// 确认完成时间
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminBookingFinishTimeText), ResourceType = typeof(DisplayElement), Sort = 8)]
        public string FinishTimeText { get; set; }

        /// <summary>
        /// 取消时间
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminBookingCancelTimeText), ResourceType = typeof(DisplayElement), Sort = 9)]
        public string CancelTimeText { get; set; }

        ///<summary>
        /// 支付类型
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminBookingPaymentTypeText), ResourceType = typeof(DisplayElement), Sort = 10)]
        public string PaymentTypeText { get; set; }

        /// <summary>
        /// 支付金額
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminBookingPaymentMoneyText), ResourceType = typeof(DisplayElement), Sort = 11)]
        public string PaymentMoneyText { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminBookingDiscountText), ResourceType = typeof(DisplayElement), Sort = 12)]
        public string DiscountText { get; set; }

        /// <summary>
        /// 实际支付金额
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminBookingActualPaymentMoneyText), ResourceType = typeof(DisplayElement), Sort = 13)]
        public string ActualPaymentMoneyText { get; set; }


        /// <summary>
        /// 订单状态
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminBookingOrderStatusText), ResourceType = typeof(DisplayElement), Sort = 14)]
        public string OrderStatusText { get; set; }

        /// <summary>
        /// 预约状态
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminBookingBookingStatusText), ResourceType = typeof(DisplayElement), Sort = 15)]
        public string BookingStatusText { get; set; }
    }
}
