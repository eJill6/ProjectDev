using MS.Core.MM.Models.Entities.MessageUserRead;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.Vip;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;

namespace MMService.Models.My
{
    /// <summary>
    /// 個人中心資訊
    /// </summary>
    public class CenterInfo
    {
        public int UserId { get; set; }

        /// <summary>
        /// 預約次數
        /// </summary>
        public int BookingCount { get; set; }

        /// <summary>
        /// 上架數量
        /// </summary>
        public int PutOnShelvesCount { get; set; }

        /// <summary>
        /// 收益
        /// </summary>
        public string Income { get; set; }

        /// <summary>
        /// 鑽石數
        /// </summary>
        public string Point { get; set; }

        /// <summary>
        /// 有效的會員卡
        /// </summary>
        public ResUserVip[] Vips { get; set; } = null!;

        /// <summary>
        /// 會員各種數量
        /// </summary>
        public UserSummaryInfo Quantity { get; set; } = null!;

        /// <summary>
        /// 積分
        /// </summary>
        public int RewardsPoint { get; set; }

        /// <summary>
        /// 餘額
        /// </summary>
        public string Amount { get; set; } = null!;

        /// <summary>
        /// 註冊時間
        /// </summary>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 解鎖數量
        /// </summary>
        public int UnlockCount { get; set; }

        /// <summary>
        /// 身分
        /// </summary>
        public IdentityType Identity { get; set; }

        /// <summary>
        /// 是否有綁定手機
        /// </summary>
        public bool HasPhone { get; set; }

		/// <summary>
		/// 联系软件
		/// </summary>
		public ContactType ContactType { get; set; }

		/// <summary>
		/// 软件号码
		/// </summary>
		public string Contact { get; set; }

		/// <summary>
		/// 头像
		/// </summary>
		public string Avatar { get; set; }
        /// <summary>
        /// 用户消息未读数量
        /// </summary>
        
        public UserUnreadMessage UnreadMessage { get; set; }
        /// <summary>
        /// 广场收藏帖子
        /// </summary>
        public int CollectSquareCount { get; set; } = 0;
        /// <summary>
        /// 寻芳阁收藏帖子
        /// </summary>
        public int CollectXfgCount { get; set; } = 0;
        /// <summary>
        /// 店铺收藏记录
        /// </summary>
        public int CollectShopCount { get; set; } = 0;
        /// <summary>
        /// 用户收藏数
        /// </summary>
        public UserFavoriteStatistics[] UserFavorites { get; set; }
    }
}