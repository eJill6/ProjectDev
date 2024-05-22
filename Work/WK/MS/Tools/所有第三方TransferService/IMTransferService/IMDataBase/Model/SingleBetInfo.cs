using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDataBase.Model
{
    public class SingleBetInfo

    {
        public string Provider { get; set; }

        public string GameId { get; set; }

        public string BetId { get; set; }

        public string WagerCreationDateTime { get; set; }

        public string LastUpdatedDate { get; set; }

        public string PlayerId { get; set; }

        public string ProviderPlayerId { get; set; }

        public string Currency { get; set; }

        public string StakeAmount { get; set; }

        public string WinLoss { get; set; }

        public string OddsType { get; set; }

        public string WagerType { get; set; }

        public string Platform { get; set; }

        public int IsSettled { get; set; }

        public int IsCancelled { get; set; }

        public string SettlementDateTime { get; set; }

        public string DetailItems { get; set; }

        public string PlayName { get; set; }
    }
}