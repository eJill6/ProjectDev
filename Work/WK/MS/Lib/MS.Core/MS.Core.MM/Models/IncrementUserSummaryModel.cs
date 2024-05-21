using MS.Core.MM.Models.Entities.User;
using System.ComponentModel;

namespace MS.Core.MM.Models
{
    public class IncrementUserSummaryModel
    {
        /// <summary>
        /// 玩家ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 類型
        /// </summary>
        public UserSummaryCategoryEnum Category { get; set; }
        /// <summary>
        /// 種類(發贴數、解鎖數)
        /// </summary>
        public UserSummaryTypeEnum Type { get; set; }

        public decimal Amount { get; set; }
    }
}