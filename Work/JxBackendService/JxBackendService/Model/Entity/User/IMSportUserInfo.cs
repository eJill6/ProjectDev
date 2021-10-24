using System;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.User
{
    public partial class IMSportUserInfo : BaseTPGameUserInfo
    {
        public DateTime? LastIMSportUpdateTime { get; set; }
    }
}
