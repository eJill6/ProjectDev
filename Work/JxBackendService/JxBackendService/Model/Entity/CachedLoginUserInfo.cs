using JxBackendService.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Entity
{
    public class CachedLoginUserInfo
    {
        [ExplicitKey]
        public string UserInfoCacheKey { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int MarkToDelete { get; set; }
        public bool MarkNotActive { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
