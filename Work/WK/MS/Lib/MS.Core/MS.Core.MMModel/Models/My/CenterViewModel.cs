using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.My
{
    /// <summary>
    /// 個人中心資訊
    /// </summary>
    public class CenterViewModel
    {
        public int UserId { get; set; }

        /// <summary>
        /// 預約次數
        /// </summary>
        public int BookingCount { get; set; }

        /// <summary>
        /// 解鎖數量
        /// </summary>
        public int UnlockCount { get; set; }

        /// <summary>
        /// 上架數量
        /// </summary>
        public int PutOnShelvesCount { get; set; }

        /// <summary>
        /// 收益
        /// </summary>
        public decimal Income { get; set; }

        /// <summary>
        /// 鑽石數
        /// </summary>
        public decimal Point { get; set; }

        /// <summary>
        /// 有效的會員卡
        /// </summary>
        public ResUserEfficientVipViewModel[] Vips { get; set; }

        /// <summary>
        /// 會員各種數量
        /// </summary>
        public UserSummaryViewModel Quantity { get; set; }

        /// <summary>
        /// 積分
        /// </summary>
        public int RewardsPoint { get; set; }

        /// <summary>
        /// 餘額
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 註冊時間
        /// </summary>
        public DateTime RegisterTime { get; set; }

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
        public string ContactType { get; set; }

        /// <summary>
        /// 软件号码
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 用户消息未读数
        /// </summary>
        public UserUnreadMessageViewModel UnreadMessage { get; set; }
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
        /// 收藏数
        /// </summary>
        public UserFavoriteStatisticsViewModel[] UserFavorites { get; set; }
        /// <summary>
        /// 用户是否申请过觅老板
        /// </summary>
        public bool IsApplyBoss { get; set; }
    }
}
