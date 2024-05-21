using System;

namespace MS.Core.MMModel.Models.Chat
{
    public class MSIMOneOnOneChatMessageViewModel
    {
        public int OwnerUserID { get; set; }

        public int DialogueUserID { get; set; }

        public long MessageID { get; set; }

        public string MessageIDText => MessageID.ToString();

        public int MessageType { get; set; }

        public string Message { get; set; }

        public long PublishTimestamp { get; set; }

        public string PublishDateTimeText
        {
            get
            {
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddMilliseconds(PublishTimestamp).ToLocalTime();
                return dtDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public int PublishUserID { get; set; }
    }
}
