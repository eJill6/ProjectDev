using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class ProfitLossTypeName : BaseStringValueModel<ProfitLossTypeName>
    {
        private ProfitLossTypeName() { }

        public RefundType RefundTypeParam { get; private set; }

        public bool IsGivePrizesNeedBankSelection { get; private set; }

        /// <summary>充值</summary>
        public static ProfitLossTypeName CZ = new ProfitLossTypeName()
        {
            Value = "充值",
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_CZ),
            RefundTypeParam = RefundType.RechargeBySystem,
            IsGivePrizesNeedBankSelection = true
        };

        /// <summary>提现</summary>
        public static ProfitLossTypeName TX = new ProfitLossTypeName()
        {
            Value = "提现",
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_TX)
        };

        /// <summary>抽水</summary>
        public static ProfitLossTypeName FD = new ProfitLossTypeName()
        {
            Value = "抽水",
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_FD)
        };

        /// <summary>亏盈</summary>
        public static ProfitLossTypeName KY = new ProfitLossTypeName()
        {
            Value = "亏盈",
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_KY),
        };

        /// <summary>红包</summary>
        public static ProfitLossTypeName HB = new ProfitLossTypeName()
        {
            Value = "红包",
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_HB),
            RefundTypeParam = RefundType.HBBySystem
        };

        /// <summary>转账</summary>
        public static ProfitLossTypeName ZZ = new ProfitLossTypeName()
        {
            Value = "转账",
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_ZZ)
        };

        /// <summary>佣金</summary>
        public static ProfitLossTypeName YJ = new ProfitLossTypeName()
        {
            Value = "佣金",
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_YJ),
            RefundTypeParam = RefundType.YJBySystem
        };

        /// <summary>契约分红</summary>
        public static ProfitLossTypeName Commission = new ProfitLossTypeName()
        {
            Value = "契约分红",
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_Commission)
        };

        /// <summary>派奖</summary>
        public static ProfitLossTypeName Prizes = new ProfitLossTypeName()
        {
            Value = "派奖",
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_Prizes),
            RefundTypeParam = RefundType.PrizeBySystem
        };

        /// <summary>活动</summary>
        public static ProfitLossTypeName Activity = new ProfitLossTypeName()
        {
            Value = "活动",
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_Activity),
            RefundTypeParam = RefundType.Activity,
        };

        /// <summary>调账</summary>
        public static ProfitLossTypeName Adjustments = new ProfitLossTypeName()
        {
            Value = "调账",
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_Adjustments),
            RefundTypeParam = RefundType.Adjustments,
        };
    }

    public class BetResultType : BaseIntValueModel<BetResultType>
    {
        private BetResultType() { }

        public static BetResultType Win = new BetResultType()
        {
            Value = 1,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.BetResultType_Win)
        };

        public static BetResultType Lose = new BetResultType()
        {
            Value = 0,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.BetResultType_Lose)
        };

        public static BetResultType Draw = new BetResultType()
        {
            Value = -1,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.BetResultType_Draw)
        };
    }

    public class PTBetResultType : BaseIntValueModel<PTBetResultType>
    {
        private PTBetResultType() { }

        public static PTBetResultType Win = new PTBetResultType()
        {
            Value = 1,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.BetResultType_Win)
        };

        public static PTBetResultType Lose = new PTBetResultType()
        {
            Value = 0,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.BetResultType_Lose)
        };

        public static PTBetResultType Draw = new PTBetResultType()
        {
            Value = -1,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.BetResultType_Draw)
        };

        public static PTBetResultType NoFactionAward = new PTBetResultType()
        {
            Value = -2
        };

        public static PTBetResultType Cancel = new PTBetResultType()
        {
            Value = -3
        };
    }
}
