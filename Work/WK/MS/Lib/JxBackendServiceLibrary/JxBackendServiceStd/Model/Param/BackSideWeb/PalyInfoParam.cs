using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Model.Param.BackSideWeb
{
    public class PalyInfoParam : PagedParam
    {
        public int? LotteryID { get; set; }
        public string PalyCurrentNum { get; set; }
        public int? UserId { get; set; }
        public int? IsFactionAward { get; set; }
        public int? IsWin { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string RoomId { get; set; }
    }
}
