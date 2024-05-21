using MMService.DBTools;
using MS.Core.Attributes;
using MS.Core.Models;
using System.Data;

namespace MS.Core.MM.Models.Entities.User
{
    public class MMEarnestMoneyHistory : BaseDBModel
    {
        /// <summary>
        /// Id
        /// </summary>
        [AutoKey]
        public int Id { get; set; }

        /// <summary>
        /// 会员Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 保证金
        /// </summary>
        public decimal EarnestMoney { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public string? ExamineMan { get; set; }

        /// <summary>
        /// 审核時間
        /// </summary>
        public DateTime ExamineTime { get; set; }
    }
}