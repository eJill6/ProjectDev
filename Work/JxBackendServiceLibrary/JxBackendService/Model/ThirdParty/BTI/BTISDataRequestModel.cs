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
        public string From { get; set; }

        public string To { get; set; }

        public BTISPagination Pagination { get; set; }
    }

    public class BTISPagination
    {
        /// <summary>page起始值為0</summary>
        public int Page { get; set; }

        /// <summary>使用pagination的情況下每次最高只能撈取1000筆資料</summary>
        public int RowPerPage => 1000;
    }
}