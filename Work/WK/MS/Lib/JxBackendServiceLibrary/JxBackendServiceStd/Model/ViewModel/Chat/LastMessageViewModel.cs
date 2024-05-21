using JxBackendService.Common.Util;

namespace JxBackendService.Model.ViewModel.Chat
{
    public class LastMessageViewModel
    {
        /// <summary>房間號</summary>
        public string RoomID { get; set; }

        /// <summary>顯示名稱</summary>
        public string RoomName { get; set; }

        /// <summary>頭像網址</summary>
        public string AvatarUrl { get; set; }

        /// <summary>最後一則訊息ID</summary>
        public long MessageID { get; set; }

        public string MessageIDText => MessageID.ToString();

        public int MessageType { get; set; }

        /// <summary>最後一則訊息內容</summary>
        public string Message { get; set; }

        /// <summary>最後一則訊息發送時間</summary>
        public long PublishTimestamp { get; set; }

        /// <summary>最後一則訊息顯示文字時間</summary>
        public string PublishDateTimeText => PublishTimestamp.ToDateTime().ToFormatDateTimeString();

        /// <summary>未讀數量</summary>
        public int UnreadCount { get; set; }
    }
}