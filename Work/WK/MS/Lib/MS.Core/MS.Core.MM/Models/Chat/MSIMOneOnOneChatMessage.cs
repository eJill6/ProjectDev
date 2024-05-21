using MS.Core.Models;

namespace MS.Core.MM.Models.Chat
{
    public class MSIMOneOnOneChatMessage : BaseDBModel
    {

        public int OwnerUserID { get; set; }

        public int DialogueUserID { get; set; }

        public long MessageID { get; set; }

        public int MessageType { get; set; }

        public string Message { get; set; }

        public long PublishTimestamp { get; set; }

        public int PublishUserID { get; set; }
    }
}
