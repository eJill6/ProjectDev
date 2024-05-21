export enum MyBookingStatusType {
        /// <summary>
        /// 服务中 待接单,服务中,拒绝退款
        /// </summary>
        InService = 1,
        /// <summary>
        ///  已完成 待评价,评价审核中,交易完成，评价审核未通过,订单已取消, 超时未接单，超时未接单处理中
        /// </summary>
        Completed = 2,
        /// <summary>
        ///  退款中 申请退款中
        /// </summary>
        Refunding = 3,
        /// <summary>
        /// 已退款 退款成功
        /// </summary>
        Refunded = 4,
}