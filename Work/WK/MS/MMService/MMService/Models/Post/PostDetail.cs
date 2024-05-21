using MS.Core.MM.Models.Post;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;

namespace MMService.Models.Post
{
    /// <summary>
    /// 覓贴詳情
    /// </summary>
    public class PostDetail
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
        /// 該贴用戶身份 (二期新加)
        /// </summary>
        public IdentityType UserIdentity { get; set; }

        /// <summary>
        /// 保證金 (二期新加)
        /// </summary>
        public string EarnestMoney { get; set; } = string.Empty;

        /// <summary>
        /// 照片連結
        /// </summary>
        public string[] PhotoUrls { get; set; } = Array.Empty<string>();

        /// <summary>
        /// 視頻連結
        /// </summary>
        public string VideoUrl { get; set; } = string.Empty;

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 區域代碼
        /// </summary>
        public string AreaCode { get; set; } = string.Empty;

        /// <summary>
        /// 訊息標題 (等同列表的職業)
        /// </summary>
        public string Job { get; set; } = string.Empty;

        /// <summary>
        /// 頭像連結
        /// </summary>
        public string AvatarUrl { get; set; } = string.Empty;

        /// <summary>
        /// 發贴人當下暱稱
        /// </summary>
        public string Nickname { get; set; } = string.Empty;

        /// <summary>
        /// 卡的類型
        /// </summary>
        public int[] CardType { get; set; } = Array.Empty<int>();

        /// <summary>
        /// 註冊時間
        /// </summary>
        public string RegisterTime { get; set; } = string.Empty;

        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdateTime { get; set; } = string.Empty;

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
        /// 定價/解鎖價格
        /// </summary>
        public decimal UnlockAmount { get; set; }

        /// <summary>
        /// 特價
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// 免費解鎖次數
        /// </summary>
        public decimal FreeUnlockCount { get; set; }

        /// <summary>
        /// 是否解鎖
        /// </summary>
        public bool IsUnlock { get; set; }

        /// <summary>
        /// 解锁次数
        /// </summary>
        public int Unlocks { get; set; }

        /// <summary>
        /// 用戶解鎖可以得到的訊息
        /// </summary>
        public UserUnlockGetInfo? UnlockInfo { get; set; }

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
        /// 數量
        /// </summary>
        public string Quantity { get; set; } = string.Empty;

        /// <summary>
        /// 最低價格
        /// </summary>
        public string LowPrice { get; set; } = string.Empty;

        /// <summary>
        /// 最高價格
        /// </summary>
        public string HighPrice { get; set; } = string.Empty;

        /// <summary>
        /// 營業時間
        /// </summary>
        public string BusinessHours { get; set; } = string.Empty;

        /// <summary>
        /// 服務項目
        /// </summary>
        public string[] ServiceItem { get; set; } = Array.Empty<string>();

        /// <summary>
        /// 服務描述
        /// </summary>
        public string ServiceDescribe { get; set; } = string.Empty;

        /// <summary>
        /// 是否為精選
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// 是否已評論
        /// </summary>
        public CommentStatus PostCommentStatus { get; set; }

        /// <summary>
        /// 有評論才會傳回評論id
        /// </summary>
        public string CommentId { get; set; } = string.Empty;

        /// <summary>
        /// 評論結果
        /// </summary>
        public string CommentMemo { get; set; } = string.Empty;

        /// <summary>
        /// 已回報/已投訴
        /// </summary>
        public bool HasReported { get; set; }

        /// <summary>
        /// 能投訴
        /// </summary>
        public bool CanReported { get; set; }

        /// <summary>
        /// 总投诉次数
        /// </summary>
        public int ReportedCount { get; set; }

        /// <summary>
        /// 免費解鎖權限
        /// </summary>
        public bool HasFreeUnlockAuth { get; set; }

        /// <summary>
        /// 新客必看
        /// </summary>
        public string MustSee { get; set; } = string.Empty;

        /// <summary>
        /// 跑馬燈
        /// </summary>
        public string Marquee { get; set; } = string.Empty;

        /// <summary>
        /// 当前用户是否喜爱
        /// </summary>
        public bool IsFavorite { get; set; }
        public string PostUserId { get; set; }
    }
}