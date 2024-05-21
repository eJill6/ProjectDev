using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums
{
    public class RefundType : BaseIntValueModel<RefundType>
    {
        private RefundType()
        {
            ResourceType = typeof(SelectItemElement);
        }

        /// <summary>系統贈送紅包</summary>
        public static readonly RefundType HBBySystem = new RefundType()
        {
            Value = 1,
            ResourcePropertyName = nameof(SelectItemElement.RefundType_HBBySystem)
        };

        /// <summary>扣減可用分</summary>
        public static readonly RefundType ReduceAvailableScore = new RefundType()
        {
            Value = 2,
            ResourcePropertyName = nameof(SelectItemElement.RefundType_ReduceAvailableScore)
        };

        /// <summary>紅包</summary>
        public static readonly RefundType HB = new RefundType()
        {
            Value = 4,
            ResourcePropertyName = nameof(SelectItemElement.RefundType_HB)
        };

        /// <summary>系統充值</summary>
        public static readonly RefundType RechargeBySystem = new RefundType()
        {
            Value = 5,
            ResourcePropertyName = nameof(SelectItemElement.RefundType_RechargeBySystem)
        };

        /// <summary>系統補發獎金</summary> 
        public static readonly RefundType PrizeBySystem = new RefundType()
        {
            Value = 6,
            ResourcePropertyName = nameof(SelectItemElement.RefundType_PrizeBySystem)
        };

        /// <summary>系統發送佣金</summary>
        public static readonly RefundType YJBySystem = new RefundType()
        {
            Value = 7,
            ResourcePropertyName = nameof(SelectItemElement.RefundType_YJBySystem)
        };

        /// <summary>活動</summary>
        public static readonly RefundType Activity = new RefundType()
        {
            Value = 100,
            ResourcePropertyName = nameof(SelectItemElement.RefundType_Activity)
        };

        /// <summary>調帳</summary>
        public static readonly RefundType Adjustments = new RefundType()
        {
            Value = 101,
            ResourcePropertyName = nameof(SelectItemElement.RefundType_Adjustments)
        };
    }
}
