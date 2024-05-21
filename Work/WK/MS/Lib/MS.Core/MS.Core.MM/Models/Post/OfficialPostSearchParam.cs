using MS.Core.MM.Models.SystemSettings;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;

namespace MS.Core.MM.Models.Post
{
    /// <summary>
    /// 贴子搜尋
    /// </summary>
    public class OfficialPostSearchParam
    {
        /// <summary>
        /// 查詢頁數
        /// </summary>
        public int? PageNo { get; set; }

        /// <summary>
        /// 查詢頁數
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// 是否為推薦
        /// </summary>
        public bool? IsRecommend { get; set; }

        /// <summary>
        /// 贴子排序類型。0：最新、1：紅榜、2：顏值
        /// </summary>
        public PostSortType? SortType { get; set; }

        /// <summary>
        /// 解鎖狀態。0：全部、1：預約過、2：未約過
        /// </summary>
        public ViewBookingStatus? BookingStatus { get; set; }

        /// <summary>
        /// 用戶身份。0：一般、1：覓經紀、2：覓女郎、3、覓老闆、4：星覓官  -- (官方贴專用)
        /// </summary>
        public IdentityType? UserIdentity { get; set; }

        /// <summary>
        /// 地區代碼
        /// </summary>
        public string? AreaCode { get; set; }

        /// <summary>
        /// 篩選的年齡
        /// </summary>
        public int[]? Age { get; set; }

        /// <summary>
        /// 篩選的年齡
        /// </summary>
        public int[]? Height { get; set; }

        /// <summary>
        /// 篩選的年齡
        /// </summary>
        public int[]? Cup { get; set; }

        /// <summary>
        /// 價格設定
        /// </summary>
        public PriceLowAndHigh[]? Price { get; set; }

        /// <summary>
        /// 篩選的服務項目
        /// </summary>
        public int[]? ServiceIds { get; set; }

        /// <summary>
        /// 查詢時的郵戳
        /// </summary>
        public string? Ts { get; set; }
    }
}