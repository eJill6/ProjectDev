using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminReport
{
    /// <summary>
    /// 列表搜尋參數
    /// </summary>
    public class AdminReportListParam : PageParam
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
        /// 投诉原因
        /// </summary>
        public ReportType? ReportType { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public ReviewStatus? Status { get; set; }

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
