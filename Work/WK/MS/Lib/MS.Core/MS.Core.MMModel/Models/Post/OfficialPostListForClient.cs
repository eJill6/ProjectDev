using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MMModel.Models.Post
{
    /// <summary>
    /// 官方贴子清單
    /// </summary>
    public class OfficialPostListForClient
    {
        /// <summary>
        ///  官方贴 Id
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 發贴類型
        /// </summary>
        public PostType PostType { get; set; }

        /// <summary>
        /// 封面照片
        /// </summary>
        public string CoverUrl { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 身高
        /// </summary>
        public string Height { get; set; }

        /// <summary>
        /// 年齡
        /// </summary>
        public string Age { get; set; }

        /// <summary>
        /// 罩杯
        /// </summary>
        public string Cup { get; set; }

        /// <summary>
        /// 地區編碼
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 最低價格
        /// </summary>
        public string LowPrice { get; set; }

        /// <summary>
        /// 顏值
        /// </summary>
        public string FacialScore { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 是否營業中。0：休息、1：營業中
        /// </summary>
        public bool? IsOpen { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        public int Views { get; set; }

        /// <summary>
        /// 帖子观看
        /// </summary>
        public int? ViewBaseCount { get; set; }
    }
}