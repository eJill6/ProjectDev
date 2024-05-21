using MS.Core.Models.Models;

namespace BackSideWeb.Model.Param.PublishRecord
{
    public class PublishRecordParam : PageParam
    {
        /// <summary>
        /// 帖子編號
        /// </summary>
        public string? PostId { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 使用者編號
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 帖子區域
        /// </summary>
        public int? PostType { get; set; }

        /// <summary>
        /// 帖子狀態
        /// </summary>
        public int? Status { get; set; }

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
        public int? UserIdentity { get; set; }

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
