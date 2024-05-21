using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums.Chat;
using System.Text.Json.Serialization;

namespace JxBackendService.Model.Param.Chat
{
    public class SendMessageParam
    {
        /// <summary>對方用戶ID或群組ID</summary>
        [CustomizedRequired]
        public string RoomID { get; set; }

        /// <summary>訊息類別</summary>
        public int MessageTypeValue { get; set; }

        /// <summary>訊息內容</summary>
        [CustomizedRequired]
        [CustomizedMaxLength(8000)]
        public string Message { get; set; }

        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [CustomizedRequired]
        public MessageType MessageType => MessageType.GetSingle(MessageTypeValue);
    }

    public class SendAddMessageQueueParam : SendMessageParam
    {
        public int OwnerUserID { get; set; }

        public int PublishUserID { get; set; }

        public long MessageID { get; set; }

        public long PublishTimestamp { get; set; }
    }
}