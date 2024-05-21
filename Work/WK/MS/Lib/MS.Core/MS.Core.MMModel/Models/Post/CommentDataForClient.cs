using System;

namespace MS.Core.MMModel.Models.Post
{
    /// <summary>
    /// 評論資料
    /// </summary>
    public class CommentDataForClient
    {
        /// <summary>
        /// 贴子 Id
        /// </summary>
        public string PostId { get; set; } = string.Empty;

        /// <summary>
        /// 消費時間
        /// </summary>
        public DateTime SpentTime { get; set; }

        /// <summary>
        /// 評論內容
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// 地區代碼
        /// </summary>
        public string AreaCode { get; set; } = string.Empty;

        /// <summary>
        /// 照片 id list
        /// </summary>
        public string[] PhotoIds { get; set; } 
    }
} 