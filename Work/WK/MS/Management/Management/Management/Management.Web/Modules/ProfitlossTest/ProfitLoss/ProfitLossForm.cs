using Serenity.ComponentModel;
using Serenity.Web;
using System;

namespace Management.ProfitlossTest.Forms
{
    [FormScript("ProfitlossTest.ProfitLoss")]
    [BasedOnRow(typeof(ProfitLossRow), CheckNames = true)]
    public class ProfitLossForm
    {
        public int UserId { get; set; }
        public DateTime ProfitLossTime { get; set; }
        public string ProfitLossType { get; set; }
        public decimal ProfitLossMoney { get; set; }
        public decimal WinMoney { get; set; }
        public decimal PrizeMoney { get; set; }
        public decimal AllBetMoney { get; set; }
        public string GameType { get; set; }
        public string PlayId { get; set; }
        public string Memo { get; set; }
    }
}