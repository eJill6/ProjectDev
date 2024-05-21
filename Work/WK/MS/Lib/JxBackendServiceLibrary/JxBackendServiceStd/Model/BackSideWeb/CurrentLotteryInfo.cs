using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.BackSideWeb
{
    public class CurrentLotteryInfo
    {
        /// <summary>
        /// Id
        /// </summary>       
        public string CurrentLotteryID { get; set; }
        /// <summary>
        /// 期号
        /// </summary>       
        public string IssueNo { get; set; }
        /// <summary>
        /// 彩种
        /// </summary>
        public int? LotteryID { get; set; }
        /// <summary>
        /// 開獎類型
        /// </summary>
        public string LotteryType { get; set; }
        /// <summary>
        /// 開獎號碼
        /// </summary>
        public string CurrentLotteryNum { get; set; }
        /// <summary>
        /// 封单时间
        /// </summary>
        public DateTime? CurrentLotteryTime { get; set; }
        /// <summary>
        /// 开奖时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 开奖状态
        /// </summary>
        public int? IsLottery { get; set; }
    }
}
