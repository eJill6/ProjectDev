using System;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.User
{
    public partial class LCUserInfo : BaseTPGameUserInfo
    {
        public DateTime? LastLCUpdateTime { get; set; }
    }
}
