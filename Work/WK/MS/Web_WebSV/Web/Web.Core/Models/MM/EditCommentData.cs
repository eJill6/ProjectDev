using MS.Core.MMModel.Models.Post;

namespace Web.Core.Models.MM
{
    /// <summary>
    /// 編輯評論
    /// </summary>
    public class EditCommentData : CommentDataForClient
    {
        /// <summary>
        /// 評論id
        /// </summary>
        public string CommentId { get; set; }
    }
}