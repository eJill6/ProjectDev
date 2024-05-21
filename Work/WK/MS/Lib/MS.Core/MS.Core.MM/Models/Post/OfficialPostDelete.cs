namespace MS.Core.MM.Models.Post
{
    /// <summary>
    /// 刪除官方贴
    /// </summary>
    public class OfficialPostDelete
    {
        /// <summary>
        /// 贴子id list
        /// </summary>
        public string[] PostIds { get; set; } = new string[0];

        /// <summary>
        /// 会员ID(Admin删除帖子需要用到)
        /// </summary>
        public int UserId { get; set; }

        public int IsDelete { get; set; }
    }
}