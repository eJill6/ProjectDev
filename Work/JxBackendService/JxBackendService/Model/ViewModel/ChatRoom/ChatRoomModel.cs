using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.ChatRoom
{
    /// <summary>
    /// [人員]聊天室列表Model
    /// </summary>
    public class MemberChatRoomListViewModel
    {
        public MemberChatRoomListViewModel()
        {
            ParentChatRoomList = new List<MemberChatRoomViewModel>();
            ChildChatRoomList = new List<MemberChatRoomViewModel>();
        }

        public List<MemberChatRoomViewModel> ParentChatRoomList { get; set; }
        public List<MemberChatRoomViewModel> ChildChatRoomList { get; set; }
    }

    /// <summary>
    /// [人員]聊天室詳細Model
    /// </summary>
    public class MemberChatRoomViewModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int AvatarId { get; set; }

        public int UnReadMessageTotalCount { get; set; }
    }

    /// <summary>
    /// [群聊]聊天室列表Model
    /// </summary>
    public class GroupChatRoomListViewModel
    {
        public GroupChatRoomListViewModel()
        {
            JoinedChatRoomList = new List<GroupChatRoomViewModel>();
            ManageChatRoomList = new List<GroupChatRoomViewModel>();
        }

        public List<GroupChatRoomViewModel> JoinedChatRoomList { get; set; }

        public int ManageChatRoomMaxCount { get; set; }

        public List<GroupChatRoomViewModel> ManageChatRoomList { get; set; }
    }

    /// <summary>
    /// [人員]聊天室詳細Model
    /// </summary>
    public class GroupChatRoomViewModel
    {
        public int ChatRoomId { get; set; }

        public string ChatRoomName { get; set; }

        public int ChatRoomMemberCount { get; set; }

        public int UnReadMessageTotalCount { get; set; }

        public bool EnableSendMessage { get; set; }
    }

    /// <summary>
    /// [群聊]聊天室下級列表Model[新增模式]
    /// </summary>
    public class GroupChatRoomChildCreateModeViewModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int AvatarId { get; set; }

        public string LastLoginTime { get; set; }
    }

    /// <summary>
    /// [群聊]聊天室下級列表Model[編輯模式]
    /// </summary>
    public class GroupChatRoomChildEditModeViewModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int AvatarId { get; set; }

        public string LastLoginTime { get; set; }

        public bool EnableSendMessage { get; set; }
    }

    /// <summary>
    /// [群聊]聊天室訊息Model
    /// </summary>
    public class GroupChatRoomMessageViewModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int AvatarId { get; set; }

        public long MessageId { get; set; }

        public string MessageContent { get; set; }

        public string SendTime { get; set; }
    }

    /// <summary>
    /// [群聊]發送MQ訊息時SendContent的自訂義Model
    /// </summary>
    public class GroupChatRoomMqSendContentModel
    {
        /// <summary>
        /// 所屬的站內信群聊 GroupId
        /// </summary>
        public int BelongGroupId { get; set; } = 0;

        /// <summary>
        /// 發送訊息的 UserId
        /// </summary>
        public int PublishUserId { get; set; } = 0;

        /// <summary>
        /// 發送訊息的 User Name
        /// </summary>
        public string PublishUserName { get; set; } = string.Empty;

        /// <summary>
        /// 發送訊息的內容
        /// </summary>
        public string MessageContent { get; set; } = string.Empty;
    }

    /// <summary>
    /// 站內信用戶發送訊息數量與時間Model
    /// </summary>
    public class ChatRoomUserSendMessageCountViewModel
    {
        public int SendTotalCount { get; set; }

        public DateTime StartSendTime { get; set; }

        public bool IsFirstSend { get; set; }
    }
}
