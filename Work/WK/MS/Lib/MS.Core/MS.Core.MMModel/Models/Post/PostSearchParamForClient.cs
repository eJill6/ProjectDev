using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.SystemSettings;
using MS.Core.Models.Models;

namespace MS.Core.MMModel.Models.Post
{
    /// <summary>
    /// 贴子搜尋
    /// </summary>
    public class PostSearchParamForClient
    {
        /// <summary>
        /// 查詢頁數  (原page參數因為了與後台統一，因此先暫時先保留等四期刪除)
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// 查詢頁數
        /// </summary>
        public int? PageNo { get; set; }

        /// <summary>
        /// 一頁有幾筆
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// 贴子類型。1：廣場、2：寻芳阁(原為中介)、3：官方、4：體驗
        /// </summary>
        public PostType? PostType { get; set; }

        /// <summary>
        /// 是否為推薦
        /// </summary>
        public bool? IsRecommend { get; set; }

        /// <summary>
        /// 贴子排序類型。0：時間(最新)、1：熱門
        /// </summary>
        public PostSortType? SortType { get; set; }

        /// <summary>
        /// 訊息 Id
        /// </summary>
        public int? MessageId { get; set; }

        /// <summary>
        /// 解鎖狀態。0：未解鎖、1：已解鎖
        /// </summary>
        public PostLockStatus? LockStatus { get; set; }

        /// <summary>
        /// 地區代碼
        /// </summary>
        public string AreaCode { get; set; } = string.Empty;

        /// <summary>
        /// 篩選的年齡
        /// </summary>
        public int[] Age { get; set; }

        /// <summary>
        /// 篩選的年齡
        /// </summary>
        public int[] Height { get; set; }

        /// <summary>
        /// 篩選的年齡
        /// </summary>
        public int[] Cup { get; set; }

        /// <summary>
        /// 價格設定
        /// </summary>
        public PriceLowAndHighForClient[] Price { get; set; }

        /// <summary>
        /// 篩選的服務項目
        /// </summary>
        public int[] ServiceIds { get; set; }

        /// <summary>
        /// 查詢時的郵戳
        /// </summary>
        public string Ts { get; set; } = string.Empty;

        /// <summary>
        /// 已認證。0:未認證 1：已認證
        /// </summary>
        public bool? IsCertified { get; set; }
    }
}