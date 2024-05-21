using Serenity.ComponentModel;
using Serenity.Data.Mapping;
using System;
using System.ComponentModel;

namespace Management.LotteryHistory.Columns
{
    [ColumnsScript("LotteryHistory.LotteryNum")]
    [BasedOnRow(typeof(LotteryNumRow), CheckNames = true)]
    public class LotteryNumColumns
    {
        [EditLink, DisplayName("Db.Shared.RecordId"), AlignRight]
        public int CurrentLotteryId { get; set; }
        public DateTime CurrentLotteryTime { get; set; }
        [IgnoreName]
        public int DrawTimeConsuming { get; set; }
        public string LotteryType { get; set; }
        public string CurrentLotteryNum { get; set; }
        public int LotteryId { get; set; }
        public string IssueNo { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsLottery { get; set; }
        public string Msg { get; set; }
    }
}