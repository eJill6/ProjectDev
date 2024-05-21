using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminPost
{
    /// <summary>
    /// 列表搜尋參數
    /// </summary>
    public class AdminPostListParam : PageParam
    {
        /// <summary>
        /// 贴子編號
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 使用者編號
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 贴子區域
        /// </summary>
        public PostType? PostType { get; set; }

        /// <summary>
        /// 贴子狀態
        /// </summary>
        public ReviewStatus? Status { get; set; }
        public int? UserIdentity { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public int SortType { get; set; }

        /// <summary>
        /// 0: 首次送审时间
        /// 1: 再次送审时间
        /// 2: 审核时间
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
