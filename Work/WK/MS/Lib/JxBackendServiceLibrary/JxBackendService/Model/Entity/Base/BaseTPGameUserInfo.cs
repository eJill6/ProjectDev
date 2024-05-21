using JxBackendService.Model.Attributes;
using System;

namespace JxBackendService.Model.Entity.Base
{
    public class BaseTPGameUserInfo
    {
        [ExplicitKey]
        public int UserID { get; set; }

        public decimal TransferIn { get; set; } = 0;

        public decimal TransferOut { get; set; } = 0;

        public decimal WinOrLoss { get; set; } = 0;

        public decimal Rebate { get; set; } = 0;

        public decimal AvailableScores { get; set; } = 0;

        public decimal FreezeScores { get; set; } = 0;
    }

    public class TPGameUserInfoWithLastUpdateTime : BaseTPGameUserInfo
    {
        public DateTime? LastUpdateTime { get; set; }
    }
}