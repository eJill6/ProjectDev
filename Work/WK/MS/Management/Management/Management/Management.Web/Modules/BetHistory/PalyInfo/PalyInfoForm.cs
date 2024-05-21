using Serenity.ComponentModel;
using Serenity.Web;
using System;

namespace Management.BetHistory.Forms
{
    [FormScript("BetHistory.PalyInfo")]
    [BasedOnRow(typeof(PalyInfoRow), CheckNames = true)]
    public class PalyInfoForm
    {
        public string PalyCurrentNum { get; set; }
        public string PalyNum { get; set; }
        public int PlayTypeId { get; set; }
        public int LotteryId { get; set; }
        public string UserName { get; set; }
        public int NoteNum { get; set; }
        public decimal SingleMoney { get; set; }
        public decimal NoteMoney { get; set; }
        public DateTime NoteTime { get; set; }
        public bool IsWin { get; set; }
        public decimal WinMoney { get; set; }
        public int IsFactionAward { get; set; }
        public int PlayTypeRadioId { get; set; }
        public decimal RebatePro { get; set; }
        public string RebateProMoney { get; set; }
        public int WinNum { get; set; }
        public int UserId { get; set; }
        public int NoticeId { get; set; }
        public DateTime LotteryTime { get; set; }
        public decimal UserRebatePro { get; set; }
        public int Multiple { get; set; }
        public string OrderKey { get; set; }
        public decimal CurrencyUnit { get; set; }
        public int Ratio { get; set; }
        public string SourceType { get; set; }
        public string MemoJson { get; set; }
        public string ClientIp { get; set; }
        public string RoomId { get; set; }
        public string ResultJson { get; set; }
    }
}