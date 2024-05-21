using JxBackendService.Model.Attributes;

namespace JxBackendService.Model.Entity.Chat
{
    public class MSIMLastMessageKey
    {
        [ExplicitKey]
        public int OwnerUserID { get; set; }

        [ExplicitKey]
        public string RoomID { get; set; }

        public long MessageID { get; set; }
    }

    public class MSIMLastMessageInfo : MSIMLastMessageKey
    {
        public int MessageType { get; set; }

        public string Message { get; set; }

        public long PublishTimestamp { get; set; }

        public int UnreadCount { get; set; }
    }
}