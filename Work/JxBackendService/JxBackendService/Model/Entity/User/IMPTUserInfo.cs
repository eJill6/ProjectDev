using System;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.User
{
    public partial class IMPTUserInfo : BaseTPGameUserInfo
    {
        public DateTime? LastIMPTUpdateTime { get; set; }
    }
}
