using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public class SaveProfitlossToPlatformParam
    {
        public List<InsertTPGameProfitlossParam> TPGameProfitlosses { get; set; } 
        

        public Func<string, SaveBetLogFlags, bool> UpdateSQLiteToSavedStatus { get; set; }
        
        /// <summary>配合舊版第三方串接,由舊程式取得資料後送入</summary>
        public Dictionary<int, UserScore> UserScoreMap { get; set; }
    }
}
