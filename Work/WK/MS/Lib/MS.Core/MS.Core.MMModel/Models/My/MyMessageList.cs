using MS.Core.MMModel.Models.Post.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.My
{
    public class MyMessageList
    {
        public string Id { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType MessageType { get; set; }
        /// 消息标题
        public string MessageTitle{get; set; }
        /// 发布时间
        public string PublishTime{get;set; }
        /// 消息内容
        public string MessageContent{get;set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
