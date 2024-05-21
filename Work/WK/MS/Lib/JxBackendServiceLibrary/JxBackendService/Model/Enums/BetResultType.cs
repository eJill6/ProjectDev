using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums
{
    public class BetResultType : BaseIntValueModel<BetResultType>
    {
        private BetResultType()
        { }

        public int MiseLiveOrderStatus { get; private set; } = 9999;

        /// <summary>贏</summary>
        public static BetResultType Win = new BetResultType()
        {
            Value = 1,
            MiseLiveOrderStatus = 1,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.BetResultType_Win)
        };

        /// <summary>輸</summary>
        public static BetResultType Lose = new BetResultType()
        {
            Value = 0,
            MiseLiveOrderStatus = 2,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.BetResultType_Lose)
        };

        /// <summary>和局</summary>
        public static BetResultType Draw = new BetResultType()
        {
            Value = -1,
            MiseLiveOrderStatus = 3,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.BetResultType_Draw)
        };

        /// <summary>半贏</summary>
        public static BetResultType HalfWin = new BetResultType()
        {
            Value = 2,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.BetResultType_HalfWin)
        };

        /// <summary>半輸</summary>
        public static BetResultType HalfLose = new BetResultType()
        {
            Value = 3,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.BetResultType_HalfLose)
        };

        /// <summary>兌現</summary>
        public static BetResultType Cashout = new BetResultType()
        {
            Value = 4,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.BetResultType_ExchangeCash)
        };
    }
}