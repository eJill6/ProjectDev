using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Entities.MessageUserRead
{
    /// <summary>
    /// 用户未读消息数
    /// </summary>
    public class UserUnreadMessage
    {
        /// <summary>
        /// 公告未读消息
        /// </summary>
        public int AnnouncementUnreadCount { get; set; }
        /// <summary>
        /// 投诉未读消息
        /// </summary>
        public int ComplaintUnreadCount { get; set; }

        /// <summary>
        /// 公告已读消息
        /// </summary>
        public int AnnouncementReadCount { get; set; }
        /// <summary>
        /// 投诉已读消息
        /// </summary>
        public int ComplaintReadCount { get; set; }
        /// <summary>
        /// 公告消息
        /// </summary>
        public int AnnouncementCount { get; set; }
        /// <summary>
        /// 投诉消息
        /// </summary>
        public int ComplaintCount { get; set; }
        /// <summary>
        /// 总未读消息
        /// </summary>
        public int TotalUnreadCount { get; set; }
    }
}
