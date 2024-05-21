using JxBackendService.Model.ThirdParty.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ThirdParty.AllBet
{
    public class ABEBBetLog : BaseRemoteBetLog
    {
        public int appType { get; set; }
        public decimal betAmount { get; set; }
        public int betMethod { get; set; }
        public long betNum { get; set; }
        public string betTime { get; set; }
        public int betType { get; set; }
        public string client { get; set; }
        public int commission { get; set; }
        public string currency { get; set; }
        public decimal deposit { get; set; }
        public decimal exchangeRate { get; set; }
        public string gameResult { get; set; }
        public string gameRoundEndTime { get; set; }
        public int gameRoundId { get; set; }
        public string gameRoundStartTime { get; set; }
        public int gameType { get; set; }
        public string ip { get; set; }
        public int state { get; set; }
        public int status { get; set; }
        public string tableName { get; set; }
        public decimal validAmount { get; set; }
        public decimal winOrLoss { get; set; }

        public override string KeyId => $"{betNum}";

        public override string TPGameAccount => client;
    }
}
