using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums.Chat;

namespace JxBackendService.Model.Entity.Chat
{
    public class MSIMOneOnOneChatMessageKey
    {
        [ExplicitKey]
        public int OwnerUserID { get; set; }

        [ExplicitKey]
        public int DialogueUserID { get; set; }

        [ExplicitKey]
        public long MessageID { get; set; }
    }

    public class MSIMOneOnOneChatMessage : MSIMOneOnOneChatMessageKey
    {
        public int MessageType { get; set; }

        public string Message { get; set; }

        public long PublishTimestamp { get; set; }

        public int PublishUserID { get; set; }
    }
}