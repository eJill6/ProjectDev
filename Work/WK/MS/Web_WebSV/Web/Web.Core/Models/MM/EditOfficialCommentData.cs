using MS.Core.MMModel.Models.Post;

namespace Web.Core.Models.MM
{
    /// <summary>
    /// 編輯官方評論
    /// </summary>
    public class EditOfficialCommentData : OfficialCommentDataForClient
    {
        /// <summary>
        /// 評論id
        /// </summary>
        public string CommentId { get; set; }
    }
}
