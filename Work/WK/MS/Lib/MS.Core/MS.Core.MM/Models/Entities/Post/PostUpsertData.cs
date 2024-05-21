namespace MS.Core.MM.Models.Entities.Post
{
    public class PostUpsertData : MMPost
    {
        /// <summary>
        /// 服務種類 Id
        /// </summary>
        public string ServiceIds { get; set; } = string.Empty;

        /// <summary>
        /// 圖檔 list
        /// </summary>
        public string PhotoIds { get; set; } = string.Empty;

        /// <summary>
        /// 視頻list
        /// </summary>
        public string? VideoIds { get; set; } = string.Empty;

        /// <summary>
        /// 聯繫方式json
        /// </summary>
        public string ContactInfos { get; set; } = string.Empty;

        /// <summary>
        /// 会员每日第一次发帖
        /// </summary>
        public bool IsVipPostDay { get; set; }
    }
}