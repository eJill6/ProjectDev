using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MMModel.Models.Post
{
    /// <summary>
    /// 舉報資料
    /// </summary>
    public class ReportDataForClient
    {
        /// <summary>
        /// 舉報類型 0：騙子、1：廣告騷擾、2：貨不對版、3：無效聯絡方式
        /// </summary>
        public ReportType ReportType { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; } = string.Empty;

        /// <summary>
        /// 被檢舉的贴子 Id
        /// </summary>
        public string PostId { get; set; } = string.Empty;

        /// <summary>
        /// 證據圖片
        /// </summary>
        public string[] PhotoIds { get; set; }
    }
}