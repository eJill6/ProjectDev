using System;
using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.Entity.Game.Lottery
{
    public class PalyInfo
    {
        [Key]
        public int PalyID { get; set; }

        public string PalyCurrentNum { get; set; }

        public int LotteryID { get; set; }

        public int? PlayTypeID { get; set; }

        public string PalyNum { get; set; }

        public string UserName { get; set; }

        public int? NoteNum { get; set; }

        public decimal? SingleMoney { get; set; }

        public decimal? NoteMoney { get; set; }

        public DateTime? NoteTime { get; set; }

        public bool? IsWin { get; set; }

        public decimal? WinMoney { get; set; }

        public int? IsFactionAward { get; set; }

        public int? PlayTypeRadioID { get; set; }

        public decimal? RebatePro { get; set; }

        public string RebateProMoney { get; set; }

        public int? WinNum { get; set; }

        public int? UserID { get; set; }

        public int? NoticeID { get; set; }

        public DateTime? LotteryTime { get; set; }

        public decimal? UserRebatePro { get; set; }

        public int? Multiple { get; set; }

        public string OrderKey { get; set; }

        public decimal? CurrencyUnit { get; set; }

        public int? Ratio { get; set; }

        public string SourceType { get; set; }

        public string MemoJson { get; set; }

        public string ClientIP { get; set; }

        public string RoomId { get; set; }
    }
}