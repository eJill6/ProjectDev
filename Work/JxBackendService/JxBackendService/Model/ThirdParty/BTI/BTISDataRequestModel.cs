using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ThirdParty.BTI
{
    public class BTISBaseDataRequest
    {

    }

    public class BTISDataRequest : BTISBaseDataRequest
    {
        [JsonIgnore]
        public string Token { get; set; }        
    }

    public class BTISDataGetTokenRequest : BTISBaseDataRequest
    {
        public string AgentUserName { get; set; }

        public string AgentPassword { get; set; }
    }

    public class BettingHistoryRequest : BTISDataRequest
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
