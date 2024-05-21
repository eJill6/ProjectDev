// export enum ManageBookingStatusType{
//   All = 0,
//   /// 待接單
//   Pending = 1,

//   /// 服務中
//   InService = 2,

//   /// 待評價
//   Finish = 3,

//   /// 取消接單
//   OrderCancelled = 4,

//   /// 超時未接單
//   TimeoutNoAcceptance = 5,

//   /// 申請退款中
//   RefundInProgress = 6,

//   /// 已退款
//   RefundSuccessful = 7,
// }

export enum ManageBookingStatusType{

        /// <summary>
        /// 服務中
        /// </summary>
        InService = 1,
        /// <summary>
        /// 交易完成
        /// </summary>
        TransactionCompleted = 2,
        /// <summary>
        /// 申請退款中
        /// </summary>
        RefundInProgress = 3,

        /// <summary>
        /// 退款成功
        /// </summary>
        RefundSuccessful = 4,

}
