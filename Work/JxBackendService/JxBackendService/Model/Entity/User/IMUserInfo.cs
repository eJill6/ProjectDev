using System;
using JxBackendService.Model.Entity.Base;
namespace JxBackendService.Model.Entity.User
{
    public partial class IMUserInfo : BaseTPGameUserInfo
    {
        public DateTime? LastIMUpdateTime { get; set; }
    }
}
