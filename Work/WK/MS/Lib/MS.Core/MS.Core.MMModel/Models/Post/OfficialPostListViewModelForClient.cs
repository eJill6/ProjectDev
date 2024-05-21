using MS.Core.Models.Models;

namespace MS.Core.MMModel.Models.Post
{
    /// <summary>
    /// 官方贴清單
    /// </summary>
    public class OfficialPostListViewModelForClient : PageResultModel<OfficialPostListForClient>
    {
        /// <summary>
        /// 分頁查詢用的時間戳
        /// </summary>
        public string Ts { get; set; }
    }
}