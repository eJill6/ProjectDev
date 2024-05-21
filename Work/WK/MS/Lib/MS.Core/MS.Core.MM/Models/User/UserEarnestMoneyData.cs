using MS.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.User
{
    public class UserEarnestMoneyData
    {
        /// <summary>
        /// 调整记录ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime ExamineTime { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public string ExamineTimeText => ExamineTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 调整金额
        /// </summary>
        public decimal EarnestMoney { get; set; }
    }
}
