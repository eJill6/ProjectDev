using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MMModel.Models.Post
{
    /// <summary>
    /// 贴子清單
    /// </summary>
    public class PostListForClient
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
        /// 服務項目
        /// </summary>
        public string[] ServiceItem { get; set; }

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

        public int Weight { get; set; }

        /// <summary>
        /// 已認證。0:未認證 1：已認證
        /// </summary>
        public bool IsCertified { get; set; }
    }
}