using JxBackendService.Resource.Element;
using System.Collections.Generic;

namespace JxBackendService.Model.Enums
{
    /// <summary>
    /// 盈虧
    /// </summary>
    public class ProfitLossTypeName : BaseStringValueModel<ProfitLossTypeName>
    {
        private ProfitLossTypeName()
        {
            ResourceType = typeof(SelectItemElement);
        }

        public RefundType RefundTypeParam { get; private set; }

        public string[] SubChangeTypes { get; private set; }

        public bool IsGivePrizesNeedBankSelection { get; private set; }

        public List<string> TableValues { get; private set; }

        /// <summary>充值</summary>
        public static ProfitLossTypeName CZ = new ProfitLossTypeName()
        {
            Value = "充值",
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_CZ),
            RefundTypeParam = RefundType.RechargeBySystem,
            IsGivePrizesNeedBankSelection = true
        };

        /// <summary>提现</summary>
        public static ProfitLossTypeName TX = new ProfitLossTypeName()
        {
            Value = "提现",
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_TX),
            SubChangeTypes = new string[]
            {
                "提现申请",
                "提现确认",
                "提现退款",
            }
        };

        /// <summary>抽水</summary>
        public static ProfitLossTypeName FD = new ProfitLossTypeName()
        {
            Value = "抽水",
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_FD)
        };

        /// <summary>亏盈</summary>
        public static ProfitLossTypeName KY = new ProfitLossTypeName()
        {
            Value = "亏盈",
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_KY),
        };

        /// <summary>红包</summary>
        public static ProfitLossTypeName HB = new ProfitLossTypeName()
        {
            Value = "红包",
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_HB),
            RefundTypeParam = RefundType.HBBySystem,
        };

        /// <summary>转账</summary>
        public static ProfitLossTypeName ZZ = new ProfitLossTypeName()
        {
            Value = "转账",
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_ZZ),
            TableValues = new List<string>() { "转入", "转出" },
        };

        /// <summary>佣金</summary>
        public static ProfitLossTypeName YJ = new ProfitLossTypeName()
        {
            Value = "佣金",
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_YJ),
            RefundTypeParam = RefundType.YJBySystem,
        };

        /// <summary>契约分红</summary>
        public static ProfitLossTypeName Commission = new ProfitLossTypeName()
        {
            Value = "契约分红",
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_Commission)
        };

        /// <summary>派奖</summary>
        public static ProfitLossTypeName Prizes = new ProfitLossTypeName()
        {
            Value = "派奖",
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_Prizes),
            RefundTypeParam = RefundType.PrizeBySystem
        };

        /// <summary>活动</summary>
        public static ProfitLossTypeName Activity = new ProfitLossTypeName()
        {
            Value = "活动",
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_Activity),
            RefundTypeParam = RefundType.Activity,
        };

        /// <summary>调账</summary>
        public static ProfitLossTypeName Adjustments = new ProfitLossTypeName()
        {
            Value = "调账",
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_Adjustments),
            RefundTypeParam = RefundType.Adjustments,
        };

        /// <summary>调整流水</summary>
        public static ProfitLossTypeName ChangeAccumulateFlowAmount = new ProfitLossTypeName()
        {
            Value = "调整流水",
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_ChangeAccumulateFlowAmount)
        };

        /// <summary>代理代存</summary>
        public static ProfitLossTypeName VIPAgentDepositForChild = new ProfitLossTypeName()
        {
            Value = "代理代存",
            ResourcePropertyName = nameof(SelectItemElement.ProfitLossTypeName_VIPAgentDepositForChild),
        };
    }
}