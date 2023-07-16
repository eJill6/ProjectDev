using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums.ThirdParty
{
    public class TPGameMoneyTransferActionType : BaseValueModel<bool, TPGameMoneyTransferActionType>
    {
        private TPGameMoneyTransferActionType()
        { }

        public static TPGameMoneyTransferActionType MoneyIn = new TPGameMoneyTransferActionType()
        {
            Value = true,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.MoneyInActionName)
        };

        public static TPGameMoneyTransferActionType MoneyOut = new TPGameMoneyTransferActionType()
        {
            Value = false,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.MoneyOutActionName)
        };
    }
}