using MS.Core.Models.Models;

namespace MS.Core.MM.Models.Post
{
    /// <summary>
    /// 官方贴清單
    /// </summary>
    public class OfficialPostListViewModel : PageResultModel<OfficialPostList>
    {
        /// <summary>
        /// 分頁查詢用的時間戳
        /// </summary>
        public string Ts { get; set; } = string.Empty;
    }
}