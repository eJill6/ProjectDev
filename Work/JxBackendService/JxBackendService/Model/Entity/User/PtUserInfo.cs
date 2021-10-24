using System;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.User
{
    public partial class PtUserInfo : BaseTPGameUserInfo
    {
        public DateTime? LastGameDate { get; set; }
    }
}
