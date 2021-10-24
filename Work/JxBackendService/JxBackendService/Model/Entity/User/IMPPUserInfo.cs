using System;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.User
{
    public partial class IMPPUserInfo : BaseTPGameUserInfo
    {
        public DateTime? LastIMPPUpdateTime { get; set; }
    }
}
