namespace JxBackendService.Model.Param.Chat
{
    public class BothChatUserIDParam
    {
        public int OwnerUserID { get; set; }

        public int DialogueUserID { get; set; }
    }

    public class QueryOneOnOneMessageParam
    {
        public BothChatUserIDParam BothChatUserIDParam { get; set; }

        public long PublishTimestamp { get; set; }
    }
}