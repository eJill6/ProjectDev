namespace JxBackendService.Model.Enums.Finance
{
    public enum MoneyInDealTypes
    {
        /// <summary>
        /// 未处理
        /// </summary>
        Unprocessed = 0,
        /// <summary>
        /// 自动处理
        /// </summary>
        Done = 1,
        /// <summary>
        /// 手动处理
        /// </summary>
        Manual_Done = 2,
        /// <summary>
        /// 处理失败
        /// </summary>
        Fail = 3,
    }
}
