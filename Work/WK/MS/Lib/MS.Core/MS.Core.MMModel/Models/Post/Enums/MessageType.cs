using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MS.Core.MMModel.Models.Post.Enums
{
    public enum MessageType
    {
        [Description("公告")]
        Announcement=1,
        [Description("投诉")]
        ComplaintPost = 2
    }
}
