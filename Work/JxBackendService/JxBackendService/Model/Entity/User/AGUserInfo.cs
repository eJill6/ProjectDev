using System;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.User
{
    public partial class AGUserInfo : BaseTPGameUserInfo
    {
        public decimal? FishAvailableScores { get; set; }
        public DateTime? LastFishUpdateTime { get; set; }
        public DateTime? LastAgUpdateTime { get; set; }

        public override decimal GetAvailableScores()
        {
            return base.GetAvailableScores() + FishAvailableScores.GetValueOrDefault();
        }
    }
}
