using MS.Core.MM.Models.Post;

namespace MMService.Models.Post
{
    public class UnlockPostResModel
    {
        /// <summary>
        /// 用戶解鎖可以得到的訊息
        /// </summary>
        public UserUnlockGetInfo? UnlockInfo { get; set; }
    }
}