namespace MS.Core.MM.Models.Entities.Post
{
    public class CommentUpsertData : MMPostComment
    {
        /// <summary>
        /// 圖檔 list
        /// </summary>
        public string PhotoIds { get; set; } = string.Empty;
    }
}