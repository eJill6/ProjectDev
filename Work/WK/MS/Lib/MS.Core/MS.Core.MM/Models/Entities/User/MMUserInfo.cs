using MS.Core.Attributes;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.Models;

namespace MS.Core.MM.Models.Entities.User
{
    public class BriefUserInfo : BaseDBModel
    {
        /// <summary>
        /// 用戶 Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 暱稱
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 會員頭像
        /// </summary>
        public string AvatarUrl { get; set; }
    }

    /// <summary>
    /// MMUserInfo Table
    /// </summary>
    public class MMUserInfo : BaseDBModel, IUserPoint
    {
        /// <summary>
        /// 用戶 Id
        /// </summary>
        [PrimaryKey]
        public int UserId { get; set; }

        /// <summary>
        /// 用戶身份
        /// </summary>
        public byte UserIdentity { get; set; }

        /// <summary>
        /// 用戶等級
        /// </summary>
        public byte UserLevel { get; set; }

        /// <summary>
        /// 積分
        /// </summary>
        public int RewardsPoint { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 註冊時間
        /// </summary>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 暱稱
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 會員頭像
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// 保證金
        /// </summary>
        public decimal EarnestMoney { get; set; }

        /// <summary>
        /// 額外的贴子發佈次數
        /// </summary>
        public int ExtraPostCount { get; set; }

        /// <summary>
        /// 備註欄
        /// </summary>
        public string? Memo { get; set; }

        /// <summary>
        /// 聯絡软件
        /// </summary>
        public string? ContactApp { get; set; }

        /// <summary>
        /// 聯絡方式
        /// </summary>
        public string? Contact { get; set; }

        /// <summary>
        /// 是否營業中
        /// </summary>
        public bool IsOpen { get; set; }
    }
}