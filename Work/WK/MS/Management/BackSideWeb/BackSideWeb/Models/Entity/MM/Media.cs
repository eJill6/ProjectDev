namespace BackSideWeb.Model.Entity.MM
{
    public class Media
    {
        /// <summary>
        /// 流水編號
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// 雲倉儲檔案路徑
        /// </summary>
        public string? FileUrl { get; set; }

        /// <summary>
        /// 媒體類型 0:圖片, 1:影片
        /// </summary>
        public int MediaType { get; set; }

        /// <summary>
        /// 媒體的來源 0:Banner, 1:帖子, 2:舉報, 3: 評論
        /// </summary>
        public int SourceType { get; set; }

        /// <summary>
        /// 對應的編號
        /// </summary>
        public string? RefId { get; set; } = string.Empty;

        /// <summary>
        /// 新增時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改時間
        /// </summary>
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 完整路徑
        /// </summary>
        public string? FullMediaUrl { get; set; }
        public byte[]? Bytes { get; set; }
        public string? FileName { get; set; }
    }
}
