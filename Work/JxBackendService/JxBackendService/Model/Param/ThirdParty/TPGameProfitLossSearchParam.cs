using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Param.ThirdParty
{
    public class TPGameProfitLossSearchParam
    {
        public int UserId { get; set; }
        public DateTime DtFrom { get; set; }
        public DateTime DtEnd { get; set; }
    }

    public class TPGameTeamProfitLossSearchParam : TPGameProfitLossSearchParam
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int SortType { get; set; }
    }
}
