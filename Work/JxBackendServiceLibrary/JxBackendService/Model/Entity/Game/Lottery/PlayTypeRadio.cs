using System;
using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.Entity.Game.Lottery
{
    public class PlayTypeRadio
    {
        [Key]
        public int PlayTypeRadioID { get; set; }

        public string PlayTypeRadioName { get; set; }

        public int PlayTypeID { get; set; }

        public string PlayDescription { get; set; }

        public int Priority { get; set; }

        public string WinExample { get; set; }

        public decimal MinBetMoney { get; set; }

        public int MaxBetCount { get; set; }

        public string TypeModel { get; set; }

        public int UserType { get; set; }

        public bool Status { get; set; }
    }
}