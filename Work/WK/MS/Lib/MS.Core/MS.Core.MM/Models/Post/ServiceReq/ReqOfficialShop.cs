using MS.Core.Models.Models;

namespace MS.Core.MM.Models.Post.ServiceReq
{
    public class ReqOfficialShop : PageParam
    {
        /// <summary>
        /// 搜索关键字(非必填)
        /// </summary>
        public string Keyword { get; set; } = string.Empty;

        /// <summary>
        /// 排序方式，0：（默认排序，成交订单数倒序）， 1：（评分倒序，若相同则按店龄倒序）
        /// </summary>
        public int? SortType { get; set; } = 0;

        /// <summary>
        /// 使用者編號
        /// </summary>
        public int? UserId { get; set; }
    }
}