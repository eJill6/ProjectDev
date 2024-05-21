using MS.Core.MM.Models.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Filters
{
    public class UserSummaryFilter
    {
        public int? UserId { get; set; }
        /// <summary>
        /// 1.广场 2.担保(原為中介) 现在为寻芳阁 3.官方
        /// </summary>
        public int? Category { get; set; }

        /// <summary>
        /// 1.發贴次數,2.解鎖贴次數,3.免費解鎖次數,4.被解鎖次數,5.評論次數,6.累積收益(已領取),7.預約次數,8.被預約次數
        /// </summary>
        public int? Type { get; set; }
    }
}
