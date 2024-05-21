namespace MS.Core.MM.Models.Post.ServiceReq
{
    public class ReqPostIdPageNo
    {
        /// <summary>
        /// 贴子id
        /// </summary>
        public string PostId { get; set; } = string.Empty;

        /// <summary>
        /// 查詢分頁
        /// </summary>
        public int PageNo { get; set; }
    }
}