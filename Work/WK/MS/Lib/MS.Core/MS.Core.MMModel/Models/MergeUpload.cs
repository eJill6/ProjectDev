using MS.Core.MMModel.Models.Media.Enums;
using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MMModel.Models
{
    /// <summary>
    /// 分批上傳後合併使用的Model
    /// </summary>
    public class MergeUpload
    {
        /// <summary>
        /// 媒體類型 0:圖片, 1:影片
        /// </summary>
        public MediaType MediaType { get; set; }

        /// <summary>
        /// 媒體的來源 0:Banner, 1:贴子, 2:舉報, 3: 評論
        /// </summary>
        public SourceType SourceType { get; set; }

        /// <summary>
        /// 所有分割的路徑
        /// </summary>
        public string[] Paths { get; set; }

        /// <summary>
        /// 副檔名
        /// </summary>
        public string Suffix { get; set; }
    }
}
