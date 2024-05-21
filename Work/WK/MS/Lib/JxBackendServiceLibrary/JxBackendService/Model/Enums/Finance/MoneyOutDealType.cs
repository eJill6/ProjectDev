using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums.Finance
{
    /// <summary>
    /// 『提現處理』狀態
    /// </summary>
    public class MoneyOutDealType : BaseValueModel<int, MoneyOutDealType>
    {
        private MoneyOutDealType()
        { }

        /// <summary>
        /// 已处理
        /// </summary>
        public static readonly MoneyOutDealType Done = new MoneyOutDealType()
        {
            Value = 1,
            ResourceType = typeof(FinanceElement),
            ResourcePropertyName = nameof(FinanceElement.OrderStatusDone)
        };

        /// <summary>
        /// 正在处理
        /// </summary>
        public static readonly MoneyOutDealType Processing = new MoneyOutDealType()
        {
            Value = 2,
            ResourceType = typeof(FinanceElement),
            ResourcePropertyName = nameof(FinanceElement.OrderStatusProcessing)
        };

        /// <summary>
        /// 已退款
        /// </summary>
        public static readonly MoneyOutDealType Refunded = new MoneyOutDealType()
        {
            Value = 5,
            ResourceType = typeof(FinanceElement),
            ResourcePropertyName = nameof(FinanceElement.OrderStatusRefunded)
        };
    }
}