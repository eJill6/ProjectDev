using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminComment
{
    /// <summary>
    /// 列表搜尋參數
    /// </summary>
    public class AdminCommentListParam : PageParam
    {
        /// <summary>
        /// 投诉单ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 贴子ID
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 贴子區域
        /// </summary>
        public PostType? PostType { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public ReviewStatus? Status { get; set; }

        /// <summary>
        /// 1. 首次送审时间、
        /// 2. 再次送审时间、
        /// 3. 审核时间、
        /// 4. 评价时间
        /// </summary>
        public int DateTimeType { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime EndDate { get; set; }

    }
}
