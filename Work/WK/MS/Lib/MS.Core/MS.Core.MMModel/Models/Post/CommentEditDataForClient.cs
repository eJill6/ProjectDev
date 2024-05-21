using System;
using System.Collections.Generic;

namespace MS.Core.MMModel.Models.Post
{
    /// <summary>
    /// 評論編輯資訊
    /// </summary>
    public class CommentEditDataForClient
    {
        /// <summary>
        /// 贴子 Id
        /// </summary>
        public string PostId { get; set; } = string.Empty;

        /// <summary>
        /// 消費時間
        /// </summary>
        public string  SpentTime { get; set; } = string.Empty;

        /// <summary>
        /// 評論內容
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// 地區代碼
        /// </summary>
        public string AreaCode { get; set; } = string.Empty;

        /// <summary>
        /// 照片對應 id/value
        /// </summary>
        public Dictionary<string, string> PhotoSource { get; set; }
    }
}