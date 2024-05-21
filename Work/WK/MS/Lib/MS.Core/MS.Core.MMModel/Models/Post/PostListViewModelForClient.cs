using MS.Core.Models.Models;

namespace MS.Core.MMModel.Models.Post
{
    /// <summary>
    /// 贴子清單
    /// </summary>
    public class PostListViewModelForClient : PageResultModel<PostListForClient>
    {
        /// <summary>
        /// 分頁查詢用的時間戳
        /// </summary>
        public string Ts { get; set; } = string.Empty;
        /// <summary>
        /// 帖子下一页的图片集合
        /// </summary>
        public NextPagePostCoverViewForClient[] NextPagePost { get; set; }
    }
}