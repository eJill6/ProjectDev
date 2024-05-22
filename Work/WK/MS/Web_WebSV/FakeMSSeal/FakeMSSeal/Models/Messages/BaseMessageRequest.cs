using System.Text.Json.Nodes;

namespace FakeMSSeal.Models.Messages
{
    public class BaseMessageRequest : BaseRequest
    {

        /// <summary>
        /// 消息推送的內容
        /// </summary>
        public JsonArray Content { get; set; }

        /// <summary>
        /// 平台名稱
        /// </summary>
        public string App { get; set; } = "amd";

        /// <inheritdoc cref="Enums.MessageType"/>
        public int Type { get; set; }
    }
}
