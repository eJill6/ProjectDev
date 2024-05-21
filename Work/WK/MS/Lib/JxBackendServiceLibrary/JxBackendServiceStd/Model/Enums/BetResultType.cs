using JxBackendService.Common.Util;
using JxBackendService.Resource.Element;
using System;

namespace JxBackendService.Model.Enums
{
    public class BetResultType : BaseIntValueModel<BetResultType>
    {
        private BetResultType()
        { }

        public Func<decimal, int> ToMiseLiveOrderStatus { get; private set; }

        /// <summary>贏</summary>
        public static BetResultType Win = new BetResultType()
        {
            Value = 1,
            ToMiseLiveOrderStatus = (winMoney) => (int)MiseLiveOrderStatuses.Win,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.BetResultType_Win)
        };

        /// <summary>輸</summary>
        public static BetResultType Lose = new BetResultType()
        {
            Value = 0,
            ToMiseLiveOrderStatus = (winMoney) => (int)MiseLiveOrderStatuses.Lose,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.BetResultType_Lose)
        };

        /// <summary>和局</summary>
        public static BetResultType Draw = new BetResultType()
        {
            Value = -1,
            ToMiseLiveOrderStatus = (winMoney) => (int)MiseLiveOrderStatuses.Draw,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.BetResultType_Draw)
        };

        /// <summary>半贏</summary>
        public static BetResultType HalfWin = new BetResultType()
        {
            Value = 2,
            ToMiseLiveOrderStatus = (winMoney) => (int)MiseLiveOrderStatuses.Win,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.BetResultType_HalfWin)
        };

        /// <summary>半輸</summary>
        public static BetResultType HalfLose = new BetResultType()
        {
            Value = 3,
            ToMiseLiveOrderStatus = (winMoney) => (int)MiseLiveOrderStatuses.Lose,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.BetResultType_HalfLose)
        };

        /// <summary>兌現</summary>
        public static BetResultType Cashout = new BetResultType()
        {
            Value = 4,
            ToMiseLiveOrderStatus = (winMoney) => winMoney.ToBetResultType().ToMiseLiveOrderStatus(winMoney),
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.BetResultType_ExchangeCash)
        };
    }

    public enum MiseLiveOrderStatuses
    {
        Win = 1,

        Lose = 2,

        Draw = 3,
    }
}