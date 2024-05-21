using MS.Core.MMModel.Models.Post.Enums;

namespace MMService.Models.Post
{
    /// <summary>
    /// 贴子清單
    /// </summary>
    public class PostList
    {
        /// <summary>
        /// 贴子 Id
        /// </summary>
        public string PostId { get; set; } = string.Empty;

        /// <summary>
        /// 發贴類型 (二期新加)
        /// </summary>
        public PostType PostType { get; set; }

        /// <summary>
        /// 封面照片
        /// </summary>
        public string CoverUrl { get; set; } = string.Empty;

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 身高
        /// </summary>
        public string Height { get; set; } = string.Empty;

        /// <summary>
        /// 年齡
        /// </summary>
        public string Age { get; set; } = string.Empty;

        /// <summary>
        /// 罩杯
        /// </summary>
        public string Cup { get; set; } = string.Empty;

        /// <summary>
        /// 服務項目
        /// </summary>
        public string[] ServiceItem { get; set; } = Array.Empty<string>();

        /// <summary>
        /// 職業
        /// </summary>
        public string Job { get; set; } = string.Empty;

        /// <summary>
        /// 地區編碼
        /// </summary>
        public string AreaCode { get; set; } = string.Empty;

        /// <summary>
        /// 是否為精選
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// 最低價格
        /// </summary>
        public string LowPrice { get; set; } = string.Empty;

        /// <summary>
        /// 收藏數
        /// </summary>
        public string Favorites { get; set; } = string.Empty;

        /// <summary>
        /// 評論數
        /// </summary>
        public string Comments { get; set; } = string.Empty;

        /// <summary>
        /// 觀看數
        /// </summary>
        public string Views { get; set; } = string.Empty;

        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdateTime { get; set; } = string.Empty;

        /// <summary>
        /// 解锁次数 ViewBaseCount+UnlockCount
        /// </summary>
        public int Unlocks { get; set; }

        /// <summary>
        /// 当前用户是否收藏
        /// </summary>
        public bool IsFavorite { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public int MessageId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}