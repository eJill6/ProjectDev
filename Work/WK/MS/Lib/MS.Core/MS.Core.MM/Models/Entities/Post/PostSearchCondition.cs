using Amazon.S3.Model;
using MS.Core.MM.Models.SystemSettings;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;

namespace MS.Core.MM.Models.Entities.Post
{
    public class PostSearchCondition
    {
        /// <summary>
        /// 贴子類型。廣場、担保(原為中介)、官方、體驗 -- (廣場贴、中介專用)
        /// </summary>
        public PostType? PostType { get; set; }

        /// <summary>
        /// 用戶Id
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 是否為推薦
        /// </summary>
        public bool? IsRecommend { get; set; }

        /// <summary>
        /// 排序類型
        /// </summary>
        public PostSortType? SortType { get; set; }

        /// <summary>
        /// 信息類型 (廣場贴、中介專用)
        /// </summary>
        public int? MessageId { get; set; }

        /// <summary>
        /// 解鎖狀態 (廣場贴、中介專用)
        /// </summary>
        public PostLockStatus? LockStatus { get; set; }

        /// <summary>
        /// 區域代碼
        /// </summary>
        public string? AreaCode { get; set; }

        /// <summary>
        /// 服務項目
        /// </summary>
        public int[]? ServiceIds { get; set; }

        /// <summary>
        /// 年齡
        /// </summary>
        public int[]? Age { get; set; }

        /// <summary>
        /// 身高
        /// </summary>
        public int[]? Height { get; set; }

        /// <summary>
        /// 罩杯
        /// </summary>
        public int[]? Cup { get; set; }

        /// <summary>
        /// 價格
        /// </summary>
        public PriceLowAndHigh[]? Price { get; set; }

        /// <summary>
        /// 解鎖狀態。0：全部、1：預約過、2：未約過 -- (官方贴專用)
        /// </summary>
        public ViewBookingStatus? BookingStatus { get; set; }

        /// <summary>
        /// 用戶身份。0：一般、1：覓經紀、2：覓女郎、3、覓老闆、4：星覓官  -- (官方贴專用)
        /// </summary>
        public IdentityType? UserIdentity { get; set; }

        /// <summary>
        /// 查詢時間戳
        /// </summary>
        public DateTime? QueryTs { get; set; }

        /// <summary>
        /// 搜尋頁數
        /// </summary>
        public int PageNo { get; set; } = 1;

        /// <summary>
        /// 搜尋頁數
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// 已認證。0:未認證 1：已認證
        /// </summary>
        public bool? IsCertified { get; set; }
    }
}