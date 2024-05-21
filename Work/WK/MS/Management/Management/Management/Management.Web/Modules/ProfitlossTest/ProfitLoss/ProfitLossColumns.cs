using Serenity.ComponentModel;
using System;
using System.ComponentModel;

namespace Management.ProfitlossTest.Columns
{
    [ColumnsScript("ProfitlossTest.ProfitLoss")]
    [BasedOnRow(typeof(ProfitLossRow), CheckNames = true)]
    public class ProfitLossColumns
    {
        [EditLink, DisplayName("Db.Shared.RecordId"), AlignRight]
        public string ProfitLossId { get; set; }
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