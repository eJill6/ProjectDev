using JxBackendService.Model.Enums.Finance;
using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums
{
    public class TransferRecordOrderStatus : BaseShortValueModel<TransferRecordOrderStatus>
    {
        public MoneyInDealType CorrelationMoneyInDealType { get; private set; }

        public MoneyOutDealType CorrelationMoneyOutDealType { get; private set; }

        private TransferRecordOrderStatus()
        { }

        public static TransferRecordOrderStatus Processing = new TransferRecordOrderStatus()
        {
            Value = 1,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.TPGameTransferOrderStatus_Processing),
            CorrelationMoneyInDealType = MoneyInDealType.Processing,
            CorrelationMoneyOutDealType = MoneyOutDealType.Processing,
        };

        public static TransferRecordOrderStatus Success = new TransferRecordOrderStatus()
        {
            Value = 2,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.TPGameTransferOrderStatus_Success),
            CorrelationMoneyInDealType = MoneyInDealType.Done,
            CorrelationMoneyOutDealType = MoneyOutDealType.Done,
        };

        public static TransferRecordOrderStatus Fail = new TransferRecordOrderStatus()
        {
            Value = 4,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.TPGameTransferOrderStatus_Fail),
            CorrelationMoneyInDealType = MoneyInDealType.Fail,
            CorrelationMoneyOutDealType = MoneyOutDealType.Refunded,
        };
    }
}