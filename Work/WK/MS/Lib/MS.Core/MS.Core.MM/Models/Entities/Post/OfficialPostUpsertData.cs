namespace MS.Core.MM.Models.Entities.Post
{
    public class OfficialPostUpsertData : MMOfficialPost
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
        /// 套餐json
        /// </summary>
        public string Combo { get; set; } = string.Empty;
    }
}