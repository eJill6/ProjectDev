namespace JxBackendService.Model.Enums.Finance
{
    public enum MoneyOutDealTypes
    {
        /// <summary>
        /// 未处理
        /// </summary>
        Unprocessed = 0,
        /// <summary>
        /// 已处理
        /// </summary>
        Done = 1,
        /// <summary>
        /// 正在处理
        /// </summary>
        Processing = 2,
        /// <summary>
        /// 处理失败
        /// </summary>
        Fail = 3,
        /// <summary>
        /// 待确认
        /// </summary>
        WaitingForConfirm = 4,
        /// <summary>
        /// 已退款
        /// </summary>
        Refunded = 5,
        /// <summary>
        /// 异常单
        /// </summary>
        ExceptionDeal = 6,
        /// <summary>
        /// 重置為未处理
        /// </summary>
        ResetToUnprocessed = 7
    }
}
