
using System.Text.Json.Nodes;

namespace FakeMSSeal.Models
{
    /// <summary>
    /// 消息推送
    /// </summary>
    /// <typeparam name="T">消息推送的類別</typeparam>
    public class MessageRequest<T> : BaseRequest where T : class
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

        /// <inheritdoc/>
        public override bool IsSkipSignSalt() => false;
    }
}
