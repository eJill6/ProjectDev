using Serenity.ComponentModel;
using Serenity.Web;
using System;

namespace Management.LotteryHistory.Forms
{
    [FormScript("LotteryHistory.LotteryNum")]
    [BasedOnRow(typeof(LotteryNumRow), CheckNames = true)]
    public class LotteryNumForm
    {
        public DateTime CurrentLotteryTime { get; set; }
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