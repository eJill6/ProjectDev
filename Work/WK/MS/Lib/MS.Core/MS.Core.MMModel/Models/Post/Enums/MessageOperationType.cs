using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MS.Core.MMModel.Models.Post.Enums
{
    public enum MessageOperationType
    {
        [Description("用户已读")]
        UserRead = 1,
        [Description("用户删除")]
        UserDelete = 2
    }
}
