using JxBackendService.Common.Util;

namespace JxBackendService.Model.ViewModel.Chat
{
    public class ChatMessageViewModel
    {
        /// <summary>訊息發送人ID</summary>
        public int PublishUserID { get; set; }

        /// <summary>訊息ID</summary>
        public long MessageID { get; set; }

        public string MessageIDText => MessageID.ToString();

        /// <summary>訊息類別</summary>
        public int MessageType { get; set; }

        /// <summary>訊息內容</summary>
        public string Message { get; set; }

        /// <summary>訊息發送時間</summary>
        public long PublishTimestamp { get; set; }

        /// <summary>訊息發送文字時間</summary>
        public string PublishDateTimeText => PublishTimestamp.ToDateTime().ToFormatDateTimeString();
    }
}