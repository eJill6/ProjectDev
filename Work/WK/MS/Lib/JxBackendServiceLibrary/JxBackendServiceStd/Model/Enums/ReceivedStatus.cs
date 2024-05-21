using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums
{
    /// <summary>
    /// 通用型領取狀態
    /// </summary>
    public class ReceivedStatus : BaseIntValueModel<ReceivedStatus>
    {
        private ReceivedStatus()
        {
            ResourceType = typeof(SelectItemElement);
        }

        /// <summary>
        /// 未領取
        /// </summary>
        public static readonly ReceivedStatus NotReceive = new ReceivedStatus()
        {
            Value = 0,
            ResourcePropertyName = nameof(SelectItemElement.ReceivedStatus_NotReceive)
        };

        /// <summary>
        /// 已領取
        /// </summary>
        public static readonly ReceivedStatus Received = new ReceivedStatus()
        {
            Value = 1,
            ResourcePropertyName = nameof(SelectItemElement.ReceivedStatus_Received)
        };
    }
}