using MS.Core.MMModel.Models.AdminReport;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;

namespace BackSideWeb.Model.Param.PublishRecord
{
    public class MMPostReportParam : PageParam
    {
        /// <summary>
        /// 投诉单ID
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// 帖子ID
        /// </summary>
        public string? PostId { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 帖子區域
        /// </summary>
        public int? PostType { get; set; }

        /// <summary>
        /// 投诉原因
        /// </summary>
        public int? ReportType { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public int? Status { get; set; }

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