using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SportDataBase.Model
{
    [DataContract]
    public class BetSettingItem
    {
        [DataMember(Order = 1)]
        public string sport_type { get; set; }
        [DataMember(Order = 2)]
        public int min_bet { get; set; }
        [DataMember(Order = 3)]
        public int max_bet { get; set; }
        [DataMember(Order = 4)]
        public int max_bet_per_match { get; set; }
    }
}
