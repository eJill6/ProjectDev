using MS.Core.MMModel.Models.Post;

namespace Web.Core.Models.MM
{
    /// <summary>
    /// 編輯帖子
    /// </summary>
    public class EditPostData : PostDataForClient
    {
        /// <summary>
        /// 帖子id
        /// </summary>
        public string PostId { get; set; }
    }
}