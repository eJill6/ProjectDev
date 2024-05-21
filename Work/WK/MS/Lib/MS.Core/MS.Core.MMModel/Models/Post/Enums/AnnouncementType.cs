using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MS.Core.MMModel.Models.Post.Enums
{
    public enum AnnouncementType
    {
        /// <summary>
        /// 公告
        /// </summary>
        [Description("公告")]
        Announcement = 1,

        /// <summary>
        /// 首页公告
        /// </summary>
        [Description("首页公告")]
        HomeAnnouncement = 2,
    }
}
