using JxBackendService.Interface.Model.MessageQueue;
using JxBackendService.Model.Enums;

namespace JxBackendService.Model.MessageQueue
{
    public class RoutingSetting : IRoutingSetting
    {
        public string RoutingKey { get; set; }

        public string RequestId { get; set; }
    }

    public class MessageQueueParam
    {
    }

    public class TransferMessage : MessageQueueParam
    {
        public string ProductCode { get; set; }

        public string Summary { get; set; }

        public bool IsReloadMiseLiveBalance { get; set; }

        public string RequestId { get; set; }
    }

    public class BWUserLogoutMessage : MessageQueueParam
    {
        public int UserID { get; set; }
    }

    public class BWUserChangePasswordMessage : MessageQueueParam
    {
        public int UserID { get; set; }
    }

    public class UpdateTPGameUserScoreParam : MessageQueueParam
    {
        public PlatformProduct Product { get; set; }

        public int UserID { get; set; }
    }

    public class TelegramQueueMessage
    {
        public string Message { get; set; }

        public string telegramChatGroupValue { get; set; }
    }
}