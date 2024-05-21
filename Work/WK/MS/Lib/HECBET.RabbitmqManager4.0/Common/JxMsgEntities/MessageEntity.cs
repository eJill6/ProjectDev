namespace JxMsgEntities
{
    using System;
    using System.Runtime.CompilerServices;

    public class MessageEntity
    {
        public RabbitMessageType MessageType { get; set; }

        public object SendContent { get; set; }

        public string SendExchange { get; set; }

        public string SendRoutKey { get; set; }

        public string SendTime { get; set; }
    }
}