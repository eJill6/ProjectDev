using JxBackendService.Model.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Entity.Game
{
    public class ProfitLoss
    {
        [ExplicitKey]
        [NVarcharColumnInfo(32)]
        public string ProfitLossID { get; set; }

        public int UserID { get; set; }

        public DateTime ProfitLossTime { get; set; }

        [NVarcharColumnInfo(50)]
        public string ProfitLossType { get; set; }

        public decimal ProfitLossMoney { get; set; }

        public decimal WinMoney { get; set; }

        public decimal PrizeMoney { get; set; }

        public decimal AllBetMoney { get; set; }

        [NVarcharColumnInfo(50)]
        public string GameType { get; set; }

        [VarcharColumnInfo(50)]
        public string PlayID { get; set; }

        [NVarcharColumnInfo(500)]
        public string Memo { get; set; }
    }
}