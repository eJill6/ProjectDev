using JxBackendService.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Entity.Base
{
    public class BaseTPGameUserInfo
    {
        [ExplicitKey]
        public int UserID { get; set; }

        [NVarcharColumnInfo(50)]
        public string UserName { get; set; }

        public decimal? TransferIn { get; set; }

        public decimal? TransferOut { get; set; }

        public decimal? WinOrLoss { get; set; }

        public decimal? Rebate { get; set; }

        public decimal? AvailableScores { get; set; }

        public decimal? FreezeScores { get; set; }

        /// <summary>
        /// 根據子類別邏輯不同產生覆寫
        /// </summary>
        public virtual decimal GetAvailableScores() => AvailableScores.GetValueOrDefault();
    }

    public class TPGameUserInfoWithLastUpdateTime : BaseTPGameUserInfo
    {
        public DateTime? LastUpdateTime { get; set; }
    }
}
