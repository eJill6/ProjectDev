using System;

namespace JxBackendService.Model.ViewModel
{
    public class UserScoreLog
    {
        public string UserName { get; set; }
        public string TypeName { get; set; }
        public decimal MoneyIn { get; set; }
        public decimal MoneyOut { get; set; }
        public decimal AvailableScores { get; set; }
        public DateTime ChangesTime { get; set; }
        public string Handle { get; set; }
        public string Memo { get; set; }
        public decimal OldAvailableScores { get; set; }
        public decimal OldFreezeScores { get; set; }
        public decimal NewFreezeScores { get; set; }
        public decimal ChangesAMoney { get; set; }
    }
}
