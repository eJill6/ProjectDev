using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models.Models;
using System;
using System.Collections.Generic;

namespace MS.Core.MMModel.Models.AdminPost
{
    /// <summary>
    /// 官方覓贴詳情
    /// </summary>
    public class AdminOfficialPostDetail
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
        /// 該贴用戶身份
        /// </summary>
        public IdentityType UserIdentity { get; set; }

        /// <summary>
        /// 該贴用戶身份
        /// </summary>
        public string UserIdentityText => UserIdentity.GetDescription();

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
        public int[] CardType { get; set; } = Array.Empty<int>();

        /// <summary>
        /// 会员身份
        /// </summary>
        public string CardName { get; set; }

        /// <summary>
        /// 会员卡到期时间
        /// </summary>
        public DateTime? CardEffectiveTime { get; set; }

        /// <summary>
        /// 年齡(歲)
        /// </summary>
        public string Age { get; set; }

        /// <summary>
        /// 身高(cm)
        /// </summary>
        public string Height { get; set; }

        /// <summary>
        /// 罩杯
        /// </summary>
        public string Cup { get; set; }

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
        /// 官方贴投訴狀態
        /// </summary>
        public ViewOfficialReportStatus ReportStatus { get; set; }

        /// <summary>
        /// 贴子狀態
        /// </summary>
        public ReviewStatus Status { get; set; }

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
        /// 会员ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 首页贴
        /// </summary>
        public bool IsHomePost { get; set; }

        /// <summary>
        /// 未通過原因
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 首次送審時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 再次送審時間
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 再次審核時間
        /// </summary>
        public DateTime? ExamineTime { get; set; }

        /// <summary>
        /// 是否營業中。0：休息、1：營業中
        /// </summary>
        public bool? IsOpen { get; set; }

        /// <summary>
        /// 是否營業中
        /// </summary>
        public string IsOpenText => IsOpen.HasValue ? IsOpen.Value ? "营业中" : "休息中" : "-";

        /// <summary>
        /// 前台删除。
        /// </summary>
        public bool? IsDelete { get; set; }

        /// <summary>
        /// 前台删除
        /// </summary>
        public string IsDeleteText => IsDelete.HasValue ? IsDelete.Value ? "删除" : "-" : "-";

        /// <summary>
        /// 已成交单
        /// </summary>
        public int CompletedOrder { get; set; }

        /// <summary>
        /// 首次送审时间
        /// </summary>
        public string CreateTimeText => CreateTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 再次送审时间
        /// </summary>

        public string UpdateTimeText
        {
            get
            {
                if (CreateTime == UpdateTime)
                {
                    return "-";
                }
                else
                {
                    return UpdateTime?.ToString(GlobalSettings.DateTimeFormat) ?? "-";
                }
            }
        }

        /// <summary>
        /// 審核時間
        /// </summary>
        public string ExamineTimeText => ExamineTime?.ToString(GlobalSettings.DateTimeFormat) ?? "-";

        /// <summary>
        /// 会员卡到期时间
        /// </summary>
        public string CardEffectiveTimeText => CardEffectiveTime?.ToString(GlobalSettings.DateTimeFormat) ?? "-";

        /// <summary>
        /// 聯繫資訊
        /// </summary>
        public AdminComboData[] Combo { get; set; } = new AdminComboData[0];

        /// <summary>
        /// 照片來源
        /// </summary>
        public Dictionary<string, string> PhotoSource { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 視頻來源
        /// </summary>
        public Dictionary<string, string> VideoSource { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 观看基础值
        /// </summary>
        public int? ViewBaseCount { get; set; }
    }
}