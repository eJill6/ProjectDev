using System;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.User
{
    public partial class IMeBETUserInfo : BaseTPGameUserInfo
    {
        public DateTime? LastIMeBETUpdateTime { get; set; }
    }
}
