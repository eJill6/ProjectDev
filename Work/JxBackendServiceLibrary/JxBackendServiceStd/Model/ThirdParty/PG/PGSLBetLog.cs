using JxBackendService.Model.ThirdParty.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ThirdParty.PG
{
    public class PGSLBetLog : BaseRemoteBetLog
    {
        public long betId { get; set; }

        public long parentBetId { get; set; }

        public string playerName { get; set; }

        public string currency { get; set; }

        public int? gameId { get; set; }

        public int platform { get; set; }

        public int betType { get; set; }

        public int? transactionType { get; set; }

        public decimal betAmount { get; set; }

        public decimal winAmount { get; set; }

        public decimal jackpotRtpContributionAmount { get; set; }

        public decimal jackpotContributionAmount { get; set; }

        public decimal jackpotWinAmount { get; set; }

        public decimal balanceBefore { get; set; }

        public decimal balanceAfter { get; set; }

        public int handsStatus { get; set; }

        public long rowVersion { get; set; }

        public long betTime { get; set; }

        public long betEndTime { get; set; }

        public override string KeyId => $"{betId}";

        public override string TPGameAccount => playerName;
    }
}