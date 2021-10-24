using System;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.User
{
    public partial class RGUserInfo : BaseTPGameUserInfo
    {
        public DateTime? LastRGUpdateTime { get; set; }
    }
}
