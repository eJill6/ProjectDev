using JxBackendService.Model.Attributes;
using System;

namespace JxBackendService.Model.Entity
{
    public class BaseUserInfoEntityModel
    {
        public string UserName { get; set; }

        [ExplicitKey]
        public int UserID { get; set; }
    }

    public class UserInfo : BaseUserInfoEntityModel
    {
        public decimal RebatePro { get; set; }

        public decimal AddedRebatePro { get; set; }

        public decimal? AvailableScores { get; set; } = 0;

        public decimal? FreezeScores { get; set; } = 0;

        public DateTime? ScoreChangeDate { get; set; }
    }
}