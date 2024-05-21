namespace MS.Core.MM.Models.Post.ServiceReq
{
    public class ReqCommentIdUserId
    {
        /// <summary>
        /// 評論id
        /// </summary>
        public string CommentId { get; set; } = string.Empty;

        /// <summary>
        /// 用戶id
        /// </summary>
        public int UserId { get; set; }
    }
}