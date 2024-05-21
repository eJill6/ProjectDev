namespace JxBackendService.Model.Param.Chat
{
    public class BaseQueryLastMessagesParam
    {
        /// <summary>房間號</summary>
        public string RoomID { get; set; }

        /// <summary>列表最後一筆的訊息ID</summary>
        public long? LastMessageID { get; set; }
    }

    public class QueryLastMessagesParam : BaseQueryLastMessagesParam
    {
        /// <summary>對話擁有者ID</summary>
        public int OwnerUserID { get; set; }

        public long? PublishTimestamp { get; set; }
    }

    public class QueryBothLastMessagesParam
    {
        public BothChatUserIDParam BothChatUserIDParam { get; set; }

        public long PublishTimestamp { get; set; }
    }
}