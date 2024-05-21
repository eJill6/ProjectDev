using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.Post;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using System;
using System.Collections.Generic;

namespace MS.Core.MMModel.Models.AdminPost
{
    public class AdminPostDetail
    {
        /// <summary>
        /// 贴 Id
        /// </summary>
        public string PostId { get; set; } = string.Empty;

        /// <summary>
        /// 贴子類型。1：廣場、2：寻芳阁(原為中介)、3：官方、4：體驗
        /// </summary>
        public PostType PostType { get; set; }

        /// <summary>
        /// 發贴當下頭像連結
        /// </summary>
        public string CoverUrl { get; set; } = string.Empty;

        /// <summary>
        /// 發贴人 Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 發贴人當下暱稱
        /// </summary>
        public string Nickname { get; set; } = string.Empty;

        /// <summary>
        /// 狀態。0：審核中、1：核準 (即上架)、2：未通過
        /// </summary>
        public ReviewStatus Status { get; set; }

        /// <summary>
        /// 預約次數
        /// </summary>
        public int AppointmentCount { get; set; }

        /// <summary>
        /// 信息類型
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// 解鎖價格
        /// </summary>
        public decimal UnlockAmount { get; set; }

        /// <summary>
        /// 会员申请调价
        /// </summary>
        public decimal ApplyAmount { get; set; }

        /// <summary>
        /// 会员申请调价
        /// </summary>
        public string ApplyAmountText => ApplyAmount.ToString(GlobalSettings.AmountFormat);

        /// <summary>
        /// 申請調價
        /// </summary>
        public bool ApplyAdjustPrice { get; set; }

        /// <summary>
        /// 信息標題
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 地區代碼
        /// </summary>
        public string AreaCode { get; set; } = string.Empty;

        /// <summary>
        /// 數量
        /// </summary>
        public string Quantity { get; set; } = string.Empty;

        /// <summary>
        /// 年齡(歲)
        /// </summary>
        public AgeDefined Age { get; set; }

        /// <summary>
        /// 身高(cm)
        /// </summary>
        public BodyHeightDefined Height { get; set; }

        /// <summary>
        /// 罩杯
        /// </summary>
        public CupDefined Cup { get; set; }

        /// <summary>
        /// 年齡(歲)
        /// </summary>
        public string AgeText => Age.GetDescription();

        /// <summary>
        /// 身高描述
        /// </summary>
        public string HeightText => Height.GetDescription();

        /// <summary>
        /// 罩杯描述
        /// </summary>
        public string CupText => Cup.GetDescription();

        /// <summary>
        /// 營業時間
        /// </summary>
        public string BusinessHours { get; set; } = string.Empty;

        /// <summary>
        /// 最低價格
        /// </summary>
        public string LowPrice { get; set; } = string.Empty;

        /// <summary>
        /// 最高價格
        /// </summary>
        public string HighPrice { get; set; } = string.Empty;

        /// <summary>
        /// 詳細地址
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 服務描述
        /// </summary>
        public string ServiceDescribe { get; set; } = string.Empty;

        /// <summary>
        /// 收藏數
        /// </summary>
        public int Favorites { get; set; }

        /// <summary>
        /// 評論數
        /// </summary>
        public long Comments { get; set; }

        /// <summary>
        /// 觀看數
        /// </summary>
        public long Views { get; set; }

        /// <summary>
        /// 解鎖次數
        /// </summary>
        public int UnlockCount { get; set; }

        /// <summary>
        /// 熱度
        /// </summary>
        public int? Heat { get; set; }

        /// <summary>
        /// 是否精選
        /// </summary>
        public bool? IsFeatured { get; set; }

        /// <summary>
        /// 建贴時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 審核人
        /// </summary>
        public string ExamineMan { get; set; }

        /// <summary>
        /// 審核時間
        /// </summary>
        public DateTime? ExamineTime { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Memo { get; set; } = string.Empty;

        /// <summary>
        /// 会员卡
        /// </summary>
        public string CardName { get; set; }

        /// <summary>
        /// 会员卡购买时间
        /// </summary>
        public string CardCreateTime { get; set; }

        /// <summary>
        /// 会员卡到期时间
        /// </summary>
        public string CardEffectiveTime { get; set; }

        /// <summary>
        /// 贴子区域
        /// </summary>
        public string PostTypeText => PostType.GetDescription();

        /// <summary>
        /// 信息类型
        /// </summary>
        public string MessageType { get; set; } = string.Empty;

        /// <summary>
        /// 用戶解鎖可以得到的訊息
        /// </summary>
        public UserUnlockGetInfoForClient UnlockInfo { get; set; }

        /// <summary>
        /// 服務項目
        /// </summary>
        public string[] ServiceItem { get; set; } = Array.Empty<string>();

        /// <summary>
        /// 註冊時間
        /// </summary>
        public string RegisterTime { get; set; } = string.Empty;

        /// <summary>
        /// 照片連結
        /// </summary>
        public string[] PhotoUrls { get; set; } = Array.Empty<string>();

        /// <summary>
        /// 視頻連結
        /// </summary>

        public string VideoUrl { get; set; } = string.Empty;

        /// <summary>
        /// 预设解锁价格
        /// </summary>
        public decimal DefaultPricing { get; set; }

        /// <summary>
        /// 预设解锁价格
        /// </summary>
        public string DefaultPricingText => DefaultPricing.ToString(GlobalSettings.AmountFormat);

        /// <summary>
        /// 解鎖價格
        /// </summary>
        public string UnlockAmountText => UnlockAmount.ToString(GlobalSettings.AmountFormat);

        /// <summary>
        /// 首次送审时间
        /// </summary>
        public string CreateTimeText => CreateTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 再次送审时间
        /// </summary>
        public string UpdateTimeText => UpdateTime?.ToString(GlobalSettings.DateTimeFormat) ?? "-";

        /// <summary>
        /// 審核時間
        /// </summary>
        public string ExamineTimeText => ExamineTime?.ToString(GlobalSettings.DateTimeFormat) ?? "-";

        /// <summary>
        /// 会员身份
        /// </summary>
        public string UserType { get; set; } = "-";

        /// <summary>
        /// 贴子狀態
        /// </summary>
        public string StatusText => Status.GetDescription();

        /// <summary>
        /// 权重
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 首页贴
        /// </summary>
        public bool IsHomePost { get; set; }

        /// <summary>
        /// 照片來源
        /// </summary>
        public Dictionary<string, string> PhotoSource { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 視頻來源
        /// </summary>
        public Dictionary<string, string> VideoSource { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 解锁基础值
        /// </summary>
        public int? UnlockBaseCount { get; set; }

        /// <summary>
        /// 观看基础值
        /// </summary>
        public int? ViewBaseCount { get; set; }
        /// <summary>
        /// 已认证
        /// </summary>
        public bool IsCertified { get;set; }
    }
}