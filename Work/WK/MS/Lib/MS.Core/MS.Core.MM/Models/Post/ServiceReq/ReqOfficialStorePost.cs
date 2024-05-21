using MS.Core.Models.Models;

namespace MS.Core.MM.Models.Post.ServiceReq
{
    public class ReqOfficialStorePost : PageParam
    {
        /// <summary>
        /// 店铺id
        /// </summary>
        public string ApplyId { get; set; } = string.Empty;

        /// <summary>
        /// 区域代码
        /// </summary>
        public string[] AreaCode { get; set; }

        /// <summary>
        /// 發贴人 Id
        /// </summary>
        public int? UserId { get; set; }
    }
}