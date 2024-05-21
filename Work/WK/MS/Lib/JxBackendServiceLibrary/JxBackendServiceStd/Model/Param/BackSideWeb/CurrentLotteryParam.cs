using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Model.Param.BackSideWeb
{
    public class CurrentLotteryParam : PagedParam
    {
        public int? LotteryID { get; set; }
        public string IssueNo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? IsLottery { get; set; }
    }
}
