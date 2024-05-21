using RestSharp;
using SLPolyGame.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLPolyGame.Web.MSSeal.Models
{
    /// <summary>
    /// 消息推送
    /// </summary>
    /// <typeparam name="T">消息推送的類別</typeparam>
    public class MessageRequest<T> : BaseRequest where T : class
    {
        /// <inheritdoc/>
        public override Method GetMethod() => Method.Post;

        /// <inheritdoc/>
        public override string GetResource() => "/dapi/message";

        /// <summary>
        /// 消息推送的內容
        /// </summary>
        public T[] Content { get; set; }

        /// <summary>
        /// 平台名稱
        /// </summary>
        public string App { get; set; } = "amd";

        /// <inheritdoc cref="Enums.MessageType"/>
        public int Type { get; set; }
    }
}