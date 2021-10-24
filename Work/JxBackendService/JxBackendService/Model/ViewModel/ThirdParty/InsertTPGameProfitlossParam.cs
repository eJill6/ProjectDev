using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public class InsertTPGameProfitlossParam
    {
        public int UserID { get; set; }
        public DateTime ProfitLossTime { get; set; }
        public string ProfitLossType { get; set; }
        public decimal ProfitLossMoney { get; set; }
        public decimal WinMoney { get; set; }
        public decimal PrizeMoney { get; set; }
        public int IsWin { get; set; }
        public string Memo { get; set; }
        public string PalyID { get; set; }
        public string GameType { get; set; }
        public string RefID { get; set; }
        public DateTime BetTime { get; set; }
        public decimal AllBetMoney { get; set; }

        /// <summary>
        /// 紀錄SQLite轉過來的KEY, 以便之後更新狀態回SQLite
        /// </summary>
        public string KeyId { get; set; }

        /// <summary>直屬抽水金額</summary>
        public decimal HighestParentRebateMoney { get; set; }

        /// <summary>上上級抽水金額</summary>
        public decimal GrandParentRebateMoney { get; set; }

        /// <summary>上級抽水金額</summary>
        public decimal ParentRebateMoney { get; set; }

        /// <summary>自己返水金額</summary>
        public decimal SelfRebateMoney { get; set; }

        /// <summary>第三方可用積分</summary>
        public decimal? AvailableScores { get; set; }

        /// <summary>第三方凍結積分</summary>
        public decimal? FreezeScores { get; set; }

        /// <summary>是否忽略此筆資料</summary>
        public bool IsIgnore { get; set; }
    }
}
