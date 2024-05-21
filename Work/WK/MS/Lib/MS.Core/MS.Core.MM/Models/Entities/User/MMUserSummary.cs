using MS.Core.Attributes;
using MS.Core.Models;

namespace MS.Core.MM.Models.Entities.User
{
    public class MMUserSummary : BaseDBModel
    {
        /// <summary>
        /// PK
        /// </summary>
        [AutoKey]
        public int Id { get; set; }

        /// <summary>
        /// 玩家ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 類別(廣場、担保(原為中介)、官方)
        /// </summary>
        public UserSummaryCategoryEnum Category { get; set; }

        /// <summary>
        /// 種類(發贴數、解鎖數)
        /// </summary>
        public UserSummaryTypeEnum Type { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 重置
        /// </summary>
        public DateTime? RestSetTime { get; set; }
    }
}