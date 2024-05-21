using JxBackendService.Common.Util;

namespace JxBackendService.Model.Param.Chat
{
    public class ChatNotificationParam
    {
        public string RoutingKey { get; set; }

        public ChatNotificationInfo ChatNotificationInfo { get; set; }
    }

    public class ChatNotificationInfo
    {
        public ChatNotificationTypes ChatNotificationType { get; set; }

        public string RoomID { get; set; }

        public long MessageID { get; set; }

        public string MessageIDText => MessageID.ToString();

        public long PublishTimestamp { get; set; }

        public string PublishDateTimeText => PublishTimestamp.ToDateTime().ToFormatDateTimeString();

        public string Message { get; set; }
    }

    public enum ChatNotificationTypes
    {
        NewMessage = 1,

        DeleteChat = 2,
    }
}