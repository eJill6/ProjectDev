namespace JxMsgEntities
{
    public class GroupChatRoomDetailNoticeEntity : BaseEntity
    {
        public int BelongGroupId { get; set; }

        public string MessageContent { get; set; }

        public int PublishUserId { get; set; }

        public string PublishUserName { get; set; }

        public int PublishUserAvatarId { get; set; }
    }

    public class BaseGroupChatRoomActionControlNoticeEntity : BaseEntity
    {
        public int ActionType { get; set; }

        public int GroupId { get; set; }

        public string GroupName { get; set; }
    }

    public class GroupChatRoomActionControlSendPermissionNoticeEntity : BaseGroupChatRoomActionControlNoticeEntity
    {
        public bool EnableSendMessage { get; set; }
    }

    public class GroupChatRoomActionControlBeJoinedNoticeEntity : BaseGroupChatRoomActionControlNoticeEntity
    {
        public int TotalMemberCount { get; set; }
    }
}
