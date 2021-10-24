using System;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.User
{
    public partial class IMBGUserInfo : BaseTPGameUserInfo
    {
        public DateTime? LastIMBGUpdateTime { get; set; }
    }
}
