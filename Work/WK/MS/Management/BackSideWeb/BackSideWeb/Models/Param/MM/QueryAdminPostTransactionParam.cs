using JxBackendService.Model.Paging;

namespace BackSideWeb.Model.Param.MM
{
    public class QueryAdminPostTransactionParam : BasePagingRequestParam
    {

        /// <summary>
        /// 帖子編號
        /// </summary>
        public string? PostId { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// 使用者編號
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// 帖子區域
        /// </summary>
        public int? PostType { get; set; }

        /// <summary>
        /// 解锁方式(0: 一般, 1: 免費, 2:打折)
        /// </summary>
        public int? UnlockType { get; set; }

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
