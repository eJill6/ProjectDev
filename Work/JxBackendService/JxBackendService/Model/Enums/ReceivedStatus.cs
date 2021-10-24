using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums
{
    public class ReceivedStatus : BaseIntValueModel<ReceivedStatus>
    {
        private ReceivedStatus() { }


        /// <summary>
        /// 未領取
        /// </summary>
        public static readonly ReceivedStatus NotReceive = new ReceivedStatus()
        {
            Value = 0,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.ReceivedStatus_NotReceive)
        };

        /// <summary>
        /// 已領取
        /// </summary>
        public static readonly ReceivedStatus Received = new ReceivedStatus()
        {
            Value = 1,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.ReceivedStatus_Received)
        };
    }
}