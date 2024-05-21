namespace MS.Core.MM.Models.Post.ServiceReq
{
    public class ReqPostIdUserId
    {
        /// <summary>
        /// 贴子 Id
        /// </summary>
        public string PostId { get; set; } = string.Empty;

        /// <summary>
        /// 用戶Id
        /// </summary>
        public int UserId { get; set; }
    }
}