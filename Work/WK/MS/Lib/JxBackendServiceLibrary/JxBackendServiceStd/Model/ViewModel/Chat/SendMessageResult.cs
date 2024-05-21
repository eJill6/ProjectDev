using JxBackendService.Common.Util;

namespace JxBackendService.Model.ViewModel.Chat
{
    public class SendMessageResult
    {
        /// <summary>訊息ID</summary>
        public long MessageID { get; set; }

        public string MessageIDText => MessageID.ToString();

        /// <summary>發送時間</summary>
        public long PublishTimestamp { get; set; }

        /// <summary>發送顯示文字時間</summary>
        public string PublishDateTimeText => PublishTimestamp.ToDateTime().ToFormatDateTimeString();
    }
}