using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums
{
    public class BudgetType : BaseIntValueModel<BudgetType>
    {
        private BudgetType()
        {
            ResourceType = typeof(SelectItemElement);
        }

        /// <summary>充值</summary>
        public static readonly BudgetType Recharge = new BudgetType()
        {
            Value = 1,
            ResourcePropertyName = nameof(SelectItemElement.BudgetType_Recharge)
        };

        /// <summary>提现申请</summary>
        public static readonly BudgetType WithdrawApply = new BudgetType()
        {
            Value = 2,
            ResourcePropertyName = nameof(SelectItemElement.BudgetType_WithdrawApply)
        };

        /// <summary>提现确认</summary>
        public static readonly BudgetType WithdrawConfirm = new BudgetType()
        {
            Value = 3,
            ResourcePropertyName = nameof(SelectItemElement.BudgetType_WithdrawConfirm)
        };

        /// <summary>提现退款</summary>
        public static readonly BudgetType WithdrawRefund = new BudgetType()
        {
            Value = 4,
            ResourcePropertyName = nameof(SelectItemElement.BudgetType_WithdrawRefund)
        };
    }
}