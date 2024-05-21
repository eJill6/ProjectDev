using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLPolyGame.Web.MSSeal.Models
{
    /// <summary>
    /// 近期開獎時間
    /// </summary>
    public class NearOpenModel
    {
        /// <summary>
        /// 期號
        /// </summary>
        public string PeriodNumber { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 遊戲編號
        /// </summary>
        public int GameId { get; set; }
    }
}