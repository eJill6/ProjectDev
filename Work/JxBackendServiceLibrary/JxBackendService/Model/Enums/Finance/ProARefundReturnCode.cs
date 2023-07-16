using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums.Finance
{
    public class ProARefundReturnCode : BaseStringValueModel<ProARefundReturnCode>
    {
        private ProARefundReturnCode(string value)
        {
            Value = value;
        }

        public static readonly ProARefundReturnCode Success = new ProARefundReturnCode("02")
        {
            ResourceType = typeof(ReturnCodeElement),
            ResourcePropertyName = nameof(ReturnCodeElement.Success)
        };

        public static readonly ProARefundReturnCode ParameterIsInvalid = new ProARefundReturnCode("00")
        {
            ResourceType = typeof(ReturnCodeElement),
            ResourcePropertyName = nameof(ReturnCodeElement.ParameterIsInvalid)
        };

        public static readonly ProARefundReturnCode FreezeScoresNotEnough = new ProARefundReturnCode("01")
        {
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.ProARefundReturnCode_FreezeScoresNotEnough)
        };

        public static readonly ProARefundReturnCode OrderAlreadyDone = new ProARefundReturnCode("03")
        {
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.ProARefundReturnCode_OrderAlreadyDone)
        };

        public static readonly ProARefundReturnCode Error = new ProARefundReturnCode("04")
        {
            ResourceType = typeof(ReturnCodeElement),
            ResourcePropertyName = nameof(ReturnCodeElement.SystemError)
        };

        public static readonly ProARefundReturnCode OrderProcessing = new ProARefundReturnCode("05")
        {
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.ProARefundReturnCode_OrderProcessing)
        };
    }
}