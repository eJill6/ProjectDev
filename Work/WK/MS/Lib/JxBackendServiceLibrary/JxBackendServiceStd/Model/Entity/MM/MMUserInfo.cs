using JxBackendService.Model.Attributes;
using System;

namespace JxBackendService.Model.Entity.MM
{
    public class BasicMMUserInfo
    {
        [ExplicitKey]
        public int UserId { get; set; }

        public string Nickname { get; set; }

        public string AvatarUrl { get; set; }
    }

    public class MMUserInfo : BasicMMUserInfo
    {
        public byte UserIdentity { get; set; }

        public byte UserLevel { get; set; }

        public int RewardsPoint { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public DateTime? RegisterTime { get; set; }

        public decimal EarnestMoney { get; set; }

        public int ExtraPostCount { get; set; }

        public string Memo { get; set; }

        public string Contact { get; set; }

        public bool IsOpen { get; set; }
    }
}