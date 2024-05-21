using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;

namespace MS.Core.MMModel.Models.Post
{
    /// <summary>
    /// 覓贴詳情
    /// </summary>
    public class OfficialPostDetailForClient
    {
        /// <summary>
        /// 贴子 Id
        /// </summary>
        public string PostId { get; set; } = string.Empty;

        /// <summary>
        /// 發贴類型
        /// </summary>
        public PostType PostType { get; set; }

		/// <summary>
		/// 当前帖子状态
		/// </summary>
		public ReviewStatus PostStatus { get; set; }

		/// <summary>
		/// 是否开启编辑
		/// </summary>
		public bool LockStatus { get; set; }

		/// <summary>
		/// 該贴用戶身份
		/// </summary>
		public IdentityType UserIdentity { get; set; }

        /// <summary>
        /// 照片連結
        /// </summary>
        public string[] PhotoUrls { get; set; } = new string[0];

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
        /// 頭像連結
        /// </summary>
        public string AvatarUrl { get; set; } = string.Empty;

        /// <summary>
        /// 暱稱
        /// </summary>
        public string Nickname { get; set; } = string.Empty;

        /// <summary>
        /// 顏值
        /// </summary>
        public string FacialScore { get; set; } = string.Empty;

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 卡的類型
        /// </summary>
        public int[] CardType { get; set; } = new int[0];

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
        public string[] ServiceItem { get; set; } = new string[0];

        /// <summary>
        /// 服務描述
        /// </summary>
        public string ServiceDescribe { get; set; } = string.Empty;

        /// <summary>
        /// 官方贴投訴狀態
        /// </summary>
        public ViewOfficialReportStatus ReportStatus { get; set; }

        /// <summary>
        /// 总投诉次数
        /// </summary>
        public int ReportedCount { get; set; }

        /// <summary>
        /// 評論人數
        /// </summary>
        public string Comments { get; set; } = string.Empty;

        /// <summary>
        /// 平均顏值
        /// </summary>
        public string AvgFacialScore { get; set; } = string.Empty;

        /// <summary>
        /// 平均服務質量
        /// </summary>
        public string AvgServiceQuality { get; set; } = string.Empty;

        /// <summary>
        /// 預約次數 (已進入待評價狀態)
        /// </summary>
        public string AppointmentCount { get; set; } = string.Empty;

        /// <summary>
        /// 有未完成的預約
        /// </summary>
        public bool HaveUnfinishedBooking { get; set; }

        /// <summary>
        /// 发帖人用户 Id
        /// </summary>
        public string PostUserId { get; set; }

        public string ShopName { get; set; }
    }
}