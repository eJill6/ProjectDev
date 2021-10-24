using JxBackendService.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel
{
    public class BaseBasicUserInfo
    {
        public int UserId { get; set; }

        public string UserName { get; set; }
    }

    public class BasicUserInfo : BaseBasicUserInfo
    {
        [IgnoreRead]
        public string UserKey { get; set; }
    }

    public class BaseUserInfoToken : BaseBasicUserInfo
    {
        public string Key { get; set; }
    }

    public class RebateUserInfo : BasicUserInfo
    {
        public static readonly decimal DirectorRebatePro = 0.075m;
        public static readonly decimal DirectorAddedRebatePro = 0.002m;

        public decimal? RebatePro { get; set; }
        public decimal? AddedRebatePro { get; set; }

        public bool IsDirector
        {
            get => (RebatePro == DirectorRebatePro && AddedRebatePro == DirectorAddedRebatePro);
            set { }
        }
    }
}
