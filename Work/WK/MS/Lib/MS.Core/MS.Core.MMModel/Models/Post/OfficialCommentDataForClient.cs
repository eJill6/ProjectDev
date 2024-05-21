namespace MS.Core.MMModel.Models.Post
{
    /// <summary>
    /// 官方評論資料
    /// </summary>
    public class OfficialCommentDataForClient
    {
        /// <summary>
        /// 預約單 Id
        /// </summary>
        public string BookingId { get; set; } = string.Empty;

        /// <summary>
        /// 顏值評分
        /// </summary>
        public int FacialScore { get; set; }

        /// <summary>
        /// 服務質量
        /// </summary>
        public int ServiceQuality { get; set; }

        /// <summary>
        /// 評論內容
        /// </summary>
        public string Comment { get; set; } = string.Empty;
    }
}