using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgDataBase.Model
{
   public class Game_Logs
    {
       public int ID { get; set; }
       public int GameType { get; set; }
       public decimal OldAvailableScores { get; set; }
       public decimal NewAvailableScores { get; set; }
       public decimal NewFreezeScores { get; set; }
       public decimal ChangesAMoney { get; set; }
       public decimal ChangesFMoney { get; set; }
       public DateTime ChangesTime { get; set; }
       public int UserID { get; set; }
       public string Handle { get; set; }
       public string Memo { get; set; }
       public int RefID { get; set; }
       public string TypeName { get; set; }
       public string UserName { get; set; }
    }
}
