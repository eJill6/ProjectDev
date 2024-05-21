namespace MS.Core.MM.Models.Post.ServiceReq
{
    /// <summary>
    /// 官方評論資料
    /// </summary>
    public class ReqOfficialCommentData : OfficialCommentData
    {
        /// <summary>
        /// 用戶 Id
        /// </summary>
        public int UserId;

        /// <summary>
        /// 暱稱
        /// </summary>
        public string? Nickname { get; set; }

        /// <summary>
        /// 評論Id
        /// </summary>
        public string? CommentId { get; set; } = string.Empty;
    }
}