using MS.Core.Models.Models;

namespace MS.Core.MMModel.Models.My
{
    /// <summary>
    /// 我的解鎖清單頁的總體資訊
    /// </summary>
    public class MyUnlockPostListViewModelForClient : PageResultModel<MyUnlockPostListForClient>
    {
        /// <summary>
        /// 提示訊息
        /// </summary>
        public string Tip { get; set; } = string.Empty;
    }
}