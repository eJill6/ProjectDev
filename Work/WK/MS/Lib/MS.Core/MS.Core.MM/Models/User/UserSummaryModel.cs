using MS.Core.MM.Models.Entities.User;

namespace MS.Core.MM.Models.User
{
    public class UserSummaryModel
    {
        /// <summary>
        /// 玩家ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 種類(發贴數、解鎖數)
        /// </summary>
        public UserSummaryTypeEnum Type { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 分類
        /// </summary>
        public UserSummaryCategoryEnum Category { get; set; }
    }
}
