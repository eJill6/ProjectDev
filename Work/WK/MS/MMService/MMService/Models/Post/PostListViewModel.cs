using MS.Core.Models.Models;

namespace MMService.Models.Post
{
    /// <summary>
    /// 贴子清單
    /// </summary>
    public class PostListViewModel : PageResultModel<PostList>
    {
        /// <summary>
        /// 分頁查詢用的時間戳
        /// </summary>
        public string Ts { get; set; } = string.Empty;
    }
}