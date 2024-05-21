using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MS.Core.MMModel.Models.Post.Enums
{
    public enum UserOperationMessageType
    {
        [Description("用户已读")]
        Read = 1,
        [Description("用户删除")]
        Delete = 2
    }
}
