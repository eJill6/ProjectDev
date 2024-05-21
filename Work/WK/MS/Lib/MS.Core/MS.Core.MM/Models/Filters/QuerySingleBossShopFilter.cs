using MS.Core.MMModel.Models.Post.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Filters
{
    /// <summary>
    /// 查询单个MMBossShop
    /// </summary>
    public class QuerySingleBossShopFilter
    {
        /// <summary>
        /// 申请ID
        /// </summary>
        public string? ApplyId { get; set; }
        /// <summary>
        /// bossId
        /// </summary>
        public string? BossId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int? UserId { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public ReviewStatus? Status { get; set; }
    }
}
