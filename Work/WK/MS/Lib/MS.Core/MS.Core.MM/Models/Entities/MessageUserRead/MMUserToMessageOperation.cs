using Microsoft.OpenApi.Models;
using MS.Core.Attributes;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Entities.MessageUserRead
{
    public class MMUserToMessageOperation: BaseDBModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [AutoKey]
        public int Id { get; set; }
        /// <summary>
        /// 消息类型 1.公告消息 2.投诉消息
        /// </summary>
        public MessageType MessageType { get; set; }
        /// <summary>
        /// 消息ID
        /// </summary>
        public string MessageId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 用户已读
        /// </summary>
        public bool IsRead { get; set; }
        /// <summary>
        /// 用户删除
        /// </summary>
        public bool IsDelete { get; set; }


    }
}
