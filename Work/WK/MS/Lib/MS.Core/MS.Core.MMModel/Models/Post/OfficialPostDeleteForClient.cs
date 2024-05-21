namespace MS.Core.MMModel.Models.Post
{
    /// <summary>
    /// 刪除官方贴
    /// </summary>
    public class OfficialPostDeleteForClient
    {
        /// <summary>
        /// 贴子id list
        /// </summary>
        public string[] PostIds { get; set; } = new string[0];
        public int IsDelete { get; set; }
    }
}